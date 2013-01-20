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
			: base( keyPointInterpolationProvider.AmountOfDimensions )
		{
			_keyPointInterpolationProvider = keyPointInterpolationProvider;
		}


		public override TMath GetDimensionValue( AbsoluteKeyPoint<TKey, TValue> value, int dimension )
		{
			return _keyPointInterpolationProvider.GetDimensionValue( value.KeyPoint, dimension );
		}

		public override TMath RelativePosition( AbsoluteKeyPoint<TKey, TValue> from, AbsoluteKeyPoint<TKey, TValue> to )
		{
			TKey key = Operator<TKey>.Subtract( @from.Key, to.Key );
			return CastOperator<double, TMath>.Cast( CastOperator<TKey, double>.Cast( key ) );
		}

		public override AbsoluteKeyPoint<TKey, TValue> CreateInstance( TMath position, TMath[] interpolated )
		{
			// Get interpolated key.
			TKey key = CastOperator<double, TKey>.Cast( CastOperator<TMath, double>.Cast( position ) );

			return new AbsoluteKeyPoint<TKey, TValue>(
				key,
				_keyPointInterpolationProvider.CreateInstance( position, interpolated ) );
		}
	}
}