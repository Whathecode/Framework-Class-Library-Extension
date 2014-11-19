using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Collections.Algorithm;
using Whathecode.System.Collections.Delegates;
using Whathecode.System.Extensions;


namespace Whathecode.Tests.System.Collections.Algorithm
{
	[TestClass]
	public class BinarySearchTest
	{
		[TestMethod]
		public void SearchTest()
		{
			var numbers = new [] { -10, -8, 0, 10, 500 };
			var indexerDelegates = new IndexerDelegates<int, int>( index => numbers[ index ], index => index );

			// Object found.
			BinarySearchResult<int> result = BinarySearch<int, int>.Search( 0, numbers.GetIndexInterval(), indexerDelegates );
			Assert.AreEqual( true, result.IsObjectFound );
			Assert.AreEqual( true, result.IsObjectInRange );
			Assert.AreEqual( 0, result.Found.Object );
			Assert.IsNull( result.NotFound );

			// Object found, border.
			result = BinarySearch<int, int>.Search( 500, numbers.GetIndexInterval(), indexerDelegates );
			Assert.AreEqual( true, result.IsObjectFound );
			Assert.AreEqual( true, result.IsObjectInRange );
			Assert.AreEqual( 500, result.Found.Object );
			Assert.IsNull( result.NotFound );

			// Object not found, but in range.
			result = BinarySearch<int, int>.Search( -9, numbers.GetIndexInterval(), indexerDelegates );
			Assert.AreEqual( false, result.IsObjectFound );
			Assert.AreEqual( true, result.IsObjectInRange );
			Assert.AreEqual( -10, result.NotFound.Smaller );
			Assert.AreEqual( -8, result.NotFound.Bigger );
			Assert.IsNull( result.Found );

			// Object not found, out of range, left.
			result = BinarySearch<int, int>.Search( -20, numbers.GetIndexInterval(), indexerDelegates );
			Assert.AreEqual( false, result.IsObjectFound );
			Assert.AreEqual( false, result.IsObjectInRange );
			Assert.IsNull( result.NotFound );
			Assert.IsNull( result.Found );

			// Object not found, out of range, right.
			result = BinarySearch<int, int>.Search( 600, numbers.GetIndexInterval(), indexerDelegates );
			Assert.AreEqual( false, result.IsObjectFound );
			Assert.AreEqual( false, result.IsObjectInRange );
			Assert.IsNull( result.NotFound );
			Assert.IsNull( result.Found );
		}
	}
}
