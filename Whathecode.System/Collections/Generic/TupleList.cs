using System;
using System.Collections.Generic;


namespace Whathecode.System.Collections.Generic
{
	/// <summary>
	///   A list of 2-tuples, or pairs.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	public class TupleList<T1, T2> : List<Tuple<T1, T2>>
	{
		public void Add( T1 item, T2 item2 )
		{
			Add( new Tuple<T1, T2>( item, item2 ) );
		}
	}

	/// <summary>
	///   A list of 3-tuples, or triples.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
	{
		public void Add( T1 item, T2 item2, T3 item3 )
		{
			Add( new Tuple<T1, T2, T3>( item, item2, item3 ) );
		}
	}

	/// <summary>
	///   A list of 4-tuples, or quadruples.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	public class TupleList<T1, T2, T3, T4> : List<Tuple<T1, T2, T3, T4>>
	{
		public void Add( T1 item, T2 item2, T3 item3, T4 item4 )
		{
			Add( new Tuple<T1, T2, T3, T4>( item, item2, item3, item4 ) );
		}
	}

	/// <summary>
	///   A list of 5-tuples, or quintuples.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	public class TupleList<T1, T2, T3, T4, T5> : List<Tuple<T1, T2, T3, T4, T5>>
	{
		public void Add( T1 item, T2 item2, T3 item3, T4 item4, T5 item5 )
		{
			Add( new Tuple<T1, T2, T3, T4, T5>( item, item2, item3, item4, item5 ) );
		}
	}
}
