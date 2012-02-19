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
	}
}
