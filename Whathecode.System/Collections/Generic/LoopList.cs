using System.Collections.Generic;


namespace Whathecode.System.Collections.Generic
{
	/// <summary>
	///   A list which loops over a list of objects indefinetely when retrieving objects from it.
	/// </summary>
	/// <typeparam name="T">The type of objects in the list.</typeparam>
	/// <author>Steven Jeuris</author>
	public class LoopList<T>
	{
		readonly Queue<T> _objects;


		/// <summary>
		///   Create a new loop list, initialized with a range of objects.
		/// </summary>
		/// <param name="range">The range of objects to initialize the list with.</param>
		public LoopList( IEnumerable<T> range )
		{
			_objects = new Queue<T>( range );
		}


		/// <summary>
		///   Get the next item from the list.
		/// </summary>
		public T Next()
		{
			T next = _objects.Dequeue();
			_objects.Enqueue( next );
			return next;
		}

		/// <summary>
		///   Returns an enumerator which only returns all objects once. 
		/// </summary>
		public IEnumerator<T> GetNonLoopedEnumerator()
		{
			return _objects.GetEnumerator();
		}
	}
}
