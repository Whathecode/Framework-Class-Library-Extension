using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Arithmetic;
using Whathecode.System.Extensions;


namespace Whathecode.Tests.System.Extensions
{
	[TestClass]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void SplitAtTest()
		{
			// Exclude split.
			const string toSplit = "Split.Here";
			int splitPosition = toSplit.IndexOf( '.' );
			string[] split = toSplit.SplitAt( splitPosition );
			Assert.AreEqual( "Split", split[ 0 ] );
			Assert.AreEqual( "Here", split[ 1 ] );

			// Include both.
			split = toSplit.SplitAt( splitPosition, SplitOption.Both );
			Assert.AreEqual( "Split.", split[ 0 ] );
			Assert.AreEqual( ".Here", split[ 1 ] );

			// Include left.
			split = toSplit.SplitAt( splitPosition, SplitOption.Left );
			Assert.AreEqual( "Split.", split[ 0 ] );
			Assert.AreEqual( "Here", split[ 1 ] );

			// Include right.
			split = toSplit.SplitAt( splitPosition, SplitOption.Right );
			Assert.AreEqual( "Split", split[ 0 ] );
			Assert.AreEqual( ".Here", split[ 1 ] );
		}
	}
}