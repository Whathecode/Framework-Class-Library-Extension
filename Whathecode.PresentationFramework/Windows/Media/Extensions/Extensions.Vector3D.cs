using System;
using System.Windows.Media.Media3D;


namespace Whathecode.System.Windows.Media.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Get the distance to another vector.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "to">The vector to get the distance to.</param>
		/// <returns>The euclidean distance to the given vector.</returns>
		public static double DistanceTo( this Vector3D source, Vector3D to )
		{
			// Pythagoras to get distance.
			return Math.Sqrt( Math.Pow( to.X - source.X, 2 ) + Math.Pow( to.Y + source.Y, 2 ) + Math.Pow( to.Z + source.Z, 2 ) );
		}
	}
}