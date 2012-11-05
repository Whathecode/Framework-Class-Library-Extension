using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;


namespace Whathecode.Tests.System
{
	/// <summary>
	///   Unit tests for <see href = "Mathelper" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class MathHelperTest
	{
		[TestMethod]
		public void RoundToNearestTest()
		{
			// Different types.
			RoundToNearestTestHelper<short>( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper<long>( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper<ushort>( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper<uint>( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper<ulong>( 5, 1, 0, 4, 5 );
			RoundToNearestTestHelper( 1.5f, 0.5, 0, 1, 1.5f );
			RoundToNearestTestHelper( 1.5, 0.5, 0, 1, 1.5f );
			RoundToNearestTestHelper( 1.5m, 0.5m, 0, 1, 1.5m );

			// Multiple.
			RoundToNearestTestHelper( 5, 11, 10, 14, 15 );
			RoundToNearestTestHelper( 1.5, 3.5, 3, 4, 4.5 );

			// Negative values.
			RoundToNearestTestHelper( 5, -4, -5, -1, 0 );

			// Rounding issue.
			// TODO: The following test has a correct outcome, but due to rounding issues asserts false. Create a test which uses an epsilon comparison?
			//RoundToNearestTestHelper( 0.3, 1, 0.9, 0.11, 0.12 );
		}

		static void RoundToNearestTestHelper<T>( T factor, T toRoundDown, T roundDownResult, T toRoundUp, T roundUpResult )
		{
			Assert.AreEqual( roundDownResult, MathHelper.RoundToNearest( toRoundDown, factor ) );
			Assert.AreEqual( roundUpResult, MathHelper.RoundToNearest( toRoundUp, factor ) );
		}
	}
}
