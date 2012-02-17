using System;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		public enum DateTimePart
		{
			Year = 0,
			Month,
			Day,
			Hour,
			Minute,
			Second,
			Millisecond
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
