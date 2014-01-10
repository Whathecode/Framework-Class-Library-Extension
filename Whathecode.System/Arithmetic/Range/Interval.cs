using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Whathecode.System.Algorithm;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Class specifying an interval from a value, to a value. Borders may be included or excluded.
	///   TODO: Implement GetHashCode(), make struct? Make immutable?
	/// </summary>
	/// <typeparam name = "TMath">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	[DataContract]
	public class Interval<TMath> : IInterval<TMath>
		where TMath : IComparable<TMath>
	{
		// ReSharper disable StaticFieldInGenericType
		readonly static bool IsIntegralType;
		// ReSharper restore StaticFieldInGenericType


		static Interval<TMath> _empty;
		public static IInterval<TMath> Empty
		{
			get
			{
				if ( _empty == null )
				{
					TMath zero = CastOperator<double, TMath>.Cast( 0 );
					_empty = new Interval<TMath>( zero, false, zero, false );
				}

				return _empty;
			}
		}

		/// <summary>
		///   The start of the interval.
		/// </summary>
		[DataMember]
		public TMath Start { get; private set; }

		/// <summary>
		///   The end of the interval.
		/// </summary>
		[DataMember]
		public TMath End { get; private set; }

		/// <summary>
		///   Is the value at the start of the interval included in the interval.
		/// </summary>
		[DataMember]
		public bool IsStartIncluded { get; private set; }

		/// <summary>
		///   Is the value at the end of the interval included in the interval.
		/// </summary>
		[DataMember]
		public bool IsEndIncluded { get; private set; }

		/// <summary>
		///   Determines whether the start of the interval lies before or after the end of the interval. true when before, false when behind.
		/// </summary>
		[DataMember]
		public bool IsReversed { get; private set; }

		/// <summary>
		///   Get the value in the center of the interval. Rounded to the nearest correct value.
		/// </summary>
		public TMath Center
		{
			get { return GetValueAt( 0.5 ); }
		}

		/// <summary>
		///   Get the size of the interval.
		/// </summary>
		public TMath Size
		{
			get
			{
				return IsReversed
					? Operator<TMath>.Subtract( Start, End )
					: Operator<TMath>.Subtract( End, Start );
			}
		}

		static Interval()
		{
			IsIntegralType = TypeHelper.IsIntegralNumericType<TMath>();			
		}

		/// <summary>
		///   Create a new interval with a specified start and end, both included in the interval.
		/// </summary>
		/// <param name = "start">The start of the interval, included in the interval.</param>
		/// <param name = "end">The end of the interval, included in the interval.</param>
		public Interval( TMath start, TMath end )
			: this( start, true, end, true ) {}

		/// <summary>
		///   Create a new interval with a specified start and end.
		/// </summary>
		/// <param name = "start">The start of the interval.</param>
		/// <param name = "isStartIncluded">Is the value at the start of the interval included in the interval.</param>
		/// <param name = "end">The end of the interval.</param>
		/// <param name = "isEndIncluded">Is the value at the end of the interval included in the interval.</param>
		public Interval( TMath start, bool isStartIncluded, TMath end, bool isEndIncluded )
		{
			Contract.Requires(
				end.CompareTo( start ) != 0 || (end.CompareTo( start ) == 0 && isStartIncluded && isEndIncluded),
				"Invalid interval arguments. e.g. ]0, 0]" );

			Start = start;
			IsStartIncluded = isStartIncluded;
			End = end;
			IsEndIncluded = isEndIncluded;			

			// Check whether the interval is a reverse interval. E.g. [5, 0]
			IsReversed = start.CompareTo(  end ) > 0;					
		}


		#region Get operations.

		/// <summary>
		///   Get the value at a given percentage within the interval, or on it's borders. Rounding to nearest occurs when needed.
		///   TODO: Would it be cleaner not to use a double for percentage, but a generic Percentage type?
		/// </summary>
		/// <param name = "percentage">The percentage in the range of which to return the value.</param>
		/// <returns>The value at the given percentage within the interval.</returns>
		public TMath GetValueAt( double percentage )
		{			
			// Use double math for the calculation, and then cast to the desired type.
			double value = percentage * CastOperator<TMath, double>.Cast( Size );
			double addition = value * (IsReversed ? -1 : 1); // Subtraction is required for a reversed interval.

			// Ensure nearest neighbour rounding for integral types.
			if ( IsIntegralType )
			{
				addition = Math.Round( addition );
			}

			return Operator<TMath>.Add( Start, CastOperator<double, TMath>.Cast( addition ) );
		}

		/// <summary>
		///   Get a percentage how far inside (or outside) the interval a certain value lies.
		/// </summary>
		/// <param name = "position">The position value to get the percentage for.</param>
		/// <returns>The percentage indicating how far inside (or outside) the interval the given value lies.</returns>
		public double GetPercentageFor( TMath position )
		{
			double size = CastOperator<TMath, double>.Cast( Size );

			// When size is zero, return 1.0 when in interval.
			if ( size == 0 )
			{
				return LiesInInterval( position ) ? 1.0 : 0.0;
			}

			var positionRange = new Interval<TMath>( Start, position );
			double percentage = CastOperator<TMath, double>.Cast( positionRange.Size ) / size;

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
		public TMath Map( TMath value, IInterval<TMath> range )
		{
			return Map<TMath>( value, range );
		}

		/// <summary>
		///   Map a value from this range, to a value in another range of another type linearly.
		/// </summary>
		/// <typeparam name = "TRange">The type of the other range.</typeparam>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		public TRange Map<TRange>( TMath value, IInterval<TRange> range )
			where TRange : IComparable<TRange>
		{
			return range.GetValueAt( GetPercentageFor( value ) );
		}

		/// <summary>
		///   Does the given value lie in the interval or not.
		/// </summary>
		/// <param name = "value">The value to check for.</param>
		/// <returns>True when the value lies within the interval, false otherwise.</returns>
		[Pure]
		public bool LiesInInterval( TMath value )
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
		public bool Intersects( IInterval<TMath> interval )
		{
			int rightOfCompare = interval.Start.CompareTo( End );
			int leftOfCompare = interval.End.CompareTo( Start );

			bool liesRightOf = rightOfCompare > 0 || (rightOfCompare == 0 && !(interval.IsStartIncluded && IsEndIncluded));
			bool liesLeftOf = leftOfCompare < 0 || (leftOfCompare == 0 && !(interval.IsEndIncluded && IsStartIncluded));

			return !(liesRightOf || liesLeftOf);
		}

		/// <summary>
		///   Limit a given value to this range. When the value is smaller/bigger than the range, snap it to the range border.
		/// </summary>
		/// <param name = "value">The value to limit.</param>
		/// <returns>The value limited to the range.</returns>
		public TMath Clamp( TMath value )
		{
			TMath smallest = IsReversed ? End : Start;
			TMath biggest = IsReversed ? Start : End;

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
		public IInterval<TMath> Clamp( IInterval<TMath> range )
		{
			var intersection = Intersection( range );
			if ( intersection == null )
			{
				return Empty;
			}

			TMath smallest = IsReversed ? End : Start;
			TMath biggest = IsReversed ? Start : End;
			TMath clampSmallest = range.IsReversed ? range.End : range.Start;
			TMath clampBiggest = range.IsReversed ? range.Start : range.End;
			bool smaller = smallest.CompareTo( clampSmallest ) < 0;
			bool bigger = biggest.CompareTo( clampBiggest ) > 0;
			return new Interval<TMath>(
				smaller ? clampSmallest : smallest,
				smaller ? intersection.IsStartIncluded : IsStartIncluded,
				bigger ? clampBiggest : biggest,
				bigger ? intersection.IsEndIncluded : IsEndIncluded );
		}

		/// <summary>
		///   Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		public void Split( TMath atPoint, SplitOption option, out IInterval<TMath> before, out IInterval<TMath> after )
		{
			Contract.Requires( atPoint.CompareTo( Start ) >= 0 && atPoint.CompareTo( End ) <= 0 );

			// Part before.
			if ( atPoint.CompareTo( Start ) != 0 )
			{
				before = new Interval<TMath>(
					Start, IsStartIncluded,
					atPoint,
					option == SplitOption.Both || option == SplitOption.Left );
			}
			else
			{
				before = null;
			}

			// Part after.
			if ( atPoint.CompareTo( End ) != 0 )
			{
				after = new Interval<TMath>(
					atPoint,
					option == SplitOption.Both || option == SplitOption.Right,
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
		public List<IInterval<TMath>> Subtract( IInterval<TMath> subtract )
		{
			var result = new List<IInterval<TMath>>();

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
						result.Add( new Interval<TMath>( Start, IsStartIncluded, subtract.Start, !subtract.IsStartIncluded ) );
					}
				}

				// Add remaining section at the back.
				if ( endInInterval )
				{
					int endCompare = subtract.End.CompareTo( End );
					if ( endCompare < 0 || (endCompare == 0 && IsEndIncluded && !subtract.IsEndIncluded) )
					{
						result.Add( new Interval<TMath>( subtract.End, !subtract.IsEndIncluded, End, IsEndIncluded ) );
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
		public IInterval<TMath> Intersection( IInterval<TMath> interval )
		{
			if ( !Intersects( interval ) )
			{
				return null;
			}

			int startCompare = Start.CompareTo( interval.Start );
			int endCompare = End.CompareTo( interval.End );

			return new Interval<TMath>(
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
			var interval = obj as Interval<TMath>;

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
			// TODO: Would a custom hash code be useful if this type is made mutable?
			return base.GetHashCode();
		}

		#endregion  // Get operations.


		#region Enumeration

		/// <summary>
		///   Execute an action each step in an interval.
		/// </summary>
		/// <param name = "step">The size of the steps.</param>
		/// <param name = "stepAction">The operation to execute.</param>
		public void EveryStepOf( TMath step, Action<TMath> stepAction )
		{
			foreach ( var i in new IntervalEnumerator<TMath>( this, step ) )
			{
				stepAction( i );
			}
		}

		#endregion  // Enumeration


		#region Modifiers

		/// <summary>
		///   Expand the interval up to the given value (and including) when required.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		public void ExpandTo( TMath value )
		{
			Contract.Ensures( LiesInInterval( value ) );

			ExpandTo( value, true );
		}

		/// <summary>
		///   Expand the interval up to the given value when required.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		public void ExpandTo( TMath value, bool include )
		{
			int startCompare = value.CompareTo( Start );
			int endCompare = value.CompareTo( End );

			if ( startCompare <= 0 || (startCompare == 0 && include) )
			{
				Start = value;
				IsStartIncluded = true;
			}

			if ( endCompare >= 0 || (endCompare == 0 && include) )
			{
				End = value;
				IsEndIncluded = true;
			}
		}

		/// <summary>
		///   Move the interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		public void Move( TMath amount )
		{
			Start = Operator<TMath>.Add( Start, amount );
			End = Operator<TMath>.Add( End, amount );
		}

		/// <summary>
		///   Scale the current interval.
		/// </summary>
		/// <param name="scale">
		///   Percentage to scale the interval up or down.
		///   Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		public void Scale( double scale, double aroundPercentage = 0.5 )
		{
			TMath scaledSize = CastOperator<double, TMath>.Cast( CastOperator<TMath, double>.Cast( Size ) * scale );
			TMath sizeDiff = Operator<TMath>.Subtract( scaledSize, Size ); // > 0 larger, < 0 smaller
			TMath startAddition = CastOperator<double, TMath>.Cast( CastOperator<TMath, double>.Cast( sizeDiff ) * aroundPercentage );
			TMath endSubtraction = Operator<TMath>.Subtract( sizeDiff, startAddition );
			Start = Operator<TMath>.Add( Start, startAddition );
			End = Operator<TMath>.Subtract( End, endSubtraction );
		}

		#endregion  // Modifiers


		public object Clone()
		{
			return new Interval<TMath>( Start, IsStartIncluded, End, IsEndIncluded );
		}

		public override string ToString()
		{
			string output = IsStartIncluded ? "[" : "]";
			output += Start + ", " + End;
			output += IsEndIncluded ? "]" : "[";
			return output;
		}
	}
}