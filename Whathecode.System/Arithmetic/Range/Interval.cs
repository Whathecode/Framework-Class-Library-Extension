﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Whathecode.System.Algorithm;
using Whathecode.System.Extensions;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Class specifying an interval from a value, to a value. Borders may be included or excluded. This type is immutable.
	/// </summary>
	/// <remarks>
	///   This is a wrapper class which simply redirect calls to a more generic base type.
	/// </remarks>
	/// <typeparam name = "T">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	[DataContract]
	public class Interval<T> : Interval<T, T>
		where T : IComparable<T>
	{
		/// <summary>
		///   Create a new interval with a specified start and end, both included in the interval.
		/// </summary>
		/// <param name = "start">The start of the interval, included in the interval.</param>
		/// <param name = "end">The end of the interval, included in the interval.</param>
		public Interval( T start, T end )
			: base( start, end ) {}

		/// <summary>
		///   Create a new interval with a specified start and end.
		/// </summary>
		/// <param name = "start">The start of the interval.</param>
		/// <param name = "isStartIncluded">Is the value at the start of the interval included in the interval.</param>
		/// <param name = "end">The end of the interval.</param>
		/// <param name = "isEndIncluded">Is the value at the end of the interval included in the interval.</param>
		public Interval( T start, bool isStartIncluded, T end, bool isEndIncluded )
			: base( start, isStartIncluded, end, isEndIncluded ) {}

		/// <summary>
		///   Create a less generic interval from a more generic base type.
		/// </summary>
		/// <param name = "interval">The more generic base type.</param>
		public Interval( Interval<T, T> interval )
			: base( interval.Start, interval.IsStartIncluded, interval.End, interval.IsEndIncluded ) {}


		/// <summary>
		///   Limit a given range to this range.
		///   When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range.</returns>
		public Interval<T> Clamp( Interval<T> range )
		{
			return new Interval<T>( base.Clamp( range ) );
		}

		/// <summary>
		///   Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		public void Split( T atPoint, SplitOption option, out Interval<T> before, out Interval<T> after )
		{
			Interval<T, T> beforeInner;
			Interval<T, T> afterInner;
			Split( atPoint, option, out beforeInner, out afterInner );
			before = new Interval<T>( beforeInner );
			after = new Interval<T>( afterInner );
		}

		/// <summary>
		///   Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		public List<Interval<T>> Subtract( Interval<T> subtract )
		{
			List<Interval<T, T>> result = base.Subtract( subtract );
			return result.Select( r => new Interval<T>( r ) ).ToList();
		}

		/// <summary>
		///   Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		public Interval<T> Intersection( Interval<T> interval )
		{
			return new Interval<T>( base.Intersection( interval ) );
		}

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given value (and including).
		///   When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		public new Interval<T> ExpandTo( T value )
		{
			return new Interval<T>( base.ExpandTo( value ) );
		}

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given value.
		///   When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		public new Interval<T> ExpandTo( T value, bool include )
		{
			return new Interval<T>( base.ExpandTo( value, include ) );
		}

		/// <summary>
		///   Returns an interval offsetted from the current interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		public new Interval<T> Move( T amount )
		{
			return new Interval<T>( base.Move( amount ) );
		}

		/// <summary>
		///   Returns a scaled version of the current interval.
		/// </summary>
		/// <param name="scale">
		///   Percentage to scale the interval up or down.
		///   Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		public new Interval<T> Scale( double scale, double aroundPercentage = 0.5 )
		{
			return new Interval<T>( base.Scale( scale, aroundPercentage ) );
		}

		public new object Clone()
		{
			return new Interval<T>( Start, IsStartIncluded, End, IsEndIncluded );
		}
	}


	/// <summary>
	///   Class specifying an interval from a value, to a value. Borders may be included or excluded. This type is immutable.
	/// </summary>
	/// <typeparam name = "T">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <typeparam name = "TSize">The type used to specify distances in between two values of <see cref="T" />.</typeparam>
	/// <author>Steven Jeuris</author>
	[DataContract]
	public class Interval<T, TSize>
		where T : IComparable<T>
	{
		// ReSharper disable StaticFieldInGenericType
		readonly static bool IsIntegralType;
		// ReSharper restore StaticFieldInGenericType

		// TODO: Is there any benefit moving these converter functions to a factory which injects them through constructor injection instead?
		/// <summary>
		///   A function which can convert <see cref="TSize" /> to a double representation.
		///   The function is used at runtime by any instance of this type to perform double calculations.
		/// </summary>
		public static Func<TSize, double> ConvertSizeToDouble { get; set; }
		/// <summary>
		///   A function which can convert a double to <see cref="TSize" />.
		///   This function is used at runtime by any instance of this type to perform double calculations.
		/// </summary>
		public static Func<double, TSize> ConvertDoubleToSize { get; set; }

		public readonly static Interval<T, TSize> Empty;

		[DataMember]
		readonly T _start;
		/// <summary>
		///   The start of the interval.
		/// </summary>
		public T Start { get { return _start; } }

		[DataMember]
		readonly T _end;
		/// <summary>
		///   The end of the interval.
		/// </summary>
		public T End { get { return _end; } }

		[DataMember]
		readonly bool _isStartIncluded;
		/// <summary>
		///   Is the value at the start of the interval included in the interval.
		/// </summary>
		public bool IsStartIncluded { get { return _isStartIncluded; } }

		[DataMember]
		readonly bool _isEndIncluded;
		/// <summary>
		///   Is the value at the end of the interval included in the interval.
		/// </summary>
		public bool IsEndIncluded { get { return _isEndIncluded; } }

		/// <summary>
		///   Determines whether the start of the interval lies before or after the end of the interval. true when before, false when behind.
		/// </summary>
		public bool IsReversed { get { return _start.CompareTo( _end ) > 0; } }

		/// <summary>
		///   Get the value in the center of the interval. Rounded to the nearest correct value.
		/// </summary>
		public T Center
		{
			get { return GetValueAt( 0.5 ); }
		}

		/// <summary>
		///   Get the size of the interval.
		/// </summary>
		public TSize Size
		{
			get
			{
				return IsReversed
					? Operator<T, TSize>.Subtract( Start, End )
					: Operator<T, TSize>.Subtract( End, Start );
			}
		}

		static Interval()
		{
			IsIntegralType = TypeHelper.IsIntegralNumericType<TSize>();

			// Initialize Empty.
			T zero = default( T );
			Empty = new Interval<T, TSize>( zero, false, zero, false );

			// Verify whether default convertion operators are available to and from double.
			try
			{
				ConvertSizeToDouble = CastOperator<TSize, double>.Cast;
				ConvertDoubleToSize = CastOperator<double, TSize>.Cast;
			}
			catch ( TypeInitializationException )
			{
				ConvertSizeToDouble = null;
				ConvertDoubleToSize = null;
			}
		}

		/// <summary>
		///   Create a new interval with a specified start and end, both included in the interval.
		/// </summary>
		/// <param name = "start">The start of the interval, included in the interval.</param>
		/// <param name = "end">The end of the interval, included in the interval.</param>
		public Interval( T start, T end )
			: this( start, true, end, true ) {}

		/// <summary>
		///   Create a new interval with a specified start and end.
		/// </summary>
		/// <param name = "start">The start of the interval.</param>
		/// <param name = "isStartIncluded">Is the value at the start of the interval included in the interval.</param>
		/// <param name = "end">The end of the interval.</param>
		/// <param name = "isEndIncluded">Is the value at the end of the interval included in the interval.</param>
		public Interval( T start, bool isStartIncluded, T end, bool isEndIncluded )
		{
			Contract.Requires(
				end.CompareTo( start ) != 0 || (end.CompareTo( start ) == 0 && isStartIncluded && isEndIncluded),
				"Invalid interval arguments. e.g. ]0, 0]" );

			_start = start;
			_isStartIncluded = isStartIncluded;
			_end = end;
			_isEndIncluded = isEndIncluded;
		}


		double Convert( TSize size )
		{
			CheckForInvalidImplementation();
			return ConvertSizeToDouble( size );
		}

		TSize Convert( double size )
		{
			CheckForInvalidImplementation();
			return ConvertDoubleToSize( size );
		}

		void CheckForInvalidImplementation()
		{
			if ( ConvertSizeToDouble == null || ConvertDoubleToSize == null )
			{
				Type type = typeof( Interval<T, TSize> );
				const string message =
					"In order to use {0} you need to set 'ConvertSizeToDouble' and 'ConvertDoubleToSize'. " +
					"These converters could not be generated automatically for the specified type parameters.";
				throw new InvalidImplementationException( String.Format( message, type ) );
			}
		}


		#region Get operations.

		/// <summary>
		///   Get the value at a given percentage within the interval, or on it's borders. Rounding to nearest occurs when needed.
		///   TODO: Would it be cleaner not to use a double for percentage, but a generic Percentage type?
		/// </summary>
		/// <param name = "percentage">The percentage in the range of which to return the value.</param>
		/// <returns>The value at the given percentage within the interval.</returns>
		public T GetValueAt( double percentage )
		{			
			// Use double math for the calculation, and then cast to the desired type.
			double value = percentage * Convert( Size );
			double addition = value * (IsReversed ? -1 : 1); // Subtraction is required for a reversed interval.

			// Ensure nearest neighbour rounding for integral types.
			if ( IsIntegralType )
			{
				addition = Math.Round( addition );
			}

			return Operator<T, TSize>.AddSize( Start, Convert( addition ) );
		}

		/// <summary>
		///   Get a percentage how far inside (or outside) the interval a certain value lies.
		/// </summary>
		/// <param name = "position">The position value to get the percentage for.</param>
		/// <returns>The percentage indicating how far inside (or outside) the interval the given value lies.</returns>
		public double GetPercentageFor( T position )
		{
			double size = Convert( Size );

			// When size is zero, return 1.0 when in interval.
			if ( size == 0 )
			{
				return LiesInInterval( position ) ? 1.0 : 0.0;
			}

			var positionRange = new Interval<T, TSize>( Start, position );
			double percentage = Convert( positionRange.Size ) / size;

			// Negate percentage when position lies before the interval.
			int positionCompare = position.CompareTo( Start );
			bool isPositionBeforeInterval = IsReversed
				? positionCompare > 0
				: positionCompare < 0;
			if ( isPositionBeforeInterval )
			{
				percentage *= -1;
			}

			return percentage;
		}


		/// <summary>
		///   Map a value from this range, to a value in another range linearly.
		/// </summary>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		public T Map( T value, Interval<T, TSize> range )
		{
			return Map<T, TSize>( value, range );
		}

		/// <summary>
		///   Map a value from this range, to a value in another range of another type linearly.
		/// </summary>
		/// <typeparam name = "TOther">The type of the other range.</typeparam>
		/// <typeparam name = "TOtherSize">The type used to specify distances in between two values of <see cref="TOther" />.</typeparam>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		public TOther Map<TOther, TOtherSize>( T value, Interval<TOther, TOtherSize> range )
			where TOther : IComparable<TOther>
		{
			return range.GetValueAt( GetPercentageFor( value ) );
		}

		/// <summary>
		///   Does the given value lie in the interval or not.
		/// </summary>
		/// <param name = "value">The value to check for.</param>
		/// <returns>True when the value lies within the interval, false otherwise.</returns>
		[Pure]
		public bool LiesInInterval( T value )
		{
			int startCompare = value.CompareTo( Start );
			int endCompare = value.CompareTo( End );

			return (startCompare > 0 || (startCompare == 0 && IsStartIncluded))
				&& (endCompare < 0 || (endCompare == 0 && IsEndIncluded));
		}

		/// <summary>
		///   Does the given interval intersect the other interval.
		/// </summary>
		/// <param name = "interval">The interval to check for intersection.</param>
		/// <returns>True when the intervals intersect, false otherwise.</returns>
		public bool Intersects( Interval<T, TSize> interval )
		{
			int rightOfCompare = interval.Start.CompareTo( End );
			int leftOfCompare = interval.End.CompareTo( Start );

			bool liesRightOf = rightOfCompare > 0 || (rightOfCompare == 0 && !(interval.IsStartIncluded && IsEndIncluded));
			bool liesLeftOf = leftOfCompare < 0 || (leftOfCompare == 0 && !(interval.IsEndIncluded && IsStartIncluded));

			return !(liesRightOf || liesLeftOf);
		}

		/// <summary>
		///   Limit a given value to this range. When the value is smaller/bigger than the range, snap it to the range border.
		///   TODO: For now this does not take into account whether the start or end of the range is included. Is this possible?
		/// </summary>
		/// <param name = "value">The value to limit.</param>
		/// <returns>The value limited to the range.</returns>
		public T Clamp( T value )
		{
			T smallest = IsReversed ? End : Start;
			T biggest = IsReversed ? Start : End;

			return value.CompareTo( smallest ) < 0
				? smallest
				: value.CompareTo( biggest ) > 0
					? biggest
					: value;
		}

		/// <summary>
		///   Limit a given range to this range.
		///   When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range.</returns>
		public Interval<T, TSize> Clamp( Interval<T, TSize> range )
		{
			var intersection = Intersection( range );
			if ( intersection == null )
			{
				return Empty;
			}

			T smallest = IsReversed ? End : Start;
			T biggest = IsReversed ? Start : End;
			T clampSmallest = range.IsReversed ? range.End : range.Start;
			T clampBiggest = range.IsReversed ? range.Start : range.End;
			bool thisIsSmaller = smallest.CompareTo( clampSmallest ) <= 0;
			bool thisIsBigger = biggest.CompareTo( clampBiggest ) >= 0;
			return new Interval<T, TSize>(
				thisIsSmaller ? clampSmallest : smallest,
				thisIsSmaller ? intersection.IsStartIncluded : IsStartIncluded,
				thisIsBigger ? clampBiggest : biggest,
				thisIsBigger ? intersection.IsEndIncluded : IsEndIncluded );
		}

		/// <summary>
		///   Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		public void Split( T atPoint, SplitOption option, out Interval<T, TSize> before, out Interval<T, TSize> after )
		{
			if ( atPoint.CompareTo( Start ) < 0 || atPoint.CompareTo( End ) > 0 )
			{
				throw new ArgumentException(
					"The point specifying where to split the interval does not lie within the interval range.", "atPoint" );
			}

			// Part before.
			bool includeInLeft = option.EqualsAny( SplitOption.Left, SplitOption.Both );
			if ( atPoint.CompareTo( Start ) != 0 || includeInLeft )
			{
				before = new Interval<T, TSize>(
					Start, IsStartIncluded,
					atPoint,
					includeInLeft );
			}
			else
			{
				before = null;
			}

			// Part after.
			bool includeInRight = option.EqualsAny( SplitOption.Right, SplitOption.Both );
			if ( atPoint.CompareTo( End ) != 0 || includeInRight )
			{
				after = new Interval<T, TSize>(
					atPoint,
					includeInRight,
					End, IsEndIncluded );
			}
			else
			{
				after = null;
			}
		}

		/// <summary>
		///   Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		public List<Interval<T, TSize>> Subtract( Interval<T, TSize> subtract )
		{
			var result = new List<Interval<T, TSize>>();

			if ( !Intersects( subtract ) )
			{
				// Nothing to subtract.
				result.Add( this );
			}
			else
			{
				bool startInInterval = LiesInInterval( subtract.Start );
				bool endInInterval = LiesInInterval( subtract.End );

				// Add remaining section at the start.   
				if ( startInInterval )
				{
					int startCompare = subtract.Start.CompareTo( Start );
					if ( startCompare > 0 || (startCompare == 0 && IsStartIncluded && !subtract.IsStartIncluded) )
					{
						result.Add( new Interval<T, TSize>( Start, IsStartIncluded, subtract.Start, !subtract.IsStartIncluded ) );
					}
				}

				// Add remaining section at the back.
				if ( endInInterval )
				{
					int endCompare = subtract.End.CompareTo( End );
					if ( endCompare < 0 || (endCompare == 0 && IsEndIncluded && !subtract.IsEndIncluded) )
					{
						result.Add( new Interval<T, TSize>( subtract.End, !subtract.IsEndIncluded, End, IsEndIncluded ) );
					}
				}
			}

			return result;
		}

		/// <summary>
		///   Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		public Interval<T, TSize> Intersection( Interval<T, TSize> interval )
		{
			if ( !Intersects( interval ) )
			{
				return null;
			}

			int startCompare = Start.CompareTo( interval.Start );
			int endCompare = End.CompareTo( interval.End );

			return new Interval<T, TSize>(
				startCompare > 0 ? Start : interval.Start,
				startCompare == 0
					? IsStartIncluded && interval.IsStartIncluded // On matching boundary, only include when they both include the boundary.
					: startCompare > 0
						? IsStartIncluded
						: interval.IsStartIncluded, // Otherwise, use the corresponding boundary.
				endCompare < 0 ? End : interval.End,
				endCompare == 0
					? IsEndIncluded && interval.IsEndIncluded
					: endCompare < 0
						? IsEndIncluded
						: interval.IsEndIncluded
				);
		}

		public override bool Equals( object obj )
		{
			var interval = obj as Interval<T, TSize>;

			if ( interval == null )
			{
				return false;
			}

			return IsStartIncluded == interval.IsStartIncluded
				&& IsEndIncluded == interval.IsEndIncluded
				&& Start.CompareTo( interval.Start ) == 0
				&& End.CompareTo( interval.End ) == 0;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Start.GetHashCode();
				hash = hash * 23 + End.GetHashCode();
				hash = hash * 23 + IsStartIncluded.GetHashCode();
				hash = hash * 23 + IsEndIncluded.GetHashCode();
				return hash;
			}
		}

		#endregion  // Get operations.


		#region Enumeration

		/// <summary>
		///   Execute an action each step in an interval.
		/// </summary>
		/// <param name = "step">The size of the steps.</param>
		/// <param name = "stepAction">The operation to execute.</param>
		public void EveryStepOf( TSize step, Action<T> stepAction )
		{
			foreach ( var i in new IntervalEnumerator<T, TSize>( this, step ) )
			{
				stepAction( i );
			}
		}

		#endregion  // Enumeration


		#region Modifiers

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given value (and including).
		///   When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		public Interval<T, TSize> ExpandTo( T value )
		{
			Contract.Ensures( LiesInInterval( value ) );

			return ExpandTo( value, true );
		}

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given value.
		///   When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		public Interval<T, TSize> ExpandTo( T value, bool include )
		{
			T start = Start;
			T end = End;
			bool isStartIncluded = IsStartIncluded;
			bool isEndIncluded = IsEndIncluded;

			// Modify interval when needed.
			int startCompare = value.CompareTo( Start );
			int endCompare = value.CompareTo( End );
			if ( startCompare <= 0 )
			{
				start = value;
				isStartIncluded |= include;
			}
			if ( endCompare >= 0 )
			{
				end = value;
				isEndIncluded |= include;
			}

			return new Interval<T, TSize>( start, isStartIncluded, end, isEndIncluded );
		}

		/// <summary>
		///   Returns an interval offsetted from the current interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		public Interval<T, TSize> Move( TSize amount )
		{
			return new Interval<T, TSize>(
				Operator<T, TSize>.AddSize( Start, amount ),
				IsStartIncluded,
				Operator<T, TSize>.AddSize( End, amount ),
				IsEndIncluded );
		}

		/// <summary>
		///   Returns a scaled version of the current interval.
		/// </summary>
		/// <param name="scale">
		///   Percentage to scale the interval up or down.
		///   Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		public Interval<T, TSize> Scale( double scale, double aroundPercentage = 0.5 )
		{
			TSize scaledSize = Convert( Convert( Size ) * scale );
			TSize sizeDiff = Operator<TSize>.Subtract( Size, scaledSize ); // > 0 larger, < 0 smaller
			TSize startAddition = Convert( Convert( sizeDiff ) * aroundPercentage );
			TSize endSubtraction = Operator<TSize>.Subtract( sizeDiff, startAddition );
			T start = Operator<T, TSize>.AddSize( Start, startAddition );
			T end = Operator<T, TSize>.SubtractSize( End, endSubtraction );

			return new Interval<T, TSize>( start, IsStartIncluded, end, IsEndIncluded );
		}

		#endregion  // Modifiers


		public object Clone()
		{
			return new Interval<T, TSize>( Start, IsStartIncluded, End, IsEndIncluded );
		}

		public override string ToString()
		{
			string output = IsStartIncluded ? "[" : "]";
			output += Start + ", " + End;
			output += IsEndIncluded ? "]" : "[";
			return output;
		}

		public static Interval<T, TSize> Parse( string interval )
		{
			var exception =
				new ArgumentException( "Incorrectly formatted string, expecting an interval in the format of e.g. \"[0, 1]\".", "interval" );

			// Get groups from formatted interval: e.g. [0, 10] or ]0,0[
			Match match = Regex.Match( interval, @"^([\[\]])(.+)\s*,\s*(.+)([\[\]])$" );
			if ( match.Groups.Count != 5 )
			{
				throw exception;
			}

			// Parse retrieved groups.
			bool isStartIncluded, isEndIncluded;
			T start, end;
			try
			{
				isStartIncluded = ParseIncluded( match.Groups[ 1 ].Value );
				var converter = TypeDescriptor.GetConverter( typeof( T ) );
				start = (T)converter.ConvertFrom( match.Groups[ 2 ].Value );
				end = (T)converter.ConvertFrom( match.Groups[ 3 ].Value );
				isEndIncluded = ParseIncluded( match.Groups[ 4 ].Value );
			}
			catch ( ArgumentException )
			{
				throw exception;
			}

			return new Interval<T, TSize>( start, isStartIncluded, end, isEndIncluded );
		}

		static bool ParseIncluded( string input )
		{
			char bracket = input[ 0 ];
			switch ( bracket )
			{
				case '[':
					return true;
				case ']':
					return false;
				default:
					throw new ArgumentException( "Expecting '[' or ']' to specify interval boundaries.", "input" );
			}
		}
	}
}