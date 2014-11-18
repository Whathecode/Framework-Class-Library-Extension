using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;
using Whathecode.System.Algorithm;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Operators;


namespace Whathecode.Tests.System.Arithmetic.Range
{
	/// <summary>
	///   Unit tests for <see cref="Interval{T, TSize}" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class IntervalTest
	{
		static readonly DateTime StartDate = new DateTime( 2014, 12, 1 );
		static readonly DateTime EndDate = new DateTime( 2014, 12, 2 );
		static readonly TimeSpan TimeDifference = TimeSpan.FromDays( 1 );


		[TestInitialize]
		public void Initialize()
		{
			Interval<DateTime, TimeSpan>.ConvertDoubleToSize = d => new TimeSpan( (long)Math.Round( d ) );
			Interval<DateTime, TimeSpan>.ConvertSizeToDouble = s => s.Ticks;
		}


		[TestMethod]
		public void IncorrectInitializationTest()
		{
			var incorrect = new Interval<TimeSpan, TimeSpan>( TimeSpan.Zero, TimeSpan.Zero );
			// ReSharper disable UnusedVariable
			AssertHelper.ThrowsException<InvalidImplementationException>( () => { var value = incorrect.GetValueAt( 0.5 ); } );
			AssertHelper.ThrowsException<InvalidImplementationException>( () => { var perc = incorrect.GetPercentageFor( TimeSpan.Zero ); } );
			AssertHelper.ThrowsException<InvalidImplementationException>( () => incorrect.Scale( 1.0 ) );
			AssertHelper.ThrowsException<InvalidImplementationException>( () => { var center = incorrect.Center; } );
			AssertHelper.ThrowsException<InvalidImplementationException>( 
				() => { var mapped = incorrect.Map( TimeSpan.Zero, new Interval<int>( 1, 1 ) ); } );
			// ReSharper restore UnusedVariable
		}


		#region Empty Tests

		[TestMethod]
		public void EmptyTest()
		{
			EmptyTestHelper( 0, 0 );
			EmptyTestHelper( 0.0, 0.0 );
			EmptyTestHelper( 0f, 0f );
			EmptyTestHelper( 0L, 0L );
			EmptyTestHelper( DateTime.MinValue, TimeSpan.Zero );
		}

		static void EmptyTestHelper<T, TSize>( T minValue, TSize size )
			where T : IComparable<T>
		{
			// Test for expected start, end and size.
			var empty = Interval<T, TSize>.Empty;
			Assert.AreEqual( minValue, empty.Start );
			Assert.AreEqual( minValue, empty.End );
			Assert.AreEqual( size, empty.Size );

			// Test that reference stays the same.
			var secondEmpty = Interval<T, TSize>.Empty;
			Assert.AreSame( empty, secondEmpty );
		}

		#endregion // Empty Tests


		#region IsReversed Test

		[TestMethod]
		public void IsReversedTest()
		{
			IsReversedTestHelper( 0, 10 );
			IsReversedTestHelper( DateTime.MinValue, TimeSpan.FromDays( 1 ) );
		}

		static void IsReversedTestHelper<T, TSize>( T value, TSize addition )
			where T : IComparable<T>
		{
			IInterval<T, TSize> normal = NewInterval( value, addition );
			Assert.IsFalse( normal.IsReversed );

			var reversed = new Interval<T, TSize>( normal.End, normal.Start );
			Assert.IsTrue( reversed.IsReversed );
		}

		#endregion // IsReversed Test


		#region Center Test

		[TestMethod]
		public void CenterTest()
		{
			CenterTestHelper( 0, 5 );
			CenterTestHelper( 0.0, 5.0 );
			CenterTestHelper( DateTime.MinValue, TimeSpan.FromDays( 1 ) );
		}

		static void CenterTestHelper<T, TSize>( T start, TSize addition )
			where T : IComparable<T>
		{
			T center = AddSize( start, addition );
			T end = AddSize( center, addition );

			var interval = new Interval<T, TSize>( start, end );
			Assert.AreEqual( center, interval.Center );
		}

		#endregion // Center Test


		#region Size Test

		[TestMethod]
		public void SizeTest()
		{
			SizeTestHelper( 0, 10, 10 );
			SizeTestHelper( 5, 10, 5 );
			SizeTestHelper( StartDate, EndDate, TimeDifference );
		}

		public void SizeTestHelper<T, TSize>( T start, T end, TSize size )
			where T : IComparable<T>
		{
			var interval = new Interval<T, TSize>( start, end );
			Assert.AreEqual( size, interval.Size );
		}

		#endregion // Size Test


		#region GetValueAtTest

		[TestMethod]
		public void GetValueAtTest()
		{
			GetValueAtTestHelper( 0, 10, 100, 0.1 );
			GetValueAtTestHelper( StartDate, TimeSpan.FromHours( 1 ), EndDate, 1.0 / 24 );
		}

		public void GetValueAtTestHelper<T, TSize>( T start, TSize addition, T end, double percentage )
			where T : IComparable<T>
		{
			var valueAt = AddSize( start, addition );

			var interval = new Interval<T, TSize>( start, end );
			Assert.AreEqual( valueAt, interval.GetValueAt( percentage ) );
		}

		#endregion // GetValueAtTest


		#region GetPercentageFor Tests

		[TestMethod]
		public void GetPercentageForTest()
		{
			GetPercentageForHelper( 5, 5 );
			GetPercentageForHelper( 5.0, 5.0 );
			GetPercentageForHelper( StartDate, TimeDifference );
		}

		static void GetPercentageForHelper<T, TSize>( T a, TSize b )
			where T : IComparable<T>
		{
			var aMinusB = SubtractSize( a, b );
			var aPlusB = AddSize( a, b );

			// Single range.
			var single = new Interval<T, TSize>( a, a );
			Assert.AreEqual( 1.0, single.GetPercentageFor( a ) );
			Assert.AreEqual( 0.0, single.GetPercentageFor( aMinusB ) );


			// Normal ranges. ( start < end )
			var right = new Interval<T, TSize>( a, aPlusB );
			var left = new Interval<T, TSize>( aMinusB, a );
			var big = new Interval<T, TSize>( aMinusB, aPlusB );

			// Reversed ranges. ( end < start )
			var reversedRight = new Interval<T, TSize>( aPlusB, a );
			var reversedLeft = new Interval<T, TSize>( a, aMinusB );
			var reversedBig = new Interval<T, TSize>( aPlusB, aMinusB );


			// Should lie at start.
			Assert.AreEqual( 0.0, right.GetPercentageFor( a ) );
			Assert.AreEqual( 0.0, reversedRight.GetPercentageFor( aPlusB ) );

			// Should lie in middle.
			Assert.AreEqual( 0.5, big.GetPercentageFor( a ) );
			Assert.AreEqual( 0.5, reversedBig.GetPercentageFor( a ) );

			// Should lie at end.
			Assert.AreEqual( 1.0, left.GetPercentageFor( a ) );
			Assert.AreEqual( 1.0, reversedLeft.GetPercentageFor( aMinusB ) );

			// Should lie before.
			Assert.AreEqual( -1.0, right.GetPercentageFor( aMinusB ) );
			Assert.AreEqual( -1.0, reversedLeft.GetPercentageFor( aPlusB ) );

			// Should lie after.
			Assert.AreEqual( 2.0, left.GetPercentageFor( aPlusB ) );
			Assert.AreEqual( 2.0, reversedRight.GetPercentageFor( aMinusB ) );
		}

		#endregion  // GetPercentageFor Tests


		#region Map Tests

		[TestMethod]
		public void MapTest()
		{
			// Same type ranges.
			MapHelper( 0, 10, 10, 10, 0, 10 ); // 0%
			MapHelper( 0, 30, 10, 30, 10, 20 ); // 33%
			MapHelper( 0, 10, 10, 10, 5, 15 ); // 50%
			MapHelper( 0, 10, 10, 10, 10, 20 ); // 100%
			MapHelper( 0, 4, 0, 100, 1, 25 ); // Different range sizes.
			MapHelper( -100, 200, 0, 100, 0, 50 ); // Negative to positive.
			MapHelper( 10, 10, -100, 150, 15, -25 ); // Positive to negative.
			MapHelper( 10, -10, 0, 10, 5, 5 ); // Reversed ranges.
			MapHelper( 0, 10, 100, -100, 5, 50 );
			MapHelper( 0.0, 1.0, 0.0, 100.0, 0.5, 50 ); // Double math.
			MapHelper(
				new DateTime( 2014, 12, 1 ), TimeSpan.FromDays( 1 ),
				new DateTime( 2014, 12, 3 ), TimeSpan.FromDays( 1 ),
				new DateTime( 2014, 12, 1, 12, 0, 0 ), new DateTime( 2014, 12, 3, 12, 0, 0 ) ); // DateTime math.

			// Different type ranges.
			MapHelper( 0, 10, 10.0, 10.0, 0, 10 ); // 0%
			MapHelper( 0, 30, 10.0, 30.0, 10, 20 ); // 33%
			MapHelper( 0, 10, 10.0, 10.0, 5, 15 ); // 50%
			MapHelper( 0, 10, 10.0, 10.0, 10, 20 ); // 100%
			MapHelper( 0, 4, 0.0, 100.0, 1, 25 ); // Different range sizes.
			MapHelper( -100, 200, 0.0, 100.0, 0, 50 ); // Negative to positive.
			MapHelper( 10, 10, -100.0, 150.0, 15, -25 ); // Positive to negative.
			MapHelper( 10, -10, 0.0, 10.0, 5, 5 ); // Reversed ranges.
			MapHelper( 0, 10, 100.0, -100.0, 5, 50 );
			MapHelper( 0, 10, new DateTime( 2014, 12, 1 ), TimeSpan.FromDays( 1 ), 5, new DateTime( 2014, 12, 1, 12, 0, 0 ) );
		}

		// ReSharper disable UnusedParameter.Local
		static void MapHelper<T, TSize, TOther, TOtherSize>(
			T from, TSize addition,
			TOther mapFrom, TOtherSize mapAddition,
			T valueToMap, TOther expected )
			where T : IComparable<T>
			where TOther : IComparable<TOther>
		{
			IInterval<T, TSize> interval = NewInterval( from, addition );
			TOther result = interval.Map( valueToMap, NewInterval( mapFrom, mapAddition ) );

			Assert.AreEqual( expected, result );
		}
		// ReSharper restore UnusedParameter.Local

		#endregion  // Map Tests


		#region LiesInInterval Tests

		[TestMethod]
		public void LiesInIntervalTest()
		{
			// Just one value in interval.
			var single = new Interval<int, int>( 0, 0 );
			LiesInInterval( single, 0, true );
			LiesInInterval( single, 1, false );

			// Inside/outside interval.
			LiesInIntervalTestHelper( 0, 10, 5, 11 );
			LiesInIntervalTestHelper( 0.5, 10.0, 5, 12 );
			LiesInIntervalTestHelper( StartDate, TimeSpan.FromDays( 1 ), StartDate + TimeSpan.FromHours( 1 ), StartDate - TimeSpan.FromHours( 1 ) );

			// Border of the interval.
			var border = new Interval<int, int>( 0, true, 10, false );
			LiesInInterval( border, 0, true );
			LiesInInterval( border, 10, false );
		}

		static void LiesInIntervalTestHelper<T, TSize>( T start, TSize addition, T inside, T outside )
			where T : IComparable<T>
		{
			T end = AddSize( start, addition );

			var included = new Interval<T>( start, end );
			LiesInInterval( included, inside, true );
			LiesInInterval( included, start, true ); // On edge.
			LiesInInterval( included, outside, false ); // Outside.

			var excluded = new Interval<T>( start, false, end, false );
			LiesInInterval( excluded, start, false ); // Left edge.
			LiesInInterval( excluded, end, false ); // Right edge.
		}

		static void LiesInInterval<T, TSize>( IInterval<T, TSize> interval, T value, bool expected )
			where T : IComparable<T>
		{
			bool insideInterval = interval.LiesInInterval( value );

			Assert.AreEqual( expected, insideInterval );
		}

		#endregion  // LiesInInterval Tests


		#region Intersects Tests

		[TestMethod]
		public void IntersectsTest()
		{
			IntersectsTestHelper<int, int>( 0, 5, 10, 20, 100 );
			IntersectsTestHelper<double, double>( 0.5, 10.0, 20.5, 50.6, 100.9 );
			IntersectsTestHelper<DateTime, TimeSpan>(
				StartDate,
				StartDate + TimeSpan.FromDays( 5 ),
				StartDate + TimeSpan.FromDays( 10 ),
				StartDate + TimeSpan.FromDays( 20 ),
				StartDate + TimeSpan.FromDays( 100 )
				);
		}

		/// <summary>
		///   Test intersecting intervals with given parameters from small to bigger.
		/// </summary>
		static void IntersectsTestHelper<T, TSize>( T a, T b, T c, T d, T e )
			where T : IComparable<T>
		{
			// Create required intervals.
			var ab = new Interval<T, TSize>( a, b );
			var bc = new Interval<T, TSize>( b, c );
			var cd = new Interval<T, TSize>( c, d );
			var de = new Interval<T, TSize>( d, e );
			var ad = new Interval<T, TSize>( a, d );
			var ac = new Interval<T, TSize>( a, c );
			var bd = new Interval<T, TSize>( b, d );
			var bcExcluded = new Interval<T, TSize>( b, false, c, false );

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

		static void Intersects<T, TSize>( IInterval<T, TSize> a, IInterval<T, TSize> b, bool expected )
			where T : IComparable<T>
		{
			bool intersects = a.Intersects( b );
			Assert.AreEqual( expected, intersects );
		}

		#endregion  // Intersects Tests


		#region Clamp Tests

		[TestMethod]
		public void ClampValueTest()
		{
			// Within/outside ranges.
			ClampValueTestHelper( 0, 10, 5, 5 );
			ClampValueTestHelper( 0, 10, 11, 10 );
			ClampValueTestHelper( 0, 10, -1, 0 );

			// Borders.
			ClampValueTestHelper( 0, 10, 0, 0 );
			ClampValueTestHelper( 0, 10, 10, 10 );

			// Types.
			ClampValueTestHelper( 0.0, 10.0, 5, 5 );
			ClampValueTestHelper( StartDate, TimeDifference, StartDate - TimeDifference, StartDate );
		}

		static void ClampValueTestHelper<T, TSize>( T start, TSize addition, T actual, T clamped )
			where T : IComparable<T>
		{
			T end = AddSize( start, addition );

			// Fully included interval.
			var interval = new Interval<T, TSize>( start, end );
			Assert.AreEqual( clamped, interval.Clamp( actual ) );

			// Reversed intervals.
			var reversed = new Interval<T, TSize>( end, start );
			Assert.AreEqual( clamped, reversed.Clamp( actual ) );

			// Excluded borders.
			// TODO: For now it does not matter whether start or end is included. Do we want to take this into account?
			var excludedInterval = new Interval<T, TSize>( start, false, end, false );
			Assert.AreEqual( clamped, excludedInterval.Clamp( actual ) );
		}

		[TestMethod]
		public void ClampIntervalTest()
		{
			// Within/outside range.
			ClampIntervalTestHelper( 0, true, 10, true, 1, true, 8, true, new Interval<int, int>( 1, 9 ) );
			ClampIntervalTestHelper( 0, true, 10, true, -1, true, 12, true, new Interval<int, int>( 0, 10 ) );

			// Partially overlapping.
			ClampIntervalTestHelper( 0, true, 10, true, -5, true, 10, true, new Interval<int, int>( 0, 5 ) );
			ClampIntervalTestHelper( 0, true, 10, true, 5, true, 10, true, new Interval<int, int>( 5, 10 ) );

			// Borders.
			ClampIntervalTestHelper( 0, true, 10, true, 0, true, 5, true, new Interval<int, int>( 0, 5 ) );
			ClampIntervalTestHelper( 0, true, 10, true, 5, true, 5, true, new Interval<int, int>( 5, 10 ) );
			ClampIntervalTestHelper( 0, true, 10, true, 0, true, 10, true, new Interval<int, int>( 0, 10 ) );

			// Included/excluded borders.
			ClampIntervalTestHelper( 0, false, 10, false, 0, true, 10, true, new Interval<int, int>( 0, false, 10, false ) );
			ClampIntervalTestHelper( 0, true, 10, true, 0, false, 10, false, new Interval<int, int>( 0, false, 10, false ) );
			ClampIntervalTestHelper( 0, false, 10, true, -10, true, 20, true, new Interval<int, int>( 0, false, 10, true ) );

			// Types.
			DateTime halfWay = StartDate + TimeSpan.FromTicks( TimeDifference.Ticks / 2 );
			ClampIntervalTestHelper(
				StartDate, true, TimeDifference, true,
				halfWay, true, TimeDifference, true,
				new Interval<DateTime, TimeSpan>( halfWay, StartDate + TimeDifference ) );
		}

		static void ClampIntervalTestHelper<T, TSize>(
			T startA, bool startAIncluded, TSize additionA, bool endAIncluded,
			T startB, bool startBIncluded, TSize additionB, bool endBIncluded,
			IInterval<T, TSize> clamped )
			where T : IComparable<T>
		{
			// Fully included intervals.
			IInterval<T, TSize> intervalA = NewInterval( startA, additionA, startAIncluded, endAIncluded );
			IInterval<T, TSize> intervalB = NewInterval( startB, additionB, startBIncluded, endBIncluded );
			var result = intervalA.Clamp( intervalB );
			Assert.AreEqual( clamped, result );
		}

		#endregion // Clamp Tests


		#region Split Tests

		[TestMethod]
		public void SplitTest()
		{
			// Ordinary split with different split options.
			SplitTestHelper( 0, 10, 5, SplitOption.Both, new Interval<int, int>( 0, 5 ), new Interval<int, int>( 5, 10 ) );
			SplitTestHelper( 0, 10, 5, SplitOption.Left, new Interval<int, int>( 0, 5 ), new Interval<int, int>( 5, false, 10, true ) );
			SplitTestHelper( 0, 10, 5, SplitOption.Right, new Interval<int, int>( 0, true, 5, false ), new Interval<int, int>( 5, 10 ) );
			SplitTestHelper( 0, 10, 5, SplitOption.None, new Interval<int, int>( 0, true, 5, false ), new Interval<int, int>( 5, false, 10, true ) );

			// Split on borders.
			SplitTestHelper( 0, 10, 0, SplitOption.Both, new Interval<int, int>( 0, 0 ), new Interval<int, int>( 0, 10 ) );
			SplitTestHelper( 0, 10, 0, SplitOption.Left, new Interval<int, int>( 0, 0 ), new Interval<int, int>( 0, false, 10, true ) );
			SplitTestHelper( 0, 10, 0, SplitOption.Right, null, new Interval<int, int>( 0, true, 10, true ) );

			// Types.
			DateTime halfWay = StartDate + TimeSpan.FromTicks( TimeDifference.Ticks / 2 );
			DateTime end = StartDate + TimeDifference;
			SplitTestHelper(
				StartDate, TimeDifference, halfWay, SplitOption.Both,
				new Interval<DateTime, TimeSpan>( StartDate, halfWay ), new Interval<DateTime, TimeSpan>( halfWay, end ) );

			// Invalid split point.
			AssertHelper.ThrowsException<ArgumentException>( () => SplitTestHelper( 0, 10, -5, SplitOption.Both, null, null ) );
			AssertHelper.ThrowsException<ArgumentException>( () => SplitTestHelper( 0, 10, 11, SplitOption.Both, null, null ) );
		}

		static void SplitTestHelper<T, TSize>(
			T start, TSize addition, T atPoint, SplitOption splitOption,
			IInterval<T, TSize> expectedBefore, IInterval<T, TSize> expectedAfter )
			where T : IComparable<T>
		{
			IInterval<T, TSize> interval = NewInterval( start, addition );
			IInterval<T, TSize> before;
			IInterval<T, TSize> after;
			interval.Split( atPoint, splitOption, out before, out after );

			Assert.AreEqual( expectedBefore, before );
			Assert.AreEqual( expectedAfter, after );
		}

		#endregion // Split Tests


		#region Subtract Tests

		[TestMethod]
		public void SubtractTest()
		{
			SubtractTestHelper<int, int>( 0, 5, 10, 20, 100 );
			SubtractTestHelper<double, double>( 0.5, 10.0, 20.5, 50.6, 100.9 );
			SubtractTestHelper<DateTime, TimeSpan>(
				StartDate,
				StartDate + TimeSpan.FromDays( 5 ),
				StartDate + TimeSpan.FromDays( 10 ),
				StartDate + TimeSpan.FromDays( 20 ),
				StartDate + TimeSpan.FromDays( 100 ) );
		}

		/// <summary>
		///   Test intersecting intervals with given parameters from small to bigger.
		/// </summary>
		static void SubtractTestHelper<T, TSize>( T a, T b, T c, T d, T e )
			where T : IComparable<T>
		{
			// Create required intervals.
			var ab = new Interval<T, TSize>( a, b );
			var bc = new Interval<T, TSize>( b, c );
			var cd = new Interval<T, TSize>( c, d );
			var de = new Interval<T, TSize>( d, e );
			var ad = new Interval<T, TSize>( a, d );
			var ac = new Interval<T, TSize>( a, c );
			var bd = new Interval<T, TSize>( b, d );
			var bcExcluded = new Interval<T, TSize>( b, false, c, false );

			// None overlapping intervals.
			Subtract( ab, de, ab ); // Left.
			Subtract( de, ab, de ); // Right.

			// Neighboring intervals.
			Subtract( ab, bc, new Interval<T, TSize>( a, true, b, false ) ); // Left.
			Subtract( bc, ab, new Interval<T, TSize>( b, false, c, true ) ); // Right.
			Subtract( ab, bcExcluded, ab ); // Left.
			Subtract( bcExcluded, cd, bcExcluded ); // Right.

			// Entirely overlapping intervals.
			Subtract( ab, ab, new List<Interval<T, TSize>>() ); // Identical. 
			Subtract( bc, bcExcluded,
				new List<Interval<T, TSize>>
				{
					new Interval<T, TSize>( b, true, b, true ),
					new Interval<T, TSize>( c, true, c, true )
				}
				); // Identical except borders.           
			Subtract( bc, ad, new List<Interval<T, TSize>>() ); // Inside.
			Subtract( ad, bc,
				new List<Interval<T, TSize>>
				{
					new Interval<T, TSize>( a, true, b, false ),
					new Interval<T, TSize>( c, false, d, true )
				}
				); // Outside.
			Subtract( ab, ac, new List<Interval<T, TSize>>() ); // Left.
			Subtract( bc, ac, new List<Interval<T, TSize>>() ); // Right.            

			// Partly overlapping intervals.
			Subtract( ac, bd, new Interval<T, TSize>( a, true, b, false ) ); // Right overlap.
			Subtract( bd, ac, new Interval<T, TSize>( c, false, d, true ) ); // Left overlap.
		}

		// ReSharper disable UnusedParameter.Local
		static void Subtract<T, TSize>( IInterval<T, TSize> from, IInterval<T, TSize> subtract, Interval<T, TSize> result )		
			where T : IComparable<T>
		{
			List<IInterval<T, TSize>> results = from.Subtract( subtract );

			Assert.IsTrue( results.Count == 1 );
			Assert.IsTrue( results[ 0 ].Equals( result ) );
		}
		// ReSharper restore UnusedParameter.Local

		static void Subtract<T, TSize>( IInterval<T, TSize> from, IInterval<T, TSize> subtract, List<Interval<T, TSize>> results )
			where T : IComparable<T>
		{
			List<IInterval<T, TSize>> subtracted = from.Subtract( subtract );

			Assert.IsTrue( results.Count == subtracted.Count );
			for ( int i = 0; i < results.Count; ++i )
			{
				Interval<T, TSize> result = results[ i ];
				IInterval<T, TSize> subtractResult = subtracted[ i ];
				Assert.IsTrue( result.Equals( subtractResult ) );
			}
		}

		#endregion  // Subtract Tests


		#region Intersection Tests

		[TestMethod]
		public void IntersectionTest()
		{
			// No intersection.
			IntersectionTestHelper( 0, true, 10, true, 20, true, 10, true, null );

			// Fully within, fully outside.
			IntersectionTestHelper( 0, true, 10, true, 1, true, 8, true, new Interval<int, int>( 1, 9 ) );
			IntersectionTestHelper( 1, true, 8, true, 0, true, 10, true, new Interval<int, int>( 1, 9 ) );

			// On borders.
			IntersectionTestHelper( 0, true, 10, true, 0, true, 10, true, new Interval<int, int>( 0, 10 ) );
			IntersectionTestHelper( 0, true, 10, true, 5, true, 5, true, new Interval<int, int>( 5, 10 ) );
			IntersectionTestHelper( 0, true, 10, true, 0, true, 5, true, new Interval<int, int>( 0, 5 ) );
			IntersectionTestHelper( 0, true, 10, true, 10, true, 10, true, new Interval<int, int>( 10, 10 ) );

			// Included/excluded borders.
			IntersectionTestHelper( 0, false, 10, false, 1, false, 8, false, new Interval<int, int>( 1, false, 9, false ) );
			IntersectionTestHelper( 0, false, 10, false, 0, true, 5, true, new Interval<int, int>( 0, false, 5, true ) );
			IntersectionTestHelper( 0, true, 10, false, 10, false, 10, true, null );

			// Types.
			IntersectionTestHelper(
				new DateTime( 2014, 12, 1 ), true, TimeSpan.FromDays( 1 ), true,
				new DateTime( 2014, 12, 1, 12, 0, 0, 0 ), true, TimeSpan.FromDays( 1 ), true,
				new Interval<DateTime, TimeSpan>( new DateTime( 2014, 12, 1, 12, 0, 0 ), new DateTime( 2014, 12, 2, 0, 0, 0 ) ) );
		}

		static void IntersectionTestHelper<T, TSize>(
			T startA, bool startAIncluded, TSize additionA, bool endAIncluded,
			T startB, bool startBIncluded, TSize additionB, bool endBIncluded,
			IInterval<T, TSize> intersection )
			where T : IComparable<T>
		{
			IInterval<T, TSize> intervalA = NewInterval( startA, additionA, startAIncluded, endAIncluded );
			IInterval<T, TSize> intervalB = NewInterval( startB, additionB, startBIncluded, endBIncluded );

			Assert.AreEqual( intersection, intervalA.Intersection( intervalB ) );
		}

		#endregion // Intersection Tests


		#region ExpandTo Tests

		[TestMethod]
		public void ExpandToTest()
		{
			// Extend no change/left/right.
			ExpandToTestHelper( 0, true, 10, true, 5, new Interval<int, int>( 0, 10 ) );
			ExpandToTestHelper( 0, true, 10, true, 11, new Interval<int, int>( 0, 11 ) );
			ExpandToTestHelper( 0, true, 10, true, -1, new Interval<int, int>( -1, 10 ) );

			// Extend excluded intervals.
			ExpandToTestHelper( 0, true, 10, false, 10, new Interval<int, int>( 0, 10 ) );
			ExpandToTestHelper( 0, false, 10, true, 0, new Interval<int, int>( 0, 10 ) );

			// Extend but don't exclude.
			IInterval<int, int> excluded = NewInterval( 0, 10, true, false );
			Assert.AreEqual( new Interval<int, int>( 0, true, 20, false ), excluded.ExpandTo( 20, false ) );
			Assert.AreEqual( excluded, excluded.ExpandTo( 10, false ) );

			// Types.
			DateTime extended = StartDate + TimeSpan.FromTicks( TimeDifference.Ticks * 2 );
			ExpandToTestHelper( StartDate, true, TimeDifference, true, extended, new Interval<DateTime, TimeSpan>( StartDate, extended ) );
		}

		static void ExpandToTestHelper<T, TSize>(
			T start, bool startIncluded,
			TSize addition, bool endIncluded,
			T extendTo, Interval<T, TSize> extended )
			where T : IComparable<T>
		{
			var interval = NewInterval( start, addition, startIncluded, endIncluded );
			Assert.AreEqual( extended, interval.ExpandTo( extendTo ) );
		}

		#endregion // ExpandTo Tests


		#region Move Tests

		[TestMethod]
		public void MoveTest()
		{
			// Forward move.
			MoveTestHelper( 0, true, 10, true, 10, new Interval<int, int>( 10, 20 ) );
			MoveTestHelper( 0.3, true, 10, true, 10, new Interval<double, double>( 10.3, 20.3 ) );

			// Excluded intervals move.
			MoveTestHelper( 0, false, 10, true, 10, new Interval<int, int>( 10, false, 20, true ) );

			// Backwards move.
			MoveTestHelper( 0, true, 10, true, -10, new Interval<int, int>( -10, 0 ) );

			// Reversed intervals.
			MoveTestHelper( 10, true, -10, true, 10, new Interval<int, int>( 20, 10 ) );

			// Types.
			MoveTestHelper(
				new DateTime( 2014, 1, 30 ), true,
				TimeSpan.FromDays( 1 ), true,
				TimeSpan.FromDays( 1 ),
				new Interval<DateTime, TimeSpan>( new DateTime( 2014, 1, 31 ), new DateTime( 2014, 2, 1 ) ) );
		}

		static void MoveTestHelper<T, TSize>( T start, bool startIncluded, TSize addition, bool endIncluded, TSize move, IInterval<T, TSize> moved )
			where T : IComparable<T>
		{
			IInterval<T, TSize> interval = NewInterval( start, addition, startIncluded, endIncluded );
			Assert.AreEqual( moved, interval.Move( move ) );
		}

		#endregion // Move Tests


		#region Scale Tests

		[TestMethod]
		public void ScaleTest()
		{
			// Positive intervals scale down.
			ScaleTestHelper( 0, 100, 0.5, 0.0, new Interval<int, int>( 0, 50 ) );
			ScaleTestHelper( 0, 100, 0.5, 0.5, new Interval<int, int>( 25, 75 ) );
			ScaleTestHelper( 0, 100, 0.5, 1.0, new Interval<int, int>( 50, 100 ) );

			// Positive intervals scale up. [200, 400]
			ScaleTestHelper( 200, 200, 2.0, 0.0, new Interval<int, int>( 200, 600 ) );
			ScaleTestHelper( 200, 200, 2.0, 0.5, new Interval<int, int>( 100, 500 ) );
			ScaleTestHelper( 200, 200, 2.0, 1.0, new Interval<int, int>( 0, 400 ) );

			// Negative intervals. [-200, -100]
			ScaleTestHelper( -200, 100, 0.5, 0.0, new Interval<int, int>( -200, -150 ) );
			ScaleTestHelper( -200, 100, 2.0, 0.5, new Interval<int, int>( -250, -50 ) );
			ScaleTestHelper( -200, 100, 0.5, 1.0, new Interval<int, int>( -150, -100 ) );

			// Mixed interval. [-100, 100]
			ScaleTestHelper( -100, 200, 0.5, 0.5, new Interval<int, int>( -50, 50 ) );
			ScaleTestHelper( -100, 200, 2.0, 0.5, new Interval<int, int>( -200, 200 ) );

			// Types.
			ScaleTestHelper(
				new DateTime( 2014, 12, 1 ), TimeSpan.FromDays( 1 ), 0.5, 0.5,
				new Interval<DateTime,TimeSpan>( new DateTime( 2014, 12, 1, 6, 0, 0 ), new DateTime( 2014, 12, 1, 18, 0, 0 ) ) );
		}

		static void ScaleTestHelper<T, TSize>( T start, TSize addition, double scale, double aroundPercentage, IInterval<T, TSize> scaled )
			where T : IComparable<T>
		{
			IInterval<T, TSize> interval = NewInterval( start, addition );
			Assert.AreEqual( scaled, interval.Scale( scale, aroundPercentage ) );
		}

		#endregion // Scale Tests


		static Interval<T, TSize> NewInterval<T, TSize>( T start, TSize addition, bool startIncluded = true, bool endIncluded = true )
			where T : IComparable<T>
		{
			return new Interval<T, TSize>( start, startIncluded, AddSize( start, addition ), endIncluded );
		}
		
		static T AddSize<T, TSize>( T start, TSize addition )
		{
			return Operator<T, TSize>.AddSize( start, addition );
		}

		static T SubtractSize<T, TSize>( T start , TSize subtraction )
		{
			return Operator<T, TSize>.SubtractSize( start, subtraction );
		}
	}
}