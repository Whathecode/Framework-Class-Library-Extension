using System;
using Lambda.Generic.Arithmetic;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;


namespace Whathecode.System.Arithmetic.Interpolation.TypeProvider
{
	/// <summary>
	///   Provides AbstractInterpolation with the required information to interpolate over AbsoluteKeyPoints.
	/// </summary>
	/// <typeparam name = "TKey">The type used for the absolute values to which the added values are linked.</typeparam>
	/// <typeparam name = "TValue">The type of the key points.</typeparam>
	/// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public class AbsoluteKeyPointInterpolationProvider<TKey, TValue, TMath>
		: AbstractTypeInterpolationProvider<AbsoluteKeyPoint<TKey, TValue>, TMath>
		where TKey : new()
	{
		readonly AbstractTypeInterpolationProvider<TValue, TMath> _keyPointInterpolationProvider;
		readonly IMath<TKey> _keyCalculator;


		/// <summary>
		///   Create a new interpolation provider which can interpolate over AbsoluteKeypoints,
		///   using a factory to initialize the calculator for the TKey type.
		/// </summary>
		/// <param name = "keyPointInterpolationProvider">The interpolation provider for the key point type.</param>
		public AbsoluteKeyPointInterpolationProvider( AbstractTypeInterpolationProvider<TValue, TMath> keyPointInterpolationProvider )
			: this( keyPointInterpolationProvider, CalculatorFactory.CreateBasicCalculator<TKey>( CalculatorFactory.CheckedOption.Unchecked ) )
		{
			_keyPointInterpolationProvider = keyPointInterpolationProvider;
		}

		/// <summary>
		///   Create a new interpolation provider which can interpolate over AbsoluteKeyPoints,
		///   using a specified custom calculator to do operations on the TKey type.
		/// </summary>
		/// <param name = "keyPointInterpolationProvider"></param>
		/// <param name = "keyCalculator"></param>
		public AbsoluteKeyPointInterpolationProvider(
			AbstractTypeInterpolationProvider<TValue, TMath> keyPointInterpolationProvider,
			IMath<TKey> keyCalculator )
			: base( keyPointInterpolationProvider.AmountOfDimensions + 1 )
		{
			_keyPointInterpolationProvider = keyPointInterpolationProvider;
			_keyCalculator = keyCalculator;
		}


		public override TMath GetDimensionValue( AbsoluteKeyPoint<TKey, TValue> value, int dimension )
		{
			return dimension == 0
				? KeyToMath( value.Key ) // First dimension is the key.                   
				: _keyPointInterpolationProvider.GetDimensionValue( value.KeyPoint, dimension - 1 ); // The value of the key point.
		}

		public override TMath RelativePosition( AbsoluteKeyPoint<TKey, TValue> from, AbsoluteKeyPoint<TKey, TValue> to )
		{
			return KeyToMath( _keyCalculator.Subtract( from.Key, to.Key ) );
		}

		public override AbsoluteKeyPoint<TKey, TValue> CreateInstance( TMath[] interpolated )
		{
			// Get interpolated key.
			TKey key = _keyCalculator.ConvertFrom( Calculator.ConvertToDouble( interpolated[ 0 ] ) );

			// Get interpolated key point values.
			int keyPointValueCount = interpolated.Length - 1;
			TMath[] keyPointValues = new TMath[keyPointValueCount];
			Array.Copy( interpolated, 1, keyPointValues, 0, keyPointValueCount );

			return new AbsoluteKeyPoint<TKey, TValue>(
				key,
				_keyPointInterpolationProvider.CreateInstance( keyPointValues ) );
		}

		TMath KeyToMath( TKey value )
		{
			return Calculator.ConvertFrom( _keyCalculator.ConvertToDouble( value ) );
		}
	}
}