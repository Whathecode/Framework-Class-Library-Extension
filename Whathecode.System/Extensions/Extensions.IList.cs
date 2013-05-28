using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns the interval of the indices.
		/// </summary>
		/// <typeparam name = "T">Type of the values in the list.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <returns>The interval of the indices of the list.</returns>
		public static Interval<int> GetIndexInterval<T>( this IList<T> source )
		{
			return new Interval<int>( 0, source.Count - 1 );
		}

		/// <summary>
		///   Swaps two items in a list.
		/// </summary>
		/// <typeparam name = "T">Type of the values in the list.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "item1">The item to swap with <see cref="item2" />.</param>
		/// <param name = "item2">The item to swap with <see cref="item1" />.</param>
		public static void Swap<T>( this IList<T> source, T item1, T item2 )
		{
			source.Swap( source.IndexOf( item1 ), source.IndexOf( item2 ) );
		}

		/// <summary>
		///   Swaps two items in a list, specified by their indices.
		/// </summary>
		/// <typeparam name = "T">Type of the values in the list.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "index1">The index of the item to swap with the item at <see cref="index2" />.</param>
		/// <param name = "index2">The index of the item to swap with the item at <see cref="index1" />.</param>
		public static void Swap<T>( this IList<T> source, int index1, int index2 )
		{
			Contract.Requires( source != null );
			Contract.Requires( index1 >= 0 && index1 < source.Count );
			Contract.Requires( index2 >= 0 && index2 < source.Count );

			if ( index1 == index2 )
			{
				return;
			}
			
			T temp = source[ index1 ];
			source[ index1 ] = source[ index2 ];
			source[ index2 ] = temp;
		}
	}
}