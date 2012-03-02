using System;


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
	}
}
