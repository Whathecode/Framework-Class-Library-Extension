using System;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Operators;


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


		/// <summary>
		///   Create a new interpolation provider which can interpolate over AbsoluteKeyPoints.
		/// </summary>
		public AbsoluteKeyPointInterpolationProvider( AbstractTypeInterpolationProvider<TValue, TMath> keyPointInterpolationProvider )
			: base( keyPointInterpolationProvider.AmountOfDimensions + 1 )
		{
			_keyPointInterpolationProvider = keyPointInterpolationProvider;
		}


		public override TMath GetDimensionValue( AbsoluteKeyPoint<TKey, TValue> value, int dimension )
		{
			return dimension == 0
				? KeyToMath( value.Key ) // First dimension is the key.                   
				: _keyPointInterpolationProvider.GetDimensionValue( value.KeyPoint, dimension - 1 ); // The value of the key point.
		}

		public override TMath RelativePosition( AbsoluteKeyPoint<TKey, TValue> from, AbsoluteKeyPoint<TKey, TValue> to )
		{
			return KeyToMath( Operator<TKey>.Subtract( from.Key, to.Key ) );
		}

		public override AbsoluteKeyPoint<TKey, TValue> CreateInstance( TMath[] interpolated )
		{
			// Get interpolated key.
			TKey key = CastOperator<double, TKey>.Cast( CastOperator<TMath, double>.Cast( interpolated[ 0 ] ) );

			// Get interpolated key point values.
			int keyPointValueCount = interpolated.Length - 1;
			TMath[] keyPointValues = new TMath[keyPointValueCount];
			Array.Copy( interpolated, 1, keyPointValues, 0, keyPointValueCount );

			return new AbsoluteKeyPoint<TKey, TValue>(
				key,
				_keyPointInterpolationProvider.CreateInstance( keyPointValues ) );
		}

		static TMath KeyToMath( TKey value )
		{
			return CastOperator<double, TMath>.Cast( CastOperator<TKey, double>.Cast( value ) );
		}
	}
}