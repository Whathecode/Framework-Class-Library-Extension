using System;
using System.Windows.Media.Media3D;
using Whathecode.System.Windows.Media.Extensions;


namespace Whathecode.System.Arithmetic.Interpolation.TypeProvider.Windows.Media.Media3D
{
    public class Vector3DInterpolationProvider : AbstractTypeInterpolationProvider<Vector3D, double>
    {
        public Vector3DInterpolationProvider()
            : base( 3 ) {}


        public override double GetDimensionValue( Vector3D value, int dimension )
        {
            switch ( dimension )
            {
                case 0:
                    return value.X;
                case 1:
                    return value.Y;
                case 2:
                    return value.Z;
                default:
                    throw new NotSupportedException( "Unsupported dimension while doing interpolation on type " + typeof( Vector3D ) + "." );
            }
        }

        public override double RelativePosition( Vector3D from, Vector3D to )
        {
            return from.DistanceTo( to );
        }

        public override Vector3D CreateInstance( double[] interpolated )
        {
            return new Vector3D( interpolated[ 0 ], interpolated[ 1 ], interpolated[ 2 ] );
        }
    }
}