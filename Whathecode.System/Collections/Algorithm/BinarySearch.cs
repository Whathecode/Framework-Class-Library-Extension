using System;
using System.Collections.Generic;
using Whathecode.System.Algorithm;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections.Delegates;


namespace Whathecode.System.Collections.Algorithm
{
	/// <summary>
	///   Generic implementation of the binary search algorithm which can operate on any sorted collection of comparable objects
	///   which can be accessed by a linearly complete index.
	///   TODO: Can creating a base DivideAndConquer class be useful?
	/// </summary>
	/// <remarks>
	///   This algorithm will only return valid results when the collection on which it is performed is sorted.
	///   The following is meant with 'linearly complete index'.
	///   Linearly: the distance from any index to the next is always even. E.g. 0, 1, 2, 3, ...
	///   Complete: every value within the specified range is present.
	/// </remarks>
	/// <typeparam name = "TObject">The type of the objects in the collection.</typeparam>
	/// <typeparam name = "TIndex">The type of the index of the collection.</typeparam>
	/// <author>Steven Jeuris</author>
	public class BinarySearch<TObject, TIndex>
		where TObject : IComparable<TObject>
		where TIndex : IComparable<TIndex>
	{
		static readonly EqualityComparer<TIndex> IndexComparer = EqualityComparer<TIndex>.Default;

		/// <summary>
		///   Searches for the given object.
		/// </summary>
		/// <param name = "toFind">The object to search for.</param>
		/// <param name = "range">The range in which to search.</param>
		/// <param name = "indexOperations">Operations that can be done on an indexer.</param>
		/// <returns>The found object, or it's nearest matches.</returns>
		public static BinarySearchResult<TObject> Search(
			TObject toFind,
			IInterval<TIndex> range,
			IndexerDelegates<TObject, TIndex> indexOperations )
		{
			// Make sure start and end of range are valid.
			// TODO: This is only necessary the first call of the recursion. Implementing the search as a loop could be more performant.
			range = new Interval<TIndex>(
				indexOperations.GetNearestIndex( range.Start ),
				indexOperations.GetNearestIndex( range.End ) );

			// Get object near the center of the range.
			TIndex center = indexOperations.GetNearestIndex( range.Center );
			TObject centerObject = indexOperations.GetByIndex( center );

			// Check whether desired object was found.
			int orderToCenter = toFind.CompareTo( centerObject );

			// See whether finished.
			bool isObjectFound = orderToCenter == 0;
			bool hasNoMoreObjects = IndexComparer.Equals( center, range.Start ) || IndexComparer.Equals( center, range.End );
			bool isFinished = isObjectFound || hasNoMoreObjects;

			if ( !isFinished )
			{
				// Split interval in the middle.
				IInterval<TIndex> smaller, bigger;
				range.Split( center, SplitOption.Both, out smaller, out bigger );

				// Continue recursively in the range in which the object lies.
				IInterval<TIndex> inRange = orderToCenter > 0 ? bigger : smaller;
				return Search( toFind, inRange, indexOperations );
			}
			else
			{
				TObject smaller = indexOperations.GetByIndex( range.Start );
				TObject bigger = indexOperations.GetByIndex( range.End );

				// Find the desired object.
				TObject foundObject = default(TObject);
				if ( isObjectFound )
				{
					foundObject = centerObject;
				}
				else
				{
					if ( toFind.CompareTo( smaller ) == 0 )
					{
						isObjectFound = true;
						foundObject = smaller;
					}
					else if ( toFind.CompareTo( bigger ) == 0 )
					{
						isObjectFound = true;
						foundObject = bigger;
					}
				}

				// Return result.
				return new BinarySearchResult<TObject>
				{
					IsObjectInRange = toFind.CompareTo( smaller ) >= 0 && toFind.CompareTo( bigger ) <= 0,
					IsObjectFound = isObjectFound,
					Object = foundObject,
					Smaller = smaller,
					Bigger = bigger
				};
			}
		}
	}
}