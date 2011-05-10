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
        ///   BindingFlags to return all members of a class.
        /// </summary>
        public const BindingFlags AllClassMembers =
            BindingFlags.FlattenHierarchy |
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.Static;

        /// <summary>
        ///   BindingFlags to return all values of an enum type.
        /// </summary>
        public const BindingFlags EnumValues = BindingFlags.Public | BindingFlags.Static;
    }
}