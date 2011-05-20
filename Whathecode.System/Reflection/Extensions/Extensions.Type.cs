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

            // Traverse across the type, and all it's base types.
            while ( source != null && source != typeof( object ) )
            {
                Type rawCurrent = source.IsGenericType ? source.GetGenericTypeDefinition() : source;
                if ( rawType == rawCurrent )
                {
                    // Same raw generic type, compare type arguments.
                    Type[] arguments = source.GetGenericArguments();
                    if ( arguments.Length == genericArguments.Length )
                    {
                        bool argumentsMatch = true;
                        for ( int i = 0; i < genericArguments.Length; ++i )
                        {
                            Type genericArgument = genericArguments[ i ];
                            Type argument = arguments[ i ];
                            if ( !genericArgument.IsGenericParameter && // No type specified.
                                 genericArgument != argument )
                            {
                                argumentsMatch = false;
                                break;
                            }
                        }

                        if ( argumentsMatch )
                        {
                            return source;
                        }
                    }
                }
                source = source.BaseType;
            }

            return null;
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
    }
}