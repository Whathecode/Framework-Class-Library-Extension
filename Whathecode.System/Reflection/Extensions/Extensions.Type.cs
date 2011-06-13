using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Searches for the specified method whose parameters match the specified argument types,
        ///   using the specified binding constraints.
        /// </summary>
        /// <remarks>
        ///   This is a simple overload which is missing in the original library.
        /// </remarks>
        /// <param name = "source">The source for this extension method.</param>
        /// <param name = "name">The string containing the name of the public method to get.</param>
        /// <param name = "flags"></param>
        /// <param name = "types">
        ///   An array of <see cref = "Type" /> objects representing the number, order, and type of the parameters for the method to get.
        ///   -or-
        ///   An empty array of <see cref = "Type" /> objects (as provided by the EmptyTypes field) to get a method that takes no parameters.
        /// </param>
        /// <returns>
        ///   An object representing the method that matches the requirements
        ///   and whose parameters match the specified argument types, if found; otherwise, null.
        /// </returns>
        public static MethodInfo GetMethod( this Type source, string name, BindingFlags flags, Type[] types )
        {
            return source.GetMethod( name, flags, Type.DefaultBinder, types, null );
        }

        /// <summary>
        ///   Get the first found matching generic type.
        ///   The type parameters of the generic type are optional.
        ///   E.g. Dictionary&lt;,&gt; or Dictionary&lt;string,&gt;
        ///   When full (generic) type is known (e.g. Dictionary&lt;string,string&gt;),
        ///   the "is" operator is most likely more performant, but this function will still work correctly.
        /// </summary>
        /// <param name = "source">The source for this extension method.</param>
        /// <param name = "type">The type to check for.</param>
        /// <returns>The first found matching complete generic type, or null when no matching type found.</returns>
        public static Type GetMatchingGenericType( this Type source, Type type )
        {
            Type[] genericArguments = type.GetGenericArguments();
            Type rawType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            // Used to compare type arguments and see whether they match.
            Func<Type[], bool> argumentsMatch
                = arguments => genericArguments
                                   .Zip( arguments, Tuple.Create )
                                   .All( t => t.Item1.IsGenericParameter || // No type specified.
                                              t.Item1 == t.Item2 );

            Type matchingType = null;
            if ( type.IsInterface )
            {
                // Traverse across all interfaces to find a matching interface.
                matchingType = (from t in source.GetInterfaces()
                                let rawInterface = t.IsGenericType ? t.GetGenericTypeDefinition() : t
                                where rawInterface == rawType && argumentsMatch( t.GetGenericArguments() )
                                select t).FirstOrDefault();
            }
            else
            {
                // Traverse across the type, and all it's base types.
                Type baseType = source;
                while ( baseType != null && baseType != typeof( object ) )
                {
                    Type rawCurrent = baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType;
                    if ( rawType == rawCurrent )
                    {
                        // Same raw generic type, compare type arguments.
                        if ( argumentsMatch( baseType.GetGenericArguments() ) )
                        {
                            matchingType = baseType;
                            break;
                        }
                    }
                    baseType = baseType.BaseType;
                }
            }

            return matchingType;
        }

        /// <summary>
        ///   Is a certain type of a given generic type or not.
        ///   Also works for raw generic types. E.g. Dictionary&lt;,&gt;.
        ///   When full (generic) type is known (e.g. Dictionary&lt;string,string&gt;),
        ///   the "is" operator is most likely more performant, but this function will still work correctly.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "type">The type to check for.</param>
        /// <returns>True when the type is of the given type, false otherwise.</returns>
        public static bool IsOfGenericType( this Type source, Type type )
        {
            return GetMatchingGenericType( source, type ) != null;
        }

        /// <summary>
        ///   Verify whether a given type is an enum with the <see cref = "FlagsAttribute" /> applied.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <returns>True when the given type is a flags enum, false otherwise.</returns>
        public static bool IsFlagsEnum( this Type source )
        {
            return source.IsEnum && source.GetAttribute<FlagsAttribute>() != null;
        }

        /// <summary>
        ///   Does a certain type implement a given interface or not.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "interfaceType">The interface type to check for.</param>
        /// <returns>True when the type implements the given interface, false otherwise.</returns>
        public static bool ImplementsInterface( this Type source, Type interfaceType )
        {
            return source.GetInterface( interfaceType.ToString() ) != null;
        }

        /// <summary>
        ///   Create a default initialisation of the given object type.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <returns>The default initialisation for the given objectType.</returns>
        public static object CreateDefault( this Type source )
        {
            return (source.IsValueType ? Activator.CreateInstance( source ) : null);
        }

        /// <summary>
        ///   Return all members of a specific type in a certain type.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "type">The type to search for.</param>
        /// <returns>A list of all object members with the specific type.</returns>
        public static IEnumerable<MemberInfo> GetMembers( this Type source, Type type )
        {
            return from m in source.GetMembers( ReflectionHelper.AllClassMembers )
                   where (m is FieldInfo ||
                          m is PropertyInfo ||
                          m is EventInfo)
                   where m.GetMemberType().IsOfGenericType( type )
                   select m;
        }

        /// <summary>
        ///   Returns all members which have a specified attribute annotated to them.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "memberTypes">The type of members to search in.</param>
        /// <param name = "inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
        /// <param name = "bindingFlags">
        ///   A bitmask comprised of one or more <see cref = "BindingFlags" /> that specify how the search is conducted.
        ///   -or-
        ///   Zero, to return null.
        /// </param>
        /// <typeparam name = "TAttribute">The type of the attributes to search for.</typeparam>
        /// <returns>A dictionary containing all members with their attached attributes.</returns>
        public static Dictionary<MemberInfo, TAttribute[]> GetAttributedMembers<TAttribute>(
            this Type source,
            MemberTypes memberTypes = MemberTypes.All,
            bool inherit = false,
            BindingFlags bindingFlags = ReflectionHelper.AllClassMembers )
            where TAttribute : Attribute
        {
            return (from member in source.GetMembers( bindingFlags )
                    from attribute in (Attribute[])member.GetCustomAttributes( typeof( TAttribute ), inherit )
                    where member.MemberType.HasFlag( memberTypes )
                    group attribute by member).ToDictionary( g => g.Key, g => g.Cast<TAttribute>().ToArray() );
        }

        public static IEnumerable<MethodInfo> GetFlattenedInterfaceMethods( this Type source, BindingFlags bindingFlags )
        {
            foreach ( var info in source.GetMethods( bindingFlags ) )
            {
                yield return info;
            }

            var flattened = source.GetInterfaces().SelectMany( interfaceType => GetFlattenedInterfaceMethods( interfaceType, bindingFlags ) );
            foreach ( var info in flattened )
            {
                yield return info;
            }
        }
    }
}