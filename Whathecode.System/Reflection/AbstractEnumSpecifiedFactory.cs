using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Reflection
{
    /// <summary>
    ///   An abstract factory which allows to create instances specified by an enum ID.
    ///   Reflection is used to get information on what to create through IdAttributes applied to class members.
    ///   The ID's of the IdAttributes should match the enum values.
    /// </summary>
    /// <typeparam name = "TEnum">An enum used to specify the instances to create.</typeparam>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractEnumSpecifiedFactory<TEnum> : AbstractAttributeReflectionFactory
    {
        protected Type EnumType { get; private set; }

        protected TEnum[] EnumValues { get; private set; }

        /// <summary>
        ///   Specifies whether every enum value should be used in the owner class,
        ///   or an exception will be thrown.
        /// </summary>
        protected bool RequireUsageOfAllEnums { get; private set; }

        /// <summary>
        ///   The list of members with all relevent IdAttribute's added to it.
        /// </summary>
        protected Dictionary<MemberInfo, IdAttribute[]> MatchingAttributes { get; private set; }


        /// <summary>
        ///   Create a new factory, using the outer class of the type parameter as owner type.
        /// </summary>
        protected AbstractEnumSpecifiedFactory()
            : this( null, false ) {}


        /// <summary>
        ///   Create a new factory for a specified owner type.
        /// </summary>
        /// <param name = "ownerType">The type to create this factory for.</param>
        /// <param name = "requireUsageOfAllEnums">
        ///   When true, an exception is thrown when not all enum values are used in the owner class.
        /// </param>
        protected AbstractEnumSpecifiedFactory( Type ownerType, bool requireUsageOfAllEnums )
            : base( ownerType )
        {
            RequireUsageOfAllEnums = requireUsageOfAllEnums;

            // Check whether the ID type is an enum.
            EnumType = typeof( TEnum );
            if ( !EnumType.IsEnum )
            {
                throw new ArgumentException(
                    "This factory requires an enum identifying the instances to create, " +
                    "which should be passed as the template parameter." );
            }
            EnumValues = EnumHelper<TEnum>.GetValues().ToArray();

            // Check whether the type parameter has an owner type when no owner set.
            if ( OwnerType == null )
            {
                if ( EnumType.IsNested && EnumType.DeclaringType != null )
                {
                    OwnerType = EnumType.DeclaringType;
                }
                else
                {
                    throw new ArgumentException(
                        "When not specifying an owner type in the constructor, " +
                        "the enum type passed as a template parameter should be nested inside the desired owner." );
                }
            }

            // Find all attributes with correct ID's set.
            MatchingAttributes = (from member in AttributedMembers
                                  from attribute in member.Value
                                  let idAttribute = attribute as IdAttribute
                                  where idAttribute != null && idAttribute.GetId() is TEnum
                                  group idAttribute by member).ToDictionary( g => g.Key.Key, v => v.ToArray() );

            // Should every enum have at least one matching member?
            if ( RequireUsageOfAllEnums )
            {
                int enumsUsed = (from match in MatchingAttributes
                                 from id in match.Value
                                 select id.GetId()).Distinct().Count();

                if ( EnumValues.Length != enumsUsed )
                {
                    throw new ArgumentException(
                        "Not all enum values of the template parameter type \"" + typeof( TEnum ) +
                        "\" have a class member with a matching attribute ID set in the type '" +
                        OwnerType.Name + "'." );
                }
            }
        }

        /// <summary>
        ///   Get members attributed with a certain IdAttribute with a specific ID.
        /// </summary>
        /// <typeparam name = "TAttributeType">The type of IdAttribute to look for.</typeparam>
        /// <param name = "id">The id the IdAttribute should have.</param>
        /// <returns>All matching IdAttributes for every member where matches were found.</returns>
        protected Dictionary<MemberInfo, IdAttribute[]> GetAttributedMembers<TAttributeType>( TEnum id )
            where TAttributeType : IdAttribute
        {
            return (from member in OwnerType.GetMembers( ReflectionHelper.AllClassMembers )
                    from attribute in (IdAttribute[])member.GetCustomAttributes( typeof( TAttributeType ), false )
                    where attribute.GetId().Equals( id )
                    group attribute by member).ToDictionary( g => g.Key, g => g.ToArray() );
        }
    }
}