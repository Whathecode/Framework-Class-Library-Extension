using System;
using System.Runtime.Serialization;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Class specifying an interval in time. Borders may be included or excluded. This type is immutable.
	/// </summary>
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

		public new object Clone()
		{
			return new TimeInterval( Start, IsStartIncluded, End, IsEndIncluded );
		}
	}
}
