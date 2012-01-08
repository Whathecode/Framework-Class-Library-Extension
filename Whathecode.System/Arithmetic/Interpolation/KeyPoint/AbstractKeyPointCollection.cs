using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections.Algorithm;


namespace Whathecode.System.Arithmetic.Interpolation.KeyPoint
{
	/// <summary>
	///   A collection of key points to be used by AbstractInterpolation.
	///   The key points are accessible by index.
	/// </summary>
	/// <typeparam name = "TValue">The type of key points.</typeparam>
	/// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	[ContractClass( typeof( AbstractKeyPointCollectionContract<,> ) )]
	public abstract class AbstractKeyPointCollection<TValue, TMath> : IEnumerable<TValue>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   The provider which gives information about the type, required to do interpolation.
		/// </summary>
		public AbstractTypeInterpolationProvider<TValue, TMath> TypeProvider { get; private set; }


		#region Abstract Properties

		/// <summary>
		///   The range of all the data between which is interpolated.
		///   TODO: Make readonly?
		/// </summary>
		public abstract Interval<TMath> DataRange { get; }

		/// <summary>
		///   Get the key point at a given position when present.
		/// </summary>
		/// <param name = "position">The position for which to get the key point.</param>
		/// <returns>The key point when present.</returns>
		public abstract TValue this[ TMath position ] { get; }

		/// <summary>
		///   Get the key point at a given index.
		/// </summary>
		/// <param name = "index">The index of the key point to get.</param>
		/// <returns>The key point at the given index.</returns>
		public abstract TValue this[ int index ] { get; }

		public abstract int Count { get; }

		#endregion  // Abstract Properties


		/// <summary>
		///   Create a new collection of key points.
		/// </summary>
		/// <param name = "typeProvider">
		///   The provider which gives information about the type, required to do interpolation between the key points.
		/// </param>
		protected AbstractKeyPointCollection( AbstractTypeInterpolationProvider<TValue, TMath> typeProvider )
		{
			TypeProvider = typeProvider;
		}


		/// <summary>
		///   Add new key points.
		/// </summary>
		/// <param name = "keyPoints">The key points to add.</param>
		public void Add( IEnumerable<TValue> keyPoints )
		{
			foreach ( var kp in keyPoints )
			{
				Add( kp );
			}
		}


		#region Abstract Functions

		/// <summary>
		///   Add a new key point.
		/// </summary>
		/// <param name = "keyPoint">The key point to add.</param>
		public abstract void Add( TValue keyPoint );

		/// <summary>
		///   Removes a key point with the given index.
		/// </summary>
		/// <param name = "index">The index of the key point to remove.</param>
		public abstract void Remove( int index );

		/// <summary>
		///   Clears all key points.
		/// </summary>
		public abstract void Clear();

		/// <summary>
		///   Search for the key points at a given position.
		/// </summary>
		/// <param name = "position">The position in the range.</param>
		/// <returns>The found keypoint, or it's nearest matches.</returns>
		public abstract BinarySearchResult<TMath> BinarySearch( TMath position );

		public abstract int IndexAtPosition( TMath position );

		#endregion  // Abstract Functions


		#region IEnumerable

		public abstract IEnumerator<TValue> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion  // IEnumerable
	}


	[ContractClassFor( typeof( AbstractKeyPointCollection<,> ) )]
	abstract class AbstractKeyPointCollectionContract<TValue, TMath> : AbstractKeyPointCollection<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		protected AbstractKeyPointCollectionContract( AbstractTypeInterpolationProvider<TValue, TMath> typeProvider )
			: base( typeProvider ) {}


		public override void Add( TValue keyPoint )
		{
			Contract.Requires( keyPoint != null );
		}

		public override void Remove( int index )
		{
			Contract.Requires( index >= 0 );
		}
	}
}