using System;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Arithmetic.Interpolation
{
	/// <summary>
	///   Linearly computes values between known values.
	/// </summary>
	/// <typeparam name = "TValue">The type of the values to interpolate between.</typeparam>
	/// <typeparam name = "TMath">The type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public class LinearInterpolation<TValue, TMath> : AbstractInterpolation<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   Create a new object to do linear interpolation.
		/// </summary>
		/// <param name = "keyPoints">The list of key points to interpolate between.</param>
		public LinearInterpolation( AbstractKeyPointCollection<TValue, TMath> keyPoints )
			: base( keyPoints ) {}


		protected override TValue Interpolate( int smallerIndex, int biggerIndex, TMath position, double percentage )
		{
			AbstractTypeInterpolationProvider<TValue, TMath> typeProvider = KeyPoints.TypeProvider;

			// Retrieve required values.
			TValue smaller = KeyPoints[ smallerIndex ];
			TValue bigger = KeyPoints[ biggerIndex ];

			// Retrieve required dimension values.
			TMath[] smallerValues = typeProvider.GetDimensionValues( smaller );
			TMath[] biggerValues = typeProvider.GetDimensionValues( bigger );

			// Linear interpolation.
			var interpolated = new TMath[ typeProvider.AmountOfDimensions ];
			for ( int i = 0; i < typeProvider.AmountOfDimensions; ++i )
			{
				var valueRange = new Interval<TMath>( smallerValues[ i ], biggerValues[ i ] );
				interpolated[ i ] = valueRange.GetValueAt( percentage );
			}

			return typeProvider.CreateInstance( position, interpolated );
		}

		protected override TValue TangentAt( int smallerIndex, int biggerIndex, TMath position, double percentage )
		{
			throw new NotImplementedException();
		}
	}
}