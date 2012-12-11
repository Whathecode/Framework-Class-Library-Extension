using System;
using System.Windows;
using Whathecode.System.Extensions;


namespace Whathecode.System.Arithmetic.Interpolation.TypeProvider.Windows
{
	/// <summary>
	///   Allows AbstractInterpolation to interpolate over a list of points.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class PointInterpolationProvider : AbstractTypeInterpolationProvider<Point, double>
	{
		/// <summary>
		///   Create a new provider to allow AbstractInterpolation to interpolate between a list of points.
		/// </summary>
		public PointInterpolationProvider()
			: base( 2 ) {}


		public override double GetDimensionValue( Point value, int dimension )
		{
			switch ( dimension )
			{
				case 0:
					return value.X;
				case 1:
					return value.Y;
				default:
					throw new NotSupportedException( "Unsupported dimension while doing interpolation on type " + typeof( Point ) + "." );
			}
		}

		public override double RelativePosition( Point from, Point to )
		{
			return from.DistanceTo( to );
		}

		public override Point CreateInstance( double position, double[] interpolated )
		{
			return new Point( interpolated[ 0 ], interpolated[ 1 ] );
		}
	}
}