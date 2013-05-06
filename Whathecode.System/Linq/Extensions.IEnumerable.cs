using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Whathecode.System.Extensions;


namespace Whathecode.System.Linq
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns distinct elements based on a selected value from a sequence by using the default equality comparer to compare values.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <typeparam name = "TSelect">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "selector">A transform function to apply to each element which selects the elements to be compared.</param>
		/// <returns>An <see cref="IEnumerable{T}" /> that contains distinct elements from the source sequence.</returns>
		public static IEnumerable<T> Distinct<T, TSelect>( this IEnumerable<T> source, Func<T, TSelect> selector )
		{
			var unique = new HashSet<TSelect>();

			using ( IEnumerator<T> iterator = source.GetEnumerator() )
			{
				while ( iterator.MoveNext() )
				{
					TSelect selected = selector( iterator.Current );
					if ( !unique.Contains( selected ) )
					{
						unique.Add( selected );
						yield return iterator.Current;
					}
				}
			}
		}

		/// <summary>
		///   Merges three sequences by using the specified predicate function.
		/// </summary>
		/// <typeparam name = "TFirst">The type of the elements of the first input sequence.</typeparam>
		/// <typeparam name = "TSecond">The type of the elements of the second input sequence.</typeparam>
		/// <typeparam name = "TThird">The type of the elements of the third input sequence.</typeparam>
		/// <typeparam name = "TResult">The type of the elements of the result sequence.</typeparam>
		/// <param name = "first">The first sequence to merge.</param>
		/// <param name = "second">The second sequence to merge.</param>
		/// <param name = "third">The third sequence to merge.</param>
		/// <param name = "resultSelector">A function that specifies how to merge the elements from the three sequences.</param>
		/// <returns>An <see cref = "IEnumerable{T}" /> that contains merged elements of three input sequences.</returns>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(
			this IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			IEnumerable<TThird> third,
			Func<TFirst, TSecond, TThird, TResult> resultSelector )
		{
			Contract.Requires( first != null && second != null && third != null && resultSelector != null );

			using ( IEnumerator<TFirst> iterator1 = first.GetEnumerator() )
			using ( IEnumerator<TSecond> iterator2 = second.GetEnumerator() )
			using ( IEnumerator<TThird> iterator3 = third.GetEnumerator() )
			{
				while ( iterator1.MoveNext() && iterator2.MoveNext() && iterator3.MoveNext() )
				{
					yield return resultSelector( iterator1.Current, iterator2.Current, iterator3.Current );
				}
			}
		}

		/// <summary>
		///   Returns a specified number of random elements without returning a same element twice.
		/// </summary>
		/// <remarks>
		///   This method initializes a random number generator.
		///   In case you call this method often within small timespans, use the overload which allows you to pass a random number generator yourself.
		/// </remarks>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "count">The number of elements to return.</param>
		/// <returns>A sequence of random elements from the given sequence.</returns>
		public static IEnumerable<T> TakeRandom<T>( this IEnumerable<T> source, int count )
		{
			return TakeRandom( source, count, new Random() );
		}

		/// <summary>
		///   Returns a specified number of random elements without returning a same element twice.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "count">The number of elements to return.</param>
		/// <param name = "random">The random number generator used to take random elements.</param>
		/// <returns>A sequence of random elements from the given sequence.</returns>
		public static IEnumerable<T> TakeRandom<T>( this IEnumerable<T> source, int count, Random random )
		{
			Contract.Requires( source != null );
			Contract.Requires( count <= source.Count() );

			List<T> remainingElements = source.ToList();

			for ( int taken = 0; taken < count; ++taken )
			{
				int randomIndex = remainingElements.GetIndexInterval().GetValueAt( random.NextDouble() );
				yield return remainingElements[ randomIndex ];

				remainingElements.RemoveAt( randomIndex );
			}
		}

		/// <summary>
		///   Returns all combinations of a chosen amount of selected elements in the sequence.
		///   TODO: Since the enumerable always needs to be enumerated to an array,
		///         would it make sense only to allow this extension method for lists/arrays?
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "select">The amount of elements to select for every combination.</param>
		/// <param name = "repetition">True when repetition of elements is allowed.</param>
		/// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
		public static IEnumerable<IEnumerable<T>> Combinations<T>( this IEnumerable<T> source, int select, bool repetition = false )
		{
			Contract.Requires( source != null );
			Contract.Requires( @select >= 0 );

			var list = source as T[] ?? source.ToArray();

			return @select == 0
				? new[] { new T[0] }
				: list.SelectMany( ( element, index ) =>
					list
						.Skip( repetition ? index : index + 1 )
						.Combinations( @select - 1, repetition )
						.Select( c => new[] { element }.Concat( c ) ) );
		}

		/// <summary>
		///   Returns whether the sequence contains exactly a certain amount of elements.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "count">The exact amount of elements the sequence should contain.</param>
		/// <returns>True when the sequence contains exactly the specified amount of elements, false otherwise.</returns>
		public static bool CountOf<T>( this IEnumerable<T> source, int count )
		{
			Contract.Requires( source != null );
			Contract.Requires( count >= 0 );

			return source.Take( count + 1 ).Count() == count;
		}

		/// <summary>
		///   Returns whether the sequence contains at least a certain amount of elements.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "count">The amount of elements the sequence should at leastcontain.</param>
		/// <returns>True when the sequence contains at least the specified amount of elements, false otherwise.</returns>
		public static bool CountOfAtLeast<T>( this IEnumerable<T> source, int count )
		{
			Contract.Requires( source != null );
			Contract.Requires( count >= 0 );

			return source.Take( count ).Count() == count;
		}

		/// <summary>
		///   Determines whether a sequence contains a set of specified elements by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "elements">The elements to locate in the sequence.</param>
		/// <returns>true if the source sequence contains all elements; otherwise, false.</returns>
		public static bool ContainsAll<T>( this IEnumerable<T> source, params T[] elements )
		{
			return source.ContainsAll( (IEnumerable<T>)elements );
		}

		/// <summary>
		///   Determines whether a sequence contains a set of specified elements by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "elements">The elements to locate in the sequence.</param>
		/// <returns>true if the source sequence contains all elements; otherwise, false.</returns>
		public static bool ContainsAll<T>( this IEnumerable<T> source, IEnumerable<T> elements )
		{
			return elements.All( source.Contains );
		}

		/// <summary>
		///   Determines whether a sequence contains any element from a specified sequence of elements by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "elements">The elements to locate in the sequence.</param>
		/// <returns>true if the source sequence contains any of the elements in the sequence; otherwise, false</returns>
		public static bool ContainsAny<T>( this IEnumerable<T> source, params T[] elements )
		{
			return source.ContainsAny( (IEnumerable<T>)elements );
		}

		/// <summary>
		///   Determines whether a sequence contains any element from a specified sequence of elements by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "elements">The elements to locate in the sequence.</param>
		/// <returns>true if the source sequence contains any of the elements in the sequence; otherwise, false</returns>
		public static bool ContainsAny<T>( this IEnumerable<T> source, IEnumerable<T> elements )
		{
			return source.Where( elements.Contains ).Any();
		}

		/// <summary>
		///   Determines whether a sequence contains the same set of elements than another sequence by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "search">The elements to compare with.</param>
		/// <returns>true if both sequences contain the same elements; otherwise, false.</returns>
		public static bool ContainsOnly<T>( this IEnumerable<T> source, params T[] search )
		{
			return source.ContainsOnly( (IEnumerable<T>)search );
		}

		/// <summary>
		///   Determines whether a sequence contains the same set of elements than another sequence by using the default equality comparer.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "search">The elements to compare with.</param>
		/// <returns>true if both sequences contain the same elements; otherwise, false.</returns>
		public static bool ContainsOnly<T>( this IEnumerable<T> source, IEnumerable<T> search )
		{
			var elements = source.ToList();
			return elements.Intersect( search ).Count() == elements.Count;
		}

		/// <summary>
		///   Determines whether all values in a sequence are equal.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <returns>True when all values in the sequence or equal, false otherwise.</returns>
		public static bool AllEqual<T>( this IEnumerable<T> source )
		{
			Contract.Requires( source != null );

			// ReSharper disable PossibleMultipleEnumeration
			if ( !source.Any() )	// This only attempts to retrieve the first item, not an expensive operation.
			{
				return true;
			}

			T first = source.First();
			return source.Skip( 1 ).All( o => o.Equals( first ) );
			// ReSharper restore PossibleMultipleEnumeration
		}

		/// <summary>
		///   Returns the minimum selected value from a sequence of values using the default comparer.
		///   This will throw an exception if the sequence is empty.
		/// </summary>
		/// <typeparam name = "TSource">The type of the elements of source.</typeparam>
		/// <typeparam name = "TKey">The type of the selected value within the elements.</typeparam>
		/// <param name = "source">A sequence of values to determine the minimum selected value of.</param>
		/// <param name = "selector">A function which returns the value of which the minimum needs to be determined.</param>
		/// <returns>The element with the minimal selected value. The first element with the minimal selected value when there is more than one.</returns>
		public static TSource MinBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> selector )
		{
			return source.MinBy( selector, Comparer<TKey>.Default );
		}

		/// <summary>
		///   Returns the minimum selected value from a sequence of values using the passed comparer.
		///   This will throw an exception if the sequence is empty.
		/// </summary>
		/// <typeparam name = "TSource">The type of the elements of source.</typeparam>
		/// <typeparam name = "TKey">The type of the selected value within the elements.</typeparam>
		/// <param name = "source">A sequence of values to determine the minimum selected value of.</param>
		/// <param name = "selector">A function which returns the value of which the minimum needs to be determined.</param>
		/// <param name = "comparer">The comparer used to determine which value is smaller.</param>
		/// <returns>The element with the minimal selected value. The first element with the minimal selected value when there is more than one.</returns>
		public static TSource MinBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer )
		{
			Contract.Requires( source != null && selector != null && comparer != null );

			using ( IEnumerator<TSource> sourceIterator = source.GetEnumerator() )
			{
				if ( !sourceIterator.MoveNext() )
				{
					throw new InvalidOperationException( "Sequence was empty." );
				}

				TSource min = sourceIterator.Current;
				TKey minKey = selector( min );
				while ( sourceIterator.MoveNext() )
				{
					TSource candidate = sourceIterator.Current;
					TKey candidateProjected = selector( candidate );
					if ( comparer.Compare( candidateProjected, minKey ) < 0 )
					{
						min = candidate;
						minKey = candidateProjected;
					}
				}

				return min;
			}
		}
	}
}