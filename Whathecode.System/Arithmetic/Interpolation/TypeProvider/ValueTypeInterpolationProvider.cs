namespace Whathecode.System.Arithmetic.Interpolation.TypeProvider
{
    /// <summary>
    ///   Allows AbstractInterpolation to interpolate over a basic value type. E.g. double.
    /// </summary>
    /// <typeparam name="T">The value type to interpolate.</typeparam>
    /// <author>Steven Jeuris</author>
    public class ValueTypeInterpolationProvider<T> : AbstractTypeInterpolationProvider<T, T>
    {
        public ValueTypeInterpolationProvider()
            : base( 1 ) {}  // Value types only have one dimension.

        public override T GetDimensionValue( T value, int dimension )
        {
            return value;   // There is only one dimension, the value itself.
        }

        public override T RelativePosition( T from, T to )
        {
            return Calculator.Subtract( from, to );
        }

        public override T CreateInstance( T[] interpolated )
        {
            return interpolated[ 0 ];
        }
    }
}
