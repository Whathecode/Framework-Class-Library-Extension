using System;
using System.Collections.Generic;
using System.Linq;


namespace Whathecode.System
{
    /// <summary>
    ///   A generic helper class to do common <see cref = "Enum">Enum</see> operations.
    /// </summary>
    /// <typeparam name = "T">The type of the enum.</typeparam>
    /// <author>Steven Jeuris</author>
    public static class EnumHelper<T>
    {
        /// <summary>
        ///   Converts the string representation of the name or numeric value of one or more enumerated constants (seperated by comma)
        ///   to an equivalent enumerated object. Case-sensitive operation.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        public static T Parse( string value )
        {
            return Parse( value, false );
        }

        /// <summary>
        ///   Converts the string representation of the name or numeric value of one or more enumerated constants (seperated by comma)
        ///   to an equivalent enumerated object. A parameter specifies whether the operations is case-sensitive.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">True to ignore case; false to regard case.</param>
        public static T Parse( string value, bool ignoreCase )
        {
            return (T)Enum.Parse( typeof( T ), value, ignoreCase );
        }

        /// <summary>
        ///   Retrieves an enumerator of the constants in a specified enumeration.
        /// </summary>
        /// <returns>Enumerable which can be used to enumerate over all constants in the specified enumeration.</returns>
        public static IEnumerable<T> GetValues()
        {
            return from object value in Enum.GetValues( typeof( T ) )
                   select (T)value;
        }
    }
}