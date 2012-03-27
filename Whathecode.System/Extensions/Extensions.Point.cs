using System;
using System.Windows;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Get the distance to another point.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "to">The point to get the distance to.</param>
		/// <returns>The euclidean distance to the given point.</returns>
		public static double DistanceTo( this Point source, Point to )
		{
			// Pythagoras to get distance.
			return Math.Sqrt( Math.Pow( to.X - source.X, 2 ) + Math.Pow( to.Y - source.Y, 2 ) );
		}
	}
}