using System;
using System.Collections.Generic;


namespace Whathecode.System
{
	/// <summary>
	///   A helper class to do common <see cref="TimeSpan" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class TimeSpanHelper
	{
		static readonly Dictionary<DateTimePart, Func<double, TimeSpan>> TimeSpanConstructors
			= new Dictionary<DateTimePart, Func<double, TimeSpan>>
		{
			{ DateTimePart.Day, TimeSpan.FromDays },
			{ DateTimePart.Hour, TimeSpan.FromHours },
			{ DateTimePart.Millisecond, TimeSpan.FromMilliseconds },
			{ DateTimePart.Minute, TimeSpan.FromMinutes },
			{ DateTimePart.Second, TimeSpan.FromSeconds }
		};


		/// <summary>
		///   Get the <see cref="TimeSpan" /> constructor which uses a certain amount of time units to initialize the <see cref="TimeSpan" />.
		/// </summary>
		/// <param name="unit">The units to use to initialize the <see cref="TimeSpan" /></param>
		/// <returns>A function which constructs a <see cref="TimeSpan" /> from an amount of time units.</returns>
		public static Func<double, TimeSpan> GetTimeSpanConstructor( DateTimePart unit )
		{
			if ( TimeSpanConstructors.ContainsKey( unit ) )
			{
				return TimeSpanConstructors[ unit ];
			}

			throw new NotSupportedException( "A TimeSpan can't be constructed from " + unit + "." );
		}
	}
}
