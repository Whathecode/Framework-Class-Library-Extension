using System;
using System.Reflection;


namespace Whathecode.System.Reflection
{
    /// <summary>
    ///   A helper class to do common reflection operations.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public static class ReflectionHelper
    {
        /// <summary>
        ///   BindingFlags to return all instance members of a class.
        /// </summary>
        public const BindingFlags AllInstanceMembers =
            BindingFlags.FlattenHierarchy |
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        /// <summary>
        ///   BindingFlags to return all members of a class (static and instance).
        /// </summary>
        public const BindingFlags AllClassMembers = AllInstanceMembers | BindingFlags.Static;

        /// <summary>
        ///   BindingFlags to return all values of an enum type.
        /// </summary>
        public const BindingFlags EnumValues = BindingFlags.Public | BindingFlags.Static;


        const string CastMethodName = "Cast";
        /// <summary>
        ///   Helper method to allow to do dynamic casting during reflection.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <param name="o">The object to cast.</param>
        /// <returns>The object, cast to the specified type.</returns>
        static T Cast<T>( object o )
        {
            return (T)o;
        }

        /// <summary>
        ///   Cast a given object to a desired type.
        /// </summary>
        /// <param name="o">The object to cast.</param>
        /// <param name="type">The type to cast to.</param>
        /// <returns>The object, cast to the desired type.</returns>
        public static object Cast( object o, Type type )
        {
            return typeof( ReflectionHelper )
                .GetMethod( CastMethodName, BindingFlags.Static | BindingFlags.NonPublic )
                .MakeGenericMethod( type )
                .Invoke( null, new [] { o } );
        }
    }
}