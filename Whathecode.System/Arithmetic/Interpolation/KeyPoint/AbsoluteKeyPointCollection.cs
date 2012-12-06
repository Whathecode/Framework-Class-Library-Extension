using System;
using System.Collections.Generic;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections.Algorithm;
using Whathecode.System.Extensions;


namespace Whathecode.System.Arithmetic.Interpolation.KeyPoint
{
	/// <summary>
	///   A collection of key points where the keys specify absolute values in the space to be interpolated.
	/// </summary>
	/// <typeparam name = "TValue">The type of the key points.</typeparam>
	/// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public class AbsoluteKeyPointCollection<TValue, TMath> : AbstractKeyPointCollection<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   The list of data between which is interpolated, with it's position as key.
		/// </summary>
		readonly SortedList<TMath, TValue> _data = new SortedList<TMath, TValue>();

		/// <summary>
		///   Reference keypoint, used to calculate distance to newly added values.
		/// </summary>
		readonly TValue _referenceKeyPoint;

		public override Interval<TMath> DataRange
		{
			get { return _data.GetKeysInterval(); }
		}

		public override TValue this[ TMath position ]
		{
			get { return _data[ position ]; }
		}

		public override TValue this[ int index ]
		{
			get { return _data[ _data.Keys[ index ] ]; }
		}

		public override int Count
		{
			get { return _data.Count; }
		}


		/// <summary>
		///   Create a new collection of key points with absolute values.
		/// </summary>
		/// <param name = "typeProvider">
		///   The provider which gives information about the type, required to do interpolation between the key points.
		/// </param>
		/// <param name = "referenceKeyPoint">
		///   A key point which is used to measure the distance to all other added key points.
		///   Don't adjust this key point after adding it.
		/// </param>
		public AbsoluteKeyPointCollection( AbstractTypeInterpolationProvider<TValue, TMath> typeProvider, TValue referenceKeyPoint )
			: base( typeProvider )
		{
			_referenceKeyPoint = referenceKeyPoint;
		}


		/// <summary>
		///   Add a key point.
		/// </summary>
		/// <param name = "keyPoint">The key point to add.</param>
		public override void Add( TValue keyPoint )
		{			
			// Add keypoint, with it's relative position to the reference point as a key.
			TMath relativePosition = TypeProvider.RelativePosition( keyPoint, _referenceKeyPoint );
			_data.Add( relativePosition, keyPoint );
		}

		/// <summary>
		///   Removes a key point with the given index.
		/// </summary>
		/// <param name = "index">The index of the key point to remove.</param>
		public override void Remove( int index )
		{			
			_data.RemoveAt( index );
		}

		/// <summary>
		///   Removes all key points.
		/// </summary>
		public override void Clear()
		{
			_data.Clear();
		}

		public override BinarySearchResult<TMath> BinarySearch( TMath position )
		{
			return _data.BinarySearchKeys( position );
		}

		public override int IndexAtPosition( TMath position )
		{
			return _data.IndexOfKey( position );
		}

		public override IEnumerator<TValue> GetEnumerator()
		{
			return _data.Values.GetEnumerator();
		}
	}
}