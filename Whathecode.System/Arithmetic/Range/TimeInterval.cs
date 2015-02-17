using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Whathecode.System.Algorithm;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Class specifying an interval in time. Borders may be included or excluded. This type is immutable.
	/// </summary>
	/// <remarks>
	///   This is a wrapper class which simply redirect calls to a more generic base type.
	/// </remarks>
	/// <author>Steven Jeuris</author>
	[DataContract]
	public class TimeInterval : Interval<DateTime, TimeSpan>
	{
		/// <summary>
		///   Create a new interval with a specified start and end date, both included in the interval.
		/// </summary>
		/// <param name = "start">The start date of the interval, included in the interval.</param>
		/// <param name = "end">The end date of the interval, included in the interval.</param>
		public TimeInterval( DateTime start, DateTime end )
			: this( start, true, end, true ) {}

		/// <summary>
		///   Create a new interval with a specified start and end date.
		/// </summary>
		/// <param name = "start">The start date of the interval.</param>
		/// <param name = "isStartIncluded">Determines whether the start date is included in the interval.</param>
		/// <param name = "end">The end date of the interval.</param>
		/// <param name = "isEndIncluded">Determines whether the end date is included in the interval.</param>
		public TimeInterval( DateTime start, bool isStartIncluded, DateTime end, bool isEndIncluded )
			: base( start, isStartIncluded, end, isEndIncluded )
		{
			ConvertDoubleToSize = d => new TimeSpan( (long)Math.Round( d ) );
			ConvertSizeToDouble = s => s.Ticks;
		}

		/// <summary>
		///   Create a <see cref = "TimeInterval" /> from a compatible interval type.
		/// </summary>
		/// <param name = "interval">The compatible interval type.</param>
		public TimeInterval( Interval<DateTime, TimeSpan> interval )
			: this ( interval.Start, interval.IsStartIncluded, interval.End, interval.IsEndIncluded )
		{
		}


		/// <summary>
		///   Limit a given range to this range.
		///   When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range.</returns>
		public TimeInterval Clamp( TimeInterval range )
		{
			return new TimeInterval( base.Clamp( range ) );
		}

		/// <summary>
		///   Split the interval into two intervals at the given date, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point in time where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the specified date, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the specified date, if any, null otherwise.</param>
		public void Split( DateTime atPoint, SplitOption option, out TimeInterval before, out TimeInterval after )
		{
			Interval<DateTime, TimeSpan> beforeInner;
			Interval<DateTime, TimeSpan> afterInner;
			Split( atPoint, option, out beforeInner, out afterInner );
			before = new TimeInterval( beforeInner );
			after = new TimeInterval( afterInner );
		}

		/// <summary>
		///   Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		public List<TimeInterval> Subtract( TimeInterval subtract )
		{
			List<Interval<DateTime, TimeSpan>> result = base.Subtract( subtract );
			return result.Select( r => new TimeInterval( r ) ).ToList();
		}

		/// <summary>
		///   Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		public TimeInterval Intersection( TimeInterval interval )
		{
			return new TimeInterval( base.Intersection( interval ) );
		}

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given time (and including).
		///   When the time lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "time">The time up to which to expand the interval.</param>
		public new TimeInterval ExpandTo( DateTime time )
		{
			return new TimeInterval( base.ExpandTo( time ) );
		}

		/// <summary>
		///   Returns an expanded interval of the current interval up to the given time.
		///   When the time lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		public new TimeInterval ExpandTo( DateTime value, bool include )
		{
			return new TimeInterval( base.ExpandTo( value, include ) );
		}

		/// <summary>
		///   Returns an interval offsetted from the current interval by a specified <see cref = "TimeSpan" />.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		public new TimeInterval Move( TimeSpan amount )
		{
			return new TimeInterval( base.Move( amount ) );
		}

		/// <summary>
		///   Returns a scaled version of the current interval.
		/// </summary>
		/// <param name="scale">
		///   Percentage to scale the interval up or down.
		///   Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		public new TimeInterval Scale( double scale, double aroundPercentage = 0.5 )
		{
			return new TimeInterval( base.Scale( scale, aroundPercentage ) );
		}

		/// <summary>
		///   Returns a reversed version of the current interval, swapping the start date with the end date.
		/// </summary>
		public new TimeInterval Reverse()
		{
			return new TimeInterval( base.Reverse() );
		}

		public new object Clone()
		{
			return new TimeInterval( Start, IsStartIncluded, End, IsEndIncluded );
		}
	}
}
