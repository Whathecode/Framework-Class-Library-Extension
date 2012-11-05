using System;
using Whathecode.System.Operators;


namespace Whathecode.System
{
	/// <summary>
	///   A helper class to do common <see cref = "Math" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class MathHelper
	{
		public static double RadiansToDegrees( double radians )
		{
			return radians * (180 / Math.PI);
		}

		public static double DegreesToRadians( double degrees )
		{
			return degrees * (Math.PI / 180);
		}

		/// <summary>
		///   Round a value to the nearest multiple of another value.
		/// </summary>
		/// <typeparam name = "T">The type of the value.</typeparam>
		/// <param name = "value">The value to round to the nearest multiple.</param>
		/// <param name = "roundToMultiple">The passed value will be rounded to the nearest multiple of this value.</param>
		/// <returns>A multiple of roundToMultiple, nearest to the passed value.</returns>
		public static T RoundToNearest<T>( T value, T roundToMultiple )
		{
			double factor = CastOperator<T, double>.Cast( roundToMultiple );
			double result = Math.Round( CastOperator<T, double>.Cast( value ) / factor ) * factor;
			return CastOperator<double, T>.Cast( result );
		}
	}
}
