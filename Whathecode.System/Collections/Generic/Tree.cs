using System;
using System.Collections.Generic;
using System.Linq;


namespace Whathecode.System.Collections.Generic
{
	/// <summary>
	///   A collection of elements organized within a hierarchy.
	/// </summary>
	/// <typeparam name="T">The type of the values in the dictionary.</typeparam>
	public class Tree<T>
	{
		public T Value;

		/// <summary>
		///   The parent to which this tree is connected. Null when this is a root.
		/// </summary>
		public Tree<T> Parent { get; private set; } 

		readonly List<Tree<T>> _children = new List<Tree<T>>();
		public IEnumerable<Tree<T>> Children
		{
			get { return _children; }
		}


		public Tree( T value )
		{
			Value = value;
		}


		/// <summary>
		///   Adds a full branch to this tree, as specified by an ordered list of values.
		///   Existing branches (using the default Equals comparator) remain unchanged.
		/// </summary>
		/// <param name="branch">The branch to add, ordered from the first branch after the root of this tree, to the final branch.</param>
		/// <returns>The first non-existing node of the branch which got added, or null when node already existed.</returns>
		public Tree<T> AddBranch( IEnumerable<T> branch )
		{
			// Make sure the branch isn't empty.
			List<T> fullBranch = branch.ToList();
			if ( fullBranch.Count == 0 )
			{
				return null;
			}

			// Find matching leaf.
			T first = fullBranch.First();
			Tree<T> matchingLeaf = _children.FirstOrDefault( c => c.Value.Equals( first ) );
			if ( matchingLeaf == null )
			{
				// Leaf doens't exist yet, create full remaining branch.
				Tree<T> firstNew = null;
				Tree<T> current = this;
				foreach ( var leaf in fullBranch )
				{
					current = current.AddLeaf( leaf );
					if ( firstNew == null )
					{
						firstNew = current;
					}
				}
				return firstNew;
			}

			// Leafs match, go down the branch.
			return matchingLeaf.AddBranch( fullBranch.Skip( 1 ) );
		}

		/// <summary>
		///   Adds a new leaf to this tree.
		/// </summary>
		/// <param name="leaf">The value to add as a leaf to this tree.</param>
		/// <returns>The newly added leaf.</returns>
		public Tree<T> AddLeaf( T leaf )
		{
			Tree<T> newLeaf = new Tree<T>( leaf ) { Parent = this };
			_children.Add( newLeaf );
			return newLeaf;
		}

		/// <summary>
		///   Traverses nodes up until the tree until a passed evaluation returns true. Null is returned when reaching the top of the tree.
		/// </summary>
		/// <param name="stopTraversal">Function which evaluates whether to stop at a given node or not.</param>
		/// <returns>The first node encountered where <see cref="stopTraversal" /> returns true. Null in case the top of the tree is reached.</returns>
		public Tree<T> TraverseUpUntil( Func<Tree<T>, bool> stopTraversal )
		{
			Tree<T> higherPeer = Parent;
			while ( higherPeer != null )
			{
				if ( stopTraversal( higherPeer ) )
				{
					break;
				}
				higherPeer = higherPeer.Parent;
			}

			return higherPeer;
		}

		/// <summary>
		///   Removes the current node from the parent tree.
		/// </summary>
		public void Remove()
		{
			Parent._children.Remove( this );
		}
	}
}
