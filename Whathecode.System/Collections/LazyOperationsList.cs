using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Collections
{
	/// <summary>
	///   A list which allows for modifications applied to a subrange of its elements, to be applied just in time
	///   as the elements are requested.
	///   This is useful for big lists, with identical changes applied over big ranges.
	/// </summary>
	/// <remarks>
	///   TODO: Storing the modifications could be done more easily by using an interval tree.
	///   At the moment a simple sorted list is used containing non-overlapping intervals, with their operations.
	///   TODO: Test the performance gain/loss of this on a big collection.
	///   TODO: By using intervalcollection, 'empty' intervals are left behind which can't contain any values for this implementation.
	///   E.g. ]0,1[
	/// </remarks>
	/// <typeparam name = "TObject">The type of the objects in the list.</typeparam>
	/// <author>Steven Jeuris</author>
	public partial class LazyOperationsList<TObject> : IList<TObject>
	{
		readonly List<TObject> _list;
		readonly IntervalCollection<int, Func<TObject, TObject>> _pendingOperations = new IntervalCollection<int, Func<TObject, TObject>>();


		/// <summary>
		///   Create a new list which allows for lazy operations.
		/// </summary>
		public LazyOperationsList()
		{
			_list = new List<TObject>();
		}


		/// <summary>
		///   Add a new operation which will only be performed when required.
		/// </summary>
		/// <param name = "operation">The operation to be done on all the objects in the given range, returning the updated object.</param>
		/// <param name = "range">The range on which to perform the operation.</param>
		public void AddOperation( Func<TObject, TObject> operation, IInterval<int> range )
		{
			Contract.Requires( range.Start >= 0 && range.End < Count );

			_pendingOperations.Add( range, operation );
		}

		/// <summary>
		///   Removes an element at the given index.
		/// </summary>
		/// <param name = "index">The index of the element to remove.</param>
		public void RemoveAt( int index )
		{
			// Update pending operations to reflect the removed item.
			_pendingOperations.RemoveInterval( new Interval<int>( index, index ) );

			// Move all operations to the right of the removed index, one to the left.
			int last = _list.Count - 1;
			_pendingOperations.MoveInterval( new Interval<int>( index + 1, last ), -1 );

			// Actual remove.
			_list.RemoveAt( index );
		}

		/// <summary>
		///   Execute all pending operations.
		/// </summary>
		public void FlushPendingOperations()
		{
			foreach ( var pending in _pendingOperations )
			{
				// Execute operations on entire range.
				IList<Func<TObject, TObject>> operations = pending.Values;
				pending.Interval.EveryStepOf( 1, i => ExecuteOperations( operations, i ) );
			}

			_pendingOperations.Clear();
		}

		/// <summary>
		///   Execute all pending operations for the given index.
		/// </summary>
		/// <param name = "index">The index for which to execute the pending operations.</param>
		void DoPendingOperations( int index )
		{
			Contract.Requires( index >= 0 && index <= Count );

			List<Func<TObject, TObject>> operations = _pendingOperations.ElementsAt( index );
			ExecuteOperations( operations, index );

			// Remove operations as they are no longer pending.
			_pendingOperations.RemoveInterval( new Interval<int>( index, index ) );
		}

		/// <summary>
		///   Execute given operations on a specified index.
		/// </summary>
		/// <param name = "operations">The operations to execute.</param>
		/// <param name = "i">The index of the list on which to execute the operations.</param>
		/// <returns></returns>
		void ExecuteOperations( IEnumerable<Func<TObject, TObject>> operations, int i )
		{
			foreach ( var action in operations )
			{
				_list[ i ] = action( _list[ i ] );
			}
		}


		#region IList<TObject> Members

		public int IndexOf( TObject item )
		{
			return _list.IndexOf( item );
		}

		public void Insert( int index, TObject item )
		{
			throw new NotImplementedException();
		}

		public TObject this[ int index ]
		{
			get
			{
				DoPendingOperations( index );

				return _list[ index ];
			}
			set { throw new NotImplementedException(); }
		}

		#endregion


		#region ICollection<TObject> Members

		public void Add( TObject item )
		{
			// Just add. Since it's added at the back, no changes have to be made to the pending operations.
			_list.Add( item );
		}

		/// <summary>
		///   Clears the entire list, including the pending operations on its objects.
		/// </summary>
		public void Clear()
		{
			_list.Clear();
			_pendingOperations.Clear();
		}

		public bool Contains( TObject item )
		{
			return _list.Contains( item );
		}

		public void CopyTo( TObject[] array, int arrayIndex )
		{
			// Make sure pending operations on the items which are copied are done first.
			FlushPendingOperations();

			_list.CopyTo( array, arrayIndex );
		}

		/// <summary>
		///   The amount of objects in the list.
		/// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove( TObject item )
		{
			throw new NotImplementedException();
		}

		#endregion


		#region IEnumerable<TObject> Members

		public IEnumerator<TObject> GetEnumerator()
		{
			return new Enumerator( _list );
		}

		#endregion


		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}