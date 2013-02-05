using System;
using System.Linq.Expressions;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Experimental;


namespace Whathecode.Tests.System.Experimental
{
	[TestClass]
	public class SwizzleTest
	{
		#region Common Test Members

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

		Vector _v1, _v2;
		Class _c1, _c2;

		[TestInitialize]
		public void InitializeTest()
		{
			_v1 = new Vector( 1, 2 );
			_v2 = new Vector();
			_c1 = new Class( 1, 2 );
			_c2 = new Class( 0, 0 );
		}

		public void AssertHelper<TProperty>( TProperty x1, TProperty y1, TProperty x2, TProperty y2, TProperty x, TProperty y )
		{
			Assert.AreEqual( x1, y2 );
			Assert.AreEqual( y1, x2 );
			Assert.AreEqual( y, x2 );
			Assert.AreEqual( x, y2 );
		}

		#endregion  // Common Test Members


		[TestMethod]
		public void InPlaceConstructionTest()
		{
			// Test structs as well as reference types.
			InPlaceConstruction<Vector, double>( _v1, _v2, v => v.X, v => v.Y, 1, 2 );
			InPlaceConstruction<Class, int>( _c1, _c2, c => c.X, c => c.Y, 1, 2 );
		}
		
		public void InPlaceConstruction<T, TProperty>( T t1, T t2, Expression<Func<T, TProperty>> getXExpr, Expression<Func<T, TProperty>> getYExpr, TProperty x, TProperty y )
		{
			var t2_xy = new Swizzle<T, TProperty>( t2, getXExpr, getYExpr );
			var t1_yx = new Swizzle<T, TProperty>( t1, getYExpr, getXExpr );
			t2 = t2_xy.Set( t1_yx );

			Func<T, TProperty> getX = getXExpr.Compile();
			Func<T, TProperty> getY = getYExpr.Compile();
			AssertHelper( getX( t1 ), getY( t1 ), getX( t2 ), getY( t2 ), x, y );
		}

		[TestMethod]
		public void UsingDelegatesTest()
		{
			GetSwizzle<Vector, double> XY = i => new Swizzle<Vector, double>( i, v => v.X, v => v.Y );
			GetSwizzle<Vector, double> YX = i => new Swizzle<Vector, double>( i, v => v.Y, v => v.X );

			_v2 = XY( _v2 ).Set( YX( _v1 ) );
			AssertHelper( _v1.X, _v1.Y, _v2.X, _v2.Y, 1, 2 );
		}

		[TestMethod]
		public void UsingExtensionMethodsTest()
		{
			_v2 = _v2.XY().Set( _v1.YX() );
			AssertHelper( _v1.X, _v1.Y, _v2.X, _v2.Y, 1, 2 );
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
	}
}
