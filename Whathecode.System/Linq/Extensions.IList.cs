using System;
using System.Collections.Generic;


namespace Whathecode.System.Linq
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Check whether the last value equals the given value.
        /// </summary>
        /// <typeparam name = "T">Type of the values in the list.</typeparam>
        /// <param name = "source">The source for this extension method.</param>
        /// <param name = "value">Value to check whether its the last value.</param>
        /// <returns>True when the given value is last, false otherwise.</returns>
        public static bool IsLast<T>( this IList<T> source, T value )
            where T : IComparable
        {
            return value.CompareTo( source[ source.Count - 1 ] ) == 0;
        }

        /// <summary>
        ///   Bypasses a specified number of elements in a sequence and then returns the remaining elements, optimized for IList{T}.
        /// </summary>
        /// <typeparam name = "T">Type of the values in the list.</typeparam>
        /// <param name = "source">An IList{T} to return elements from.</param>
        /// <param name = "count" >The number of elements to skip before returning the remaining elements.</param>
        /// <returns>An IEnumerable{T} that contains the elements that occur after the specified index in the input sequence.</returns>
        public static IEnumerable<T> Skip<T>( this IList<T> source, int count )
        {
            using ( var e = source.GetEnumerator() )
            {
                // MoveNext is only called to enable the exception side effect when the collection was modified.
                while ( count < source.Count && e.MoveNext() )
                {
                    yield return source[ count++ ];
                }
            }
        }

    }
}