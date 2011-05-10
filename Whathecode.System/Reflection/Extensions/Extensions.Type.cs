using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
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
        ///   Also works for raw generic types, or (partially) specified generic types.
        ///   E.g. Dictionary&lt,&gt, Dictionary&ltstring,&gt
        ///   When full (generic) type is known (e.g. Dictionary&ltstring,string&gt),
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
        ///   Does a certain type implement a given interface or not.
        /// </summary>
        /// <param name="source">The source of this extension method.</param>
        /// <param name="interfaceType">The interface type to check for.</param>
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
        public static MemberInfo[] GetMembers( this Type source, Type type )
        {
            IEnumerable<MemberInfo> typeMembers = from m in source.GetMembers( ReflectionHelper.AllClassMembers )
                                                  where (m is FieldInfo ||
                                                         m is PropertyInfo ||
                                                         m is EventInfo)
                                                  where m.GetMemberType().IsOfGenericType( type )
                                                  select m;

            return typeMembers.ToArray();
        }
    }
}