using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Linq;


namespace Whathecode.Tests.System.Linq
{
	// ReSharper disable InconsistentNaming    
	[TestClass]
	public class IEnumerableExtensionsTest
		// ReSharper restore InconsistentNaming
	{
		#region Common Test Members

		List<int> _shortList;
		List<int> _list;

		[TestInitialize]
		public void Initialize()
		{
			_shortList = new List<int> { 0, 1, 2 };
			_list = new List<int> { 0, 1, 2, 3, 4 };
		}

		#endregion // Common Test Members


		[TestMethod]
		public void CombinationsTest()
		{
			Func<IEnumerable<IEnumerable<int>>, IEnumerable<string>> toStrings = 
				input => input.Select( c => c.OrderBy( k => k ).Aggregate( "", (s, n) => s + n ) ).ToList();

			// No repetition allowed.
			var combinations = toStrings( _shortList.Combinations( 2 ) );
			Assert.IsTrue( combinations.ContainsOnly( "01", "02", "12" ) );

			// Repetition allowed.
			var repeatedCombinations = toStrings( _shortList.Combinations( 2, true ) );
			Assert.IsTrue( repeatedCombinations.ContainsOnly( "00", "11", "22", "01", "02", "12" ) );
		}

		[TestMethod]
		public void CountOfTest()
		{
			Assert.IsFalse( _list.CountOf( 0 ) );
			Assert.IsFalse( _list.CountOf( 2 ) );
			Assert.IsTrue( _list.CountOf( 5 ) );
			Assert.IsFalse( _list.CountOf( 10 ) );
		}

		[TestMethod]
		public void ContainsAllTest()
		{
			Assert.IsTrue( _list.ContainsAll( new[] { 0, 2, 4 } ) );
			Assert.IsTrue( _list.ContainsAll( 0, 2, 4 ) );
			Assert.IsFalse( _list.ContainsAll( new[] { 0, 1, 2, 5 } ) );
		}

		[TestMethod]
		public void ContainsOnlyTest()
		{			
			Assert.IsTrue( _list.ContainsOnly( new[] { 0, 1, 2, 3, 4 } ) );
			Assert.IsTrue( _list.ContainsOnly( 0, 1, 2, 3, 4 ) );
			Assert.IsFalse( _list.ContainsOnly( new[] { 0, 1, 2 } ) );
			Assert.IsFalse( _list.ContainsOnly( new[] { 5, 6, 7 } ) );			
		}
	}
}