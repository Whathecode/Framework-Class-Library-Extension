using System.Collections.Generic;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Returns the interval of the indices.
        /// </summary>
        /// <typeparam name="T">Type of the values in the list.</typeparam>
        /// <param name="source">The source for this extension method.</param>
        /// <returns>The interval of the indices of the list.</returns>
        public static Interval<int> GetIndexInterval<T>( this IList<T> source )
        {
            return new Interval<int>( 0, source.Count - 1 );
        }
    }
}
