namespace Whathecode.System.Collections.Delegates
{
	/// <summary>
	///   Operations that can be done on an indexer.
	/// </summary>
	/// <typeparam name = "TObject">The type of the objects which are retrieved by the index.</typeparam>
	/// <typeparam name = "TIndex">The type for the index.</typeparam>
	/// <author>Steven Jeuris</author>
	public class IndexerDelegates<TObject, TIndex>
	{
		/// <summary>
		///   Delegate which can get an object based on a index.
		/// </summary>
		/// <param name = "index">The index to get the object for.</param>
		/// <returns>The object for the given index.</returns>
		public delegate TObject GetByIndexDelegate( TIndex index );


		/// <summary>
		///   Delegate which can get the nearest index for a given possible invalid index.
		/// </summary>
		/// <param name = "index">The value to get the nearest correct index of.</param>
		/// <returns>The nearest correct index.</returns>
		public delegate TIndex GetNearestIndexDelegate( TIndex index );


		/// <summary>
		///   Get the object for a given index.
		/// </summary>
		public GetByIndexDelegate GetByIndex { get; private set; }

		/// <summary>
		///   Get the nearest index for a given possible invalid index.
		/// </summary>
		public GetNearestIndexDelegate GetNearestIndex { get; private set; }


		/// <summary>
		///   Initialize a new set of operations which define access to a enumerable indexer.
		/// </summary>
		/// <param name = "getByIndex">Delegate which can get an object based on a index.</param>
		/// <param name = "getNearestIndex">Delegate which can get the nearest index for a given possible invalid index.</param>
		public IndexerDelegates( GetByIndexDelegate getByIndex, GetNearestIndexDelegate getNearestIndex )
		{
			GetByIndex = getByIndex;
			GetNearestIndex = getNearestIndex;
		}
	}
}