using System.Diagnostics.Contracts;
using System.Windows.Media;


namespace Whathecode.System.Windows.Media.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Get a darker color based on a given color.
		/// </summary>
		/// <param name="color">The color to get a darker color for.</param>
		/// <param name="percentage">How much to darken the color. 0 to 1. 0 maintains the color, 1 results in black.</param>
		/// <returns>The darker version of the given color.</returns>
		public static Color Darken( this Color color, double percentage )
		{
			Contract.Requires( percentage >= 0 && percentage <= 1 );

			double darken = 1 - percentage;

			return Color.FromArgb(
				color.A,
				(byte)(color.R * darken),
				(byte)(color.G * darken),
				(byte)(color.B * darken) );
		}
	}
}
