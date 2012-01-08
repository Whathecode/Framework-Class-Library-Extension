using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Operators;


namespace Whathecode.System.Collections
{
	/// <summary>
	///   A collection containing objects in intervals.
	/// </summary>
	/// <remarks>
	///   No overlapping intervals are stored internally. When they overlap, they are split and merged.
	///   TODO: This is a quick simple implementation. An interval tree could be used internally to make this more performant.
	/// </remarks>
	/// <typeparam name = "TMath">The value type to use for the interval calculations.</typeparam>
	/// <typeparam name = "TObject">The object types in the calculation.</typeparam>
	/// <author>Steven Jeuris</author>
	public class IntervalCollection<TMath, TObject>
		: IEnumerable<IntervalCollection<TMath, TObject>.IntervalValues>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   Defines values available for a certain interval.
		/// </summary>
		public class IntervalValues : IComparable<IntervalValues>
		{
			public IList<TObject> Values { get; private set; }

			public Interval<TMath> Interval { get; private set; }


			public IntervalValues( IList<TObject> values, Interval<TMath> interval )
			{
				Values = values;
				Interval = interval;
			}


			public int CompareTo( IntervalValues other )
			{
				int intervalStartCompare = Interval.Start.CompareTo( other.Interval.Start );

				if ( intervalStartCompare == 0 && (Interval.IsStartIncluded != other.Interval.IsStartIncluded) )
				{
					// Start is the same, but not included in both.
					return Interval.IsStartIncluded ? 1 : -1;
				}

				return intervalStartCompare;
			}
		}


		readonly SortedSet<IntervalValues> _rangedObjects = new SortedSet<IntervalValues>();


		/// <summary>
		///   Add an object to a given interval.
		/// </summary>
		/// <param name = "interval">The interval to store the object in.</param>
		/// <param name = "value">The object to store.</param>
		public void Add( Interval<TMath> interval, TObject value )
		{
			Contract.Requires( value != null );

			Add( interval, new List<TObject> { value } );
		}

		/// <summary>
		///   Add a list of objects to a given interval.
		/// </summary>
		/// <param name = "interval">The interval to store the object in.</param>
		/// <param name = "values">The list of objects to store.</param>
		public void Add( Interval<TMath> interval, IList<TObject> values )
		{
			// Check for intersections with existing intervals.
			IList<IntervalValues> intersecting = FindIntersections( interval );

			if ( intersecting.Count > 0 )
			{
				// A list used to find remnants after cutting intersections.
				List<Interval<TMath>> remnants = new List<Interval<TMath>>
				{
					(Interval<TMath>)interval.Clone()
				};

				// Split up all intersecting intervals.
				foreach ( var intersectingRange in intersecting )
				{
					Interval<TMath> intersection = intersectingRange.Interval.Intersection( interval );

					SplitRemoveIntersection( intersectingRange, intersection );

					// Add intersection with objects of both intervals.
					List<TObject> mergedObjects = new List<TObject>( intersectingRange.Values );
					mergedObjects.AddRange( values );
					_rangedObjects.Add( new IntervalValues( mergedObjects, intersection ) );

					// Remove intersections from remnants.
					List<Interval<TMath>> newRemnants = new List<Interval<TMath>>();
					foreach ( var remnant in remnants )
					{
						newRemnants.AddRange( remnant.Subtract( intersection ) );
					}
					remnants = newRemnants;
				}

				// Add remnants of the newly added interval.
				foreach ( var remnant in remnants )
				{
					_rangedObjects.Add( new IntervalValues( values, remnant ) );
				}
			}
			else
			{
				// No intersections, just add.
				_rangedObjects.Add( new IntervalValues( values, interval ) );
			}
		}

		/// <summary>
		///   Partially removes a given interval from an object range, so only the non intersecting part remains.
		/// </summary>
		/// <param name = "objectRange">The range of which to remove and split a given interval.</param>
		/// <param name = "interval">The interval to remove from the range.</param>
		void SplitRemoveIntersection( IntervalValues objectRange, Interval<TMath> interval )
		{
			// Remove original range.
			_rangedObjects.Remove( objectRange );

			// Add non intersection parts.
			foreach ( var remaining in objectRange.Interval.Subtract( interval ) )
			{
				_rangedObjects.Add( new IntervalValues( objectRange.Values, remaining ) );
			}
		}

		/// <summary>
		///   Remove an entire interval and its objects.
		/// </summary>
		/// <param name = "objectRange">The interval with its objects to remove.</param>
		/// <returns>True when successful, false otherwise.</returns>
		public bool Remove( IntervalValues objectRange )
		{
			return _rangedObjects.Remove( objectRange );
		}

		/// <summary>
		///   Removes an entire interval and all objects present in that interval from the collection.
		/// </summary>
		/// <param name = "interval">The interval to remove.</param>
		public void RemoveInterval( Interval<TMath> interval )
		{
			foreach ( var intersectingRange in FindIntersections( interval ) )
			{
				Interval<TMath> intersection = intersectingRange.Interval.Intersection( interval );

				SplitRemoveIntersection( intersectingRange, intersection );
			}
		}

		/// <summary>
		///   Moves all intervals that lie within the given interval.
		/// </summary>
		/// <param name = "interval">The interval which specifies which elements will be moved.</param>
		/// <param name = "offset">The offset where to move the values in the interval.</param>
		public void MoveInterval( Interval<TMath> interval, TMath offset )
		{
			// Keep track of the intervals that need to be moved.
			List<IntervalValues> movedIntervals = new List<IntervalValues>();

			foreach ( var intersectingRange in FindIntersections( interval ) )
			{
				Interval<TMath> intersection = intersectingRange.Interval.Intersection( interval );

				// Remove part of the interval which will be moved, static parts can stay.
				SplitRemoveIntersection( intersectingRange, intersection );

				// Add moved remainder to list to be added again.
				Interval<TMath> movedInterval = new Interval<TMath>(
					Operator<TMath>.Add( intersection.Start, offset ),
					intersection.IsStartIncluded,
					Operator<TMath>.Add( intersection.End, offset ),
					intersection.IsEndIncluded );
				movedIntervals.Add( new IntervalValues( intersectingRange.Values, movedInterval ) );
			}

			// Add moved intervals again.
			foreach ( var newRange in movedIntervals )
			{
				Add( newRange.Interval, newRange.Values );
			}
		}

		/// <summary>
		///   Returns all the available elements at a given position.
		/// </summary>
		/// <param name = "position">The position at which to look for elements.</param>
		/// <returns>A list of all the elements found at the given position.</returns>
		public List<TObject> ElementsAt( TMath position )
		{
			return (
				from objectRange in _rangedObjects
				where objectRange.Interval.LiesInInterval( position )
				select objectRange.Values
				).SelectMany( o => o ).ToList();
		}

		/// <summary>
		///   Check whether the given interval with containing objects lies in this collection.
		/// </summary>
		/// <param name = "item">The interval with its objects to check whether its contained in the collection.</param>
		/// <returns>True when the interval and its objects lie in the collection, false otherwise.</returns>
		public bool Contains( IntervalValues item )
		{
			return _rangedObjects.Contains( item );
		}

		/// <summary>
		///   Clears all intervals and it's objects.
		/// </summary>
		public void Clear()
		{
			_rangedObjects.Clear();
		}

		public void CopyTo( IntervalValues[] array, int arrayIndex )
		{
			_rangedObjects.CopyTo( array, arrayIndex );
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		List<IntervalValues> FindIntersections( Interval<TMath> interval )
		{
			return (
				from o in _rangedObjects
				where o.Interval.Intersects( interval )
				select o
				).ToList();
		}


		#region IEnumerator<IntervalCollection<TMath, TObject>.IntervalValues>

		public IEnumerator<IntervalValues> GetEnumerator()
		{
			return _rangedObjects.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion  // IEnumerator<IntervalCollection<TMath, TObject>.IntervalValues>
	}
}