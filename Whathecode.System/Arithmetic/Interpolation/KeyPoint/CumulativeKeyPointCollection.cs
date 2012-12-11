using System;
using System.Collections.Generic;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections;
using Whathecode.System.Collections.Algorithm;
using Whathecode.System.Collections.Delegates;
using Whathecode.System.Extensions;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic.Interpolation.KeyPoint
{
	/// <summary>
	///   A collection of key points where the keys have a cumulative relation to the previous key point.
	/// </summary>
	/// <remarks>
	///   For performance reasons, cumulative positions are calculated just in time, based on known, stored changes due to e.g. deletion.
	///   This prevents having to update the entire collection on edits, which could be slow on big collections.
	/// </remarks>
	/// <typeparam name = "TValue">The type of the key points.</typeparam>
	/// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public partial class CumulativeKeyPointCollection<TValue, TMath> : AbstractKeyPointCollection<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   Used to link the key points to their position.
		/// </summary>
		public class KeyPoint : IComparable<KeyPoint>
		{
			public TMath Position;
			public TValue Value;


			#region IComparable<KeyPoint> Members

			public int CompareTo( KeyPoint other )
			{
				return Position.CompareTo( other.Position );
			}

			#endregion
		}


		readonly LazyOperationsList<KeyPoint>
			_data = new LazyOperationsList<KeyPoint>();

		Interval<TMath> _dataRange;
		readonly TMath _zero;
		readonly TMath _minusOne;

		/// <summary>
		///   The range of all the data between which is interpolated.
		///   TODO: Make readonly?
		/// </summary>
		public override Interval<TMath> DataRange
		{
			get { return _dataRange; }
		}

		/// <summary>
		///   Get the key point at a given position when present.
		/// </summary>
		/// <param name = "position">The position for which to get the key point.</param>
		/// <returns>The key point when present.</returns>
		public override TValue this[ TMath position ]
		{
			get
			{
				BinarySearchResult<KeyPoint> result = BinarySearchPosition( position );

				if ( result.IsObjectFound )
				{
					return result.Object.Value;
				}

				throw new IndexOutOfRangeException( "The requested value for index '" + position + "' doesn't exist." );
			}
		}

		/// <summary>
		///   Get the key point at a given index.
		/// </summary>
		/// <param name = "index">The index of the key point to get.</param>
		/// <returns>The key point at the given index.</returns>
		public override TValue this[ int index ]
		{
			get { return _data[ index ].Value; }
		}

		public override int Count
		{
			get { return _data.Count; }
		}


		/// <summary>
		///   Create a new collection of key points with cumulative values.
		/// </summary>
		/// <param name = "typeProvider">
		///   The provider which gives information about the type, required to do interpolation between the key points.
		/// </param>
		public CumulativeKeyPointCollection( AbstractTypeInterpolationProvider<TValue, TMath> typeProvider )
			: base( typeProvider )
		{
			_zero = CastOperator<double, TMath>.Cast( 0 );
			_minusOne = CastOperator<double, TMath>.Cast( -1 );
		}


		public override void Add( TValue value )
		{
			// Get position for the new point.
			TMath position = default(TMath);
			if ( _data.Count > 0 )
			{
				// Add the new key point, with the accumulated distance since the last key point.
				KeyPoint last = _data[ _data.Count - 1 ];
				TMath distance = DistanceBetween( last.Value, value );
				position = Operator<TMath>.Add( last.Position, distance );
			}

			// Create new key point.
			var newKeyPoint = new KeyPoint
			{
				Position = position,
				Value = value
			};

			// Update data range.
			if ( _dataRange == null )
			{
				// First object has zero distance.
				_dataRange = new Interval<TMath>( _zero, _zero );
			}
			else
			{
				DataRange.ExpandTo( position );
			}

			_data.Add( newKeyPoint );
		}

		TMath DistanceBetween( TValue from, TValue to )
		{
			TMath distance = TypeProvider.RelativePosition( from, to );
			return distance.CompareTo( _zero ) == -1
				? Operator<TMath>.Multiply( distance, _minusOne )
				: distance;
		}

		/// <summary>
		///   Removes a key point with the given index.
		/// </summary>
		/// <param name = "index">The index of the key point to remove.</param>
		public override void Remove( int index )
		{
			KeyPoint remove = _data[ index ];

			// Update distance for all data following this index.
			TMath excessDistance = _zero;
			if ( index != _data.Count - 1 )
			{
				KeyPoint next = _data[ index + 1 ];

				// Find excess distance
				excessDistance = DistanceBetween( remove.Value, next.Value );
				if ( index != 0 )
				{
					// Substract new distance from previous point.
					TMath newDistance = DistanceBetween( _data[ index - 1 ].Value, next.Value );
					excessDistance = Operator<TMath>.Subtract( excessDistance, newDistance );
				}

				// Update by adding a pending operation to the range of all following data.
				_data.AddOperation(
					d =>
					{
						d.Position = Operator<TMath>.Subtract( d.Position, excessDistance );
						return d;
					},
					new Interval<int>( index + 1, _data.Count - 1 ) );
			}

			_data.RemoveAt( index );

			// Update data range.
			if ( _data.Count == 0 )
			{
				_dataRange = null;
			}
			else
			{
				_dataRange = new Interval<TMath>(
					_zero,
					Operator<TMath>.Subtract( DataRange.End, excessDistance ) );
			}
		}

		/// <summary>
		///   Removes all key points.
		/// </summary>
		public override void Clear()
		{
			_data.Clear();
			_dataRange = null;
		}

		/// <summary>
		///   Search for the key points at a given position.
		/// </summary>
		/// <param name = "position">The position in the range.</param>
		/// <returns>The found keypoint, or it's nearest matches.</returns>
		public override BinarySearchResult<TMath> BinarySearch( TMath position )
		{
			// Perform a binary search based on position.
			BinarySearchResult<KeyPoint> result = BinarySearchPosition( position );

			return new BinarySearchResult<TMath>
			{
				IsObjectInRange = result.IsObjectInRange,
				IsObjectFound = result.IsObjectFound,
				Object = result.IsObjectFound ? result.Object.Position : default( TMath ),
				Smaller = result.Smaller.Position,
				Bigger = result.Bigger.Position
			};
		}

		public override int IndexAtPosition( TMath position )
		{
			BinarySearchResult<KeyPoint> result = BinarySearchPosition( position );

			return _data.IndexOf( result.Object );
		}

		BinarySearchResult<KeyPoint> BinarySearchPosition( TMath position )
		{
			return BinarySearch<KeyPoint, int>.Search(
				new KeyPoint
				{
					Position = position
				}, // A dummy KeyPoint is created for the search to work.
				_data.GetIndexInterval(),
				new IndexerDelegates<KeyPoint, int>(
					i => _data[ i ],
					i => i ) );
		}

		public override IEnumerator<TValue> GetEnumerator()
		{
			return new Enumerator( _data );
		}
	}
}