using System.Diagnostics.Contracts;


namespace Whathecode.System.Arithmetic.Interpolation.TypeProvider
{
    /// <summary>
    ///   An abstract class which provides AbstractInterpolation with the required information to do interpolation for a specific type.
    /// </summary>
    /// <typeparam name = "TValue">The type for which to get interpolation data.</typeparam>
    /// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractTypeInterpolationProvider<TValue, TMath> : AbstractBasicArithmetic<TMath>
    {
        /// <summary>
        ///   The amount of dimensions the type has.
        /// </summary>
        public int AmountOfDimensions { get; private set; }


        /// <summary>
        ///   Create a new interpolation provider with a specified amount of dimensions.
        /// </summary>
        /// <param name = "amountOfDimensions">The amount of dimensions the type to interpolate has.</param>
        protected AbstractTypeInterpolationProvider( int amountOfDimensions )
        {
            Contract.Requires( amountOfDimensions > 0 );

            AmountOfDimensions = amountOfDimensions;
        }


        #region Abstract definitions

        /// <summary>
        ///   Gets the value of a specified dimension of a specified object.
        /// </summary>
        /// <typeparam name = "TValue">The type of the value object.</typeparam>
        /// <param name = "value">The object to get the value from.</param>
        /// <param name = "dimension">The desired dimension to get the value for.</param>
        /// <returns>The value of the specified dimension of the specified object.</returns>
        public abstract TMath GetDimensionValue( TValue value, int dimension );

        /// <summary>
        ///   Gets the relative position from a value compared to another. Negative when behind, positive when ahead.
        /// </summary>
        /// <param name = "from">The value to get a relative distance for.</param>
        /// <param name = "to">The value to compare the distance to.</param>
        /// <returns>
        ///   The relative distance of the specified value, compared to another specified value.
        ///   Negative when behind, positive when ahead.
        /// </returns>
        public abstract TMath RelativePosition( TValue from, TValue to );

        /// <summary>
        ///   Create a new instance based on a given set of values for the dimensions.
        /// </summary>
        /// <param name = "interpolated">All the values for the dimensions.</param>
        /// <returns>An instance of TValue with it's dimensions set to the given values.</returns>
        public abstract TValue CreateInstance( TMath[] interpolated );

        #endregion


        /// <summary>
        ///   Gets all the dimension values of a specified object.
        /// </summary>
        /// <param name = "value">The object to get all dimension values from.</param>
        /// <returns>An array with the values for every dimension.</returns>
        public TMath[] GetDimensionValues( TValue value )
        {
            Contract.Requires( value != null );

            TMath[] values = new TMath[AmountOfDimensions];
            for ( int curDimension = 0; curDimension < AmountOfDimensions; ++curDimension )
            {
                values[ curDimension ] = GetDimensionValue( value, curDimension );
            }

            return values;
        }
    }
}