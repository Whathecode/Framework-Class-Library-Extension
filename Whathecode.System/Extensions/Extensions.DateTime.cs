using System;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns part of the <see cref="DateTime" /> object.
		/// </summary>
		/// <param name="source">The <see cref="DateTime" /> to retrieve a part from.</param>
		/// <param name="part">Which part of the <see cref="DateTime" /> to retrieve.</param>
		public static double GetDateTimePart( this DateTime source, DateTimePart part )
		{
			switch ( part )
			{
				case DateTimePart.Day:
					return source.Day;
				case DateTimePart.Hour:
					return source.Hour;
				case DateTimePart.Millisecond:
					return source.Millisecond;
				case DateTimePart.Minute:
					return source.Minute;
				case DateTimePart.Month:
					return source.Month;
				case DateTimePart.Second:
					return source.Second;
				case DateTimePart.Year:
					return source.Year;
			}

			throw new NotSupportedException();
		}

		/// <summary>
		///   Returns a new <see cref="DateTime" /> object which is rounded down to the specified <paramref name="part" />.
		/// </summary>
		/// <param name="source">The <see cref="DateTime" /> to round down.</param>
		/// <param name="part">The part to round down to.</param>
		public static DateTime Round( this DateTime source, DateTimePart part )
		{
			return new DateTime(
				source.Year,
				part >= DateTimePart.Month ? source.Month : 1,
				part >= DateTimePart.Day ? source.Day : 1,
				part >= DateTimePart.Hour ? source.Hour : 0,
				part >= DateTimePart.Minute ? source.Minute : 0,
				part >= DateTimePart.Second ? source.Second : 0,
				part >= DateTimePart.Millisecond ? source.Millisecond : 0 );
		}

		/// <summary>
		///   Returns a new <see cref="DateTime" /> object which is rounded down to the day indicating the start of the week.
		/// </summary>
		/// <param name="source">The <see cref="DateTime" /> to round down.</param>
		/// <param name="startOfWeek">They day of the week to round down to.</param>
		public static DateTime Round( this DateTime source, DayOfWeek startOfWeek )
		{
			int excessDays = source.DayOfWeek - startOfWeek;
			if ( excessDays < 0 )
			{
				excessDays += 7;
			}

			return source.Date - TimeSpan.FromDays( excessDays );
		}

		/// <summary>
		///   Safely subtract a given timespan from a <see cref="DateTime" />, preventing an <see cref="ArgumentOutOfRangeException" /> from occurring.
		///   When the subtraction results in an invalid <see cref="DateTime" />, the nearest valid <see cref="DateTime" /> is used.
		/// </summary>
		/// <param name="source">The <see cref="DateTime" /> to subtract from.</param>
		/// <param name="time">The amount of time to subtract from the <see cref="DateTime" />.</param>
		public static DateTime SafeSubtract( this DateTime source, TimeSpan time )
		{
			long minTicks = DateTime.MinValue.Ticks;

			return source.Ticks - time.Ticks < minTicks
				? DateTime.MinValue
				: source - time;
		}

		/// <summary>
		///   Safely add a given timespan to a <see cref="DateTime" />, preventing an <see cref="ArgumentOutOfRangeException" /> from occurring.
		///   When the addition results in an invalid <see cref="DateTime" />, the nearest valid <see cref="DateTime" /> is used.
		/// </summary>
		/// <param name="source">The <see cref="DateTime" /> to add to.</param>
		/// <param name="time">The amount of time to add to the <see cref="DateTime" />.</param>
		public static DateTime SafeAdd( this DateTime source, TimeSpan time )
		{
			long maxTicks = DateTime.MaxValue.Ticks;

			return source.Ticks + time.Ticks > maxTicks
				? DateTime.MaxValue
				: source + time;
		}
	}
}
