using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.Tests.System.Arithmetic.Range
{
	/// <summary>
	///   Unit tests for <see cref="Interval{TMath}" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class IntervalTest
	{
		#region LiesInInterval Tests

		[TestMethod]
		public void LiesInIntervalTest()
		{
			var single = new Interval<int>( 0, 0 );
			LiesInInterval( single, 0, true );
			LiesInInterval( single, 1, false );

			LiesInIntervalTestHelper( 0, 10, 5, 11 );
			LiesInIntervalTestHelper( 0.5, 10.5, 5, 12 );
		}

		static void LiesInIntervalTestHelper<TMath>( TMath start, TMath end, TMath inside, TMath outside )
			where TMath : IComparable<TMath>
		{
			var included = new Interval<TMath>( start, end );
			LiesInInterval( included, inside, true );
			LiesInInterval( included, start, true ); // On edge.
			LiesInInterval( included, outside, false ); // Outside.

			var excluded = new Interval<TMath>( start, false, end, false );
			LiesInInterval( excluded, start, false ); // Left edge.
			LiesInInterval( excluded, end, false ); // Right edge.
		}

		static void LiesInInterval<TMath>( Interval<TMath> interval, TMath value, bool expected )
			where TMath : IComparable<TMath>
		{
			bool insideInterval = interval.LiesInInterval( value );

			Assert.AreEqual( expected, insideInterval );
		}

		#endregion  // LiesInInterval Tests


		#region Intersects Tests

		[TestMethod]
		public void IntersectsTest()
		{
			IntersectsTestHelper( 0, 5, 10, 20, 100 );
			IntersectsTestHelper( 0.5, 10.0, 20.5, 50.6, 100.9 );
		}

		/// <summary>
		///   Test intersecting intervals with given parameters from small to bigger.
		/// </summary>
		static void IntersectsTestHelper<TMath>( TMath a, TMath b, TMath c, TMath d, TMath e )
			where TMath : IComparable<TMath>
		{
			// Create required intervals.
			var ab = new Interval<TMath>( a, b );
			var bc = new Interval<TMath>( b, c );
			var cd = new Interval<TMath>( c, d );
			var de = new Interval<TMath>( d, e );
			var ad = new Interval<TMath>( a, d );
			var ac = new Interval<TMath>( a, c );
			var bd = new Interval<TMath>( b, d );
			var bcExcluded = new Interval<TMath>( b, false, c, false );

			// None overlapping intervals.
			Intersects( ab, de, false ); // Left.
			Intersects( de, ab, false ); // Right.

			// Neighboring intervals.
			Intersects( ab, bc, true ); // Left.
			Intersects( bc, ab, true ); // Right.
			Intersects( ab, bcExcluded, false ); // Left.
			Intersects( bcExcluded, cd, false ); // Right.

			// Entirely overlapping intervals.
			Intersects( ab, ab, true ); // Identical.
			Intersects( bc, ad, true ); // Inside.
			Intersects( ad, bc, true ); // Outside.
			Intersects( ab, ac, true ); // Left.
			Intersects( bc, ac, true ); // Right.

			// Partly overlapping intervals.
			Intersects( ac, bd, true ); // Right overlap.
			Intersects( bd, ac, true ); // Left overlap.
		}

		static void Intersects<TMath>( Interval<TMath> a, Interval<TMath> b, bool expected )
			where TMath : IComparable<TMath>
		{
			bool intersects = a.Intersects( b );

			Assert.AreEqual( expected, intersects );
		}

		#endregion  // Intersects Tests


		#region Subtract Tests

		[TestMethod]
		public void SubtractTest()
		{
			SubtractTestHelper( 0, 5, 10, 20, 100 );
			SubtractTestHelper( 0.5, 10.0, 20.5, 50.6, 100.9 );
		}

		/// <summary>
		///   Test intersecting intervals with given parameters from small to bigger.
		/// </summary>
		static void SubtractTestHelper<TMath>( TMath a, TMath b, TMath c, TMath d, TMath e )
			where TMath : IComparable<TMath>
		{
			// Create required intervals.
			var ab = new Interval<TMath>( a, b );
			var bc = new Interval<TMath>( b, c );
			var cd = new Interval<TMath>( c, d );
			var de = new Interval<TMath>( d, e );
			var ad = new Interval<TMath>( a, d );
			var ac = new Interval<TMath>( a, c );
			var bd = new Interval<TMath>( b, d );
			var bcExcluded = new Interval<TMath>( b, false, c, false );

			// None overlapping intervals.
			Subtract( ab, de, ab ); // Left.
			Subtract( de, ab, de ); // Right.

			// Neighboring intervals.
			Subtract( ab, bc, new Interval<TMath>( a, true, b, false ) ); // Left.
			Subtract( bc, ab, new Interval<TMath>( b, false, c, true ) ); // Right.
			Subtract( ab, bcExcluded, ab ); // Left.
			Subtract( bcExcluded, cd, bcExcluded ); // Right.

			// Entirely overlapping intervals.
			Subtract( ab, ab, new List<Interval<TMath>>() ); // Identical. 
			Subtract( bc, bcExcluded,
				new List<Interval<TMath>>
				{
					new Interval<TMath>( b, true, b, true ),
					new Interval<TMath>( c, true, c, true )
				}
				); // Identical except borders.           
			Subtract( bc, ad, new List<Interval<TMath>>() ); // Inside.
			Subtract( ad, bc,
				new List<Interval<TMath>>
				{
					new Interval<TMath>( a, true, b, false ),
					new Interval<TMath>( c, false, d, true )
				}
				); // Outside.
			Subtract( ab, ac, new List<Interval<TMath>>() ); // Left.
			Subtract( bc, ac, new List<Interval<TMath>>() ); // Right.            

			// Partly overlapping intervals.
			Subtract( ac, bd, new Interval<TMath>( a, true, b, false ) ); // Right overlap.
			Subtract( bd, ac, new Interval<TMath>( c, false, d, true ) ); // Left overlap.
		}

		// ReSharper disable UnusedParameter.Local
		static void Subtract<TMath>( Interval<TMath> from, Interval<TMath> subtract, Interval<TMath> result )		
			where TMath : IComparable<TMath>
		{
			List<Interval<TMath>> results = from.Subtract( subtract );

			Assert.IsTrue( results.Count == 1 );
			Assert.IsTrue( results[ 0 ].Equals( result ) );
		}
		// ReSharper restore UnusedParameter.Local

		static void Subtract<TMath>( Interval<TMath> from, Interval<TMath> subtract, List<Interval<TMath>> results )
			where TMath : IComparable<TMath>
		{
			List<Interval<TMath>> subtracted = from.Subtract( subtract );

			Assert.IsTrue( results.Count == subtracted.Count );
			for ( int i = 0; i < results.Count; ++i )
			{
				Interval<TMath> result = results[ i ];
				Interval<TMath> subtractResult = subtracted[ i ];
				Assert.IsTrue( result.Equals( subtractResult ) );
			}
		}

		#endregion  // Subtract Tests


		#region GetPercentageFor Tests

		[TestMethod]
		public void GetPercentageForTest()
		{
			GetPercentageForHelper( 0, 5, 10 );
			GetPercentageForHelper( 0.0, 5.0, 10.0 );
		}

		static void GetPercentageForHelper<TMath>( TMath aMinusA, TMath a, TMath aPlusA )
			where TMath : IComparable<TMath>
		{
			// Single range.
			var single = new Interval<TMath>( a, a );
			Assert.AreEqual( 1.0, single.GetPercentageFor( a ) );
			Assert.AreEqual( 0.0, single.GetPercentageFor( aMinusA ) );


			// Normal ranges. ( start < end )
			var right = new Interval<TMath>( a, aPlusA );
			var left = new Interval<TMath>( aMinusA, a );
			var big = new Interval<TMath>( aMinusA, aPlusA );

			// Reversed ranges. ( end < start )
			var reversedRight = new Interval<TMath>( aPlusA, a );
			var reversedLeft = new Interval<TMath>( a, aMinusA );
			var reversedBig = new Interval<TMath>( aPlusA, aMinusA );


			// Should lie at start.
			Assert.AreEqual( 0.0, right.GetPercentageFor( a ) );
			Assert.AreEqual( 0.0, reversedRight.GetPercentageFor( aPlusA ) );

			// Should lie in middle.
			Assert.AreEqual( 0.5, big.GetPercentageFor( a ) );
			Assert.AreEqual( 0.5, reversedBig.GetPercentageFor( a ) );

			// Should lie at end.
			Assert.AreEqual( 1.0, left.GetPercentageFor( a ) );
			Assert.AreEqual( 1.0, reversedLeft.GetPercentageFor( aMinusA ) );

			// Should lie before.
			Assert.AreEqual( -1.0, right.GetPercentageFor( aMinusA ) );
			Assert.AreEqual( -1.0, reversedLeft.GetPercentageFor( aPlusA ) );

			// Should lie after.
			Assert.AreEqual( 2.0, left.GetPercentageFor( aPlusA ) );
			Assert.AreEqual( 2.0, reversedRight.GetPercentageFor( aMinusA ) );
		}

		#endregion  // GetPercentageFor Tests


		#region Map Tests

		[TestMethod]
		public void MapTest()
		{
			// Same type ranges.
			MapHelper( 0, 10, 10, 20, 0, 10 ); // 0%
			MapHelper( 0, 30, 10, 40, 10, 20 ); // 33%
			MapHelper( 0, 10, 10, 20, 5, 15 ); // 50%
			MapHelper( 0, 10, 10, 20, 10, 20 ); // 100%
			MapHelper( 0, 4, 0, 100, 1, 25 ); // Different range sizes.
			MapHelper( -100, 100, 0, 100, 0, 50 ); // Negative to positive.
			MapHelper( 10, 20, -100, 50, 15, -25 ); // Positive to negative.
			MapHelper( 10, 0, 0, 10, 5, 5 ); // Reversed ranges.
			MapHelper( 0, 10, 100, 0, 5, 50 );
			MapHelper<double, double>( 0, 1, 0, 100, 0.5, 50 ); // Double math.

			// Different type ranges.
			MapHelper<int, double>( 0, 10, 10, 20, 0, 10 ); // 0%
			MapHelper<int, double>( 0, 30, 10, 40, 10, 20 ); // 33%
			MapHelper<int, double>( 0, 10, 10, 20, 5, 15 ); // 50%
			MapHelper<int, double>( 0, 10, 10, 20, 10, 20 ); // 100%
			MapHelper<int, double>( 0, 4, 0, 100, 1, 25 ); // Different range sizes.
			MapHelper<int, double>( -100, 100, 0, 100, 0, 50 ); // Negative to positive.
			MapHelper<int, double>( 10, 20, -100, 50, 15, -25 ); // Positive to negative.
			MapHelper<int, double>( 10, 0, 0, 10, 5, 5 ); // Reversed ranges.
			MapHelper<int, double>( 0, 10, 100, 0, 5, 50 );
		}

		// ReSharper disable UnusedParameter.Local
		static void MapHelper<TMath, TRange>( TMath from, TMath to, TRange mapFrom, TRange mapTo, TMath valueToMap, TRange expected )
			where TMath : IComparable<TMath>
			where TRange : IComparable<TRange>
		{
			var interval = new Interval<TMath>( from, to );
			TRange result = interval.Map( valueToMap, new Interval<TRange>( mapFrom, mapTo ) );

			Assert.IsTrue( result.Equals( expected ) );
		}
		// ReSharper restore UnusedParameter.Local

		#endregion  // Map Tests


		#region Clamp Tests

		[TestMethod]
		public void ClampTest()
		{
			// Normal intervals.
			ClampHelper( 0, 10, -100, 100, 5 );
			ClampHelper( 0.0, 10.0, -100.0, 100.0, 5.0 );

			// Reversed intervals.
			ClampHelper( 10, 0, 100, -100, 5 );
			ClampHelper( 10.0, 0.0, 100.0, -100.0, 5.0 );

			// Ranges.
			var interval = new Interval<int>( 0, 10 );
			var mid = new Interval<int>( 1, 9 );
			Assert.AreEqual( interval.Clamp( mid ), mid );
			var left = new Interval<int>( 0, 5 );			
			Assert.AreEqual( interval.Clamp( left ), left );
			var leftCrossover = new Interval<int>( -1, 5 );
			Assert.AreEqual( interval.Clamp( leftCrossover ), left );
			var right = new Interval<int>( 5, 10 );
			Assert.AreEqual( interval.Clamp( right ), right );
			var rightCrossover = new Interval<int>( 5, 11 );
			Assert.AreEqual( interval.Clamp( rightCrossover ), right );
			var outside = new Interval<int>( -1, 11 );
			Assert.AreEqual( interval.Clamp( outside ), interval );
		}

		static void ClampHelper<TMath>( TMath start, TMath end, TMath leftOfRange, TMath rightOfRange, TMath inRange )
			where TMath : IComparable<TMath>
		{
			var interval = new Interval<TMath>( start, end );

			// In range.
			Assert.AreEqual( interval.Clamp( inRange ), inRange );
			Assert.AreEqual( interval.Clamp( start ), start );
			Assert.AreEqual( interval.Clamp( end ), end );

			// Outside of range.
			Assert.AreEqual( interval.Clamp( leftOfRange ), start );
			Assert.AreEqual( interval.Clamp( rightOfRange ), end );
		}

		#endregion  // Clamp Tests
	}
}