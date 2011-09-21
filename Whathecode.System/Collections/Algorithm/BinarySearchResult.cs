namespace Whathecode.System.Collections.Algorithm
{
	/// <summary>
	///   Result of a binary search.
	/// </summary>
	/// <typeparam name = "TObject">The result type of the search.</typeparam>
	/// <author>Steven Jeuris</author>
	public struct BinarySearchResult<TObject>
	{
		/// <summary>
		///   True when the object lies inside the range of values.
		/// </summary>
		public bool IsObjectInRange;

		/// <summary>
		///   True when the object was found, false otherwise.
		/// </summary>
		public bool IsObjectFound;

		/// <summary>
		///   The object, when found. Default value for TObject type otherwise.
		/// </summary>
		public TObject Object;

		/// <summary>
		///   The object smaller or equal than the object searched for.
		/// </summary>
		public TObject Smaller;

		/// <summary>
		///   The object bigger or equal than the object searched for.
		/// </summary>
		public TObject Bigger;
	}
}