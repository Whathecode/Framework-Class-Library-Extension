using System;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Get the value of a property of an object.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "property">The property to get.</param>
        /// <returns>The value of the property of the object.</returns>
        public static object GetPropertyValue( this object source, string property )
        {
            PropertyInfo info = source.GetType().GetProperty( property );

            if ( info == null )
            {
                throw new ArgumentException( "The property with name \"" + property + "\" wasn't found.", "property" );
            }

            return info.GetValue( source, null );
        }

        /// <summary>
        ///   Returns the value of a given member of an object.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "member">The member of the object to get the value from.</param>
        /// <returns>The value of the member of the object.</returns>
        public static object GetValue( this object source, MemberInfo member )
        {
            object value;
            if ( member is FieldInfo )
            {
                FieldInfo field = (FieldInfo)member;
                value = field.GetValue( source );
            }
            else if ( member is PropertyInfo )
            {
                PropertyInfo property = (PropertyInfo)member;
                value = property.GetValue( source, null );
            }
            else
            {
                throw new NotImplementedException( "Can't return value of the given member." );
            }

            return value;
        }

        /// <summary>
        ///   Do a bitwise or on two enum values of which the underlying type is unknown.
        /// </summary>
        /// <returns>The bitwise or of both enum values.</returns>
        public static object EnumOr( this object firstEnumValue, object secondEnumValue )
        {
            Type firstType = firstEnumValue.GetType();
            Type secondType = secondEnumValue.GetType();

            if ( !firstType.IsEnum || !secondType.IsEnum )
            {
                throw new ArgumentException( "Only enum values are valid for this function." );
            }
            if ( !(firstType == secondType) )
            {
                throw new ArgumentException( "Both enum values should be of the same type." );
            }
             
            // Cast to long so no bits are lost during the or operation.
            ulong bits = Convert.ToUInt64( firstEnumValue ) | Convert.ToUInt64( secondEnumValue );

            // Cast back to underlying type, followed by cast to the enum type.
            object underlyingValue = Convert.ChangeType( bits, firstType.GetEnumUnderlyingType() );
            return underlyingValue.Cast( firstType );
        }

        /// <summary>
        ///   Cast a given object to a desired type.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "type">The type to cast to.</param>
        /// <returns>The object, cast to the desired type.</returns>
        public static object Cast( this object source, Type type )
        {
            return ReflectionHelper.Cast( source, type );
        }
    }
}
