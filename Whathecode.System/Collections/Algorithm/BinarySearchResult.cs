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
		///   Stores the retrieved object if found.
		/// </summary>
		public class FoundResult
		{
			/// <summary>
			///   The object which was found.
			/// </summary>
			public readonly TObject Object;


			public FoundResult( TObject foundObject )
			{
				Object = foundObject;
			}
		}

		/// <summary>
		///   Stores the nearest objects when object is not found but within range.
		/// </summary>
		public class NotFoundResult
		{
			/// <summary>
			///   The object smaller or equal than the object searched for.
			/// </summary>
			public readonly TObject Smaller;

			/// <summary>
			///   The object bigger or equal than the object searched for.
			/// </summary>
			public readonly TObject Bigger;


			public NotFoundResult( TObject smaller, TObject bigger )
			{
				Smaller = smaller;
				Bigger = bigger;
			}
		}


		/// <summary>
		///   True when the object lies inside the range of values.
		/// </summary>
		public bool IsObjectInRange;

		/// <summary>
		///   True when the object was found, false otherwise.
		/// </summary>
		public bool IsObjectFound;

		/// <summary>
		///   Contains the retrieved object if found, null otherwise.
		/// </summary>
		public FoundResult Found;

		/// <summary>
		///   Contains the nearest objects when object is not found but within range, null otherwise.
		/// </summary>
		public NotFoundResult NotFound;
	}
}