using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Experimental;


namespace Whathecode.Tests.System.Experimental
{
	[TestClass]
	public class SwizzleTest
	{
		[TestMethod]
		public void SwizzleStructTest()
		{
			var v1 = new Vector( 1, 2 );
			var v2 = new Vector( 0, 0 );

			v2 = v2.XY().Set( v1.YX() );

			Assert.AreEqual( v1.Y, v2.X );
			Assert.AreEqual( v1.X, v2.Y );
			Assert.AreEqual( 2, v2.X );
			Assert.AreEqual( 1, v2.Y );
		}

		public class Class
		{
			public int X;
			public int Y;

			public Class( int x, int y )
			{
				X = x;
				Y = y;
			}
		}

		[TestMethod]
		public void SwizzleClassTest()
		{
			var c1 = new Class( 1, 2 );
			var c2 = new Class( 0, 0 );

			c2.XY().Set( c1.YX() );

			Assert.AreEqual( c1.Y, c2.X );
			Assert.AreEqual( c1.X, c2.Y );
			Assert.AreEqual( 2, c2.X );
			Assert.AreEqual( 1, c2.Y );
		}
	}

	static class VectorExtensions
	{
		static PropertyList<Vector, double> _xy = PropertyListFactory<Vector>.Create( v => v.X, v => v.Y );
		public static Swizzle<Vector, double> XY( this Vector source )
		{
			return new Swizzle<Vector, double>( source, _xy );
		}

		static PropertyList<Vector, double> _yx = PropertyListFactory<Vector>.Create( v => v.Y, v => v.X );
		public static Swizzle<Vector, double> YX( this Vector source )
		{
			return new Swizzle<Vector, double>( source, _yx );
		}

		static PropertyList<SwizzleTest.Class, int> _cxy = PropertyListFactory<SwizzleTest.Class>.Create( c => c.X, c => c.Y );
		public static Swizzle<SwizzleTest.Class, int> XY( this SwizzleTest.Class source )
		{
			return new Swizzle<SwizzleTest.Class, int>( source, _cxy );
		}

		static PropertyList<SwizzleTest.Class, int> _cyx = PropertyListFactory<SwizzleTest.Class>.Create( c => c.Y, c => c.X );
		public static Swizzle<SwizzleTest.Class, int> YX( this SwizzleTest.Class source )
		{
			return new Swizzle<SwizzleTest.Class, int>( source, _cyx );
		}
	}
}
