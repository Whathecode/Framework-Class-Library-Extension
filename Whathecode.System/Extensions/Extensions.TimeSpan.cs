using System;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns a new <see cref="TimeSpan" /> object which is rounded down to the specified <paramref name="part" />.
		/// </summary>
		/// <param name="source">The <see cref="TimeSpan" /> to round down.</param>
		/// <param name="part">The part to round down to.</param>
		public static TimeSpan Round( this TimeSpan source, TimeSpanPart part )
		{
			return new TimeSpan(
				part >= TimeSpanPart.Day ? source.Days : 0,
				part >= TimeSpanPart.Hour ? source.Hours : 0,
				part >= TimeSpanPart.Minute ? source.Minutes : 0,
				part >= TimeSpanPart.Second ? source.Seconds : 0,
				part >= TimeSpanPart.Millisecond ? source.Milliseconds : 0 );
		}
	}
}
