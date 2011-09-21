using System;
using System.Diagnostics.Contracts;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;


namespace Whathecode.System.Arithmetic.Interpolation
{
	/// <summary>
	///   Interpolation between known values using piecewise cubic curves and a tension parameter for the tangents.
	///   // TODO: Are double calculations inside this class always desired?
	/// </summary>
	/// <typeparam name = "TValue">The type of the values to interpolate between.</typeparam>
	/// <typeparam name = "TMath">The type to use for the calculations.</typeparam>
	public class CardinalSplineInterpolation<TValue, TMath> : AbstractInterpolation<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   Determines the 'length' of the tangents. 1 will yield all zero tangents, 0 yields a Catmull-Rom spline (default).
		/// </summary>
		public double Tension { get; private set; }


		/// <summary>
		///   Create a new object to do cardinal spline interpolation.
		/// </summary>
		/// <param name = "keyPoints">The list of key points to interpolate between.</param>
		public CardinalSplineInterpolation( AbstractKeyPointCollection<TValue, TMath> keyPoints )
			: this( keyPoints, 0.5 ) {}

		/// <summary>
		///   Create a new object to do cardinal spline interpolation.
		/// </summary>
		/// <param name = "keyPoints">The list of key points to interpolate between.</param>
		/// <param name = "tension">Specify a custom tension for the tangents. 1 will yield all zero tangents, 0.5 yields a Catmull-Rom spline (default).</param>
		public CardinalSplineInterpolation( AbstractKeyPointCollection<TValue, TMath> keyPoints, double tension )
			: base( keyPoints )
		{
			Contract.Requires( tension >= 0 && tension <= 1, "The tension should be a value between 0 and 1." );

			Tension = tension;
		}


		protected override TValue Interpolate( int smallerIndex, int biggerIndex, double percentage )
		{
			AbstractTypeInterpolationProvider<TValue, TMath> typeProvider = KeyPoints.TypeProvider;

			// Retrieve values for all dimensions for all 4 control points.
			double[][] values = RetrieveValues( smallerIndex, biggerIndex, typeProvider );

			// Set up hermite base functions.   
			double t = percentage;
			double t2 = Math.Pow( t, 2 );
			double t3 = Math.Pow( t, 3 );
			double baseFunction00 = (2 * t3) - (3 * t2) + 1;
			double baseFunction10 = t3 - (2 * t2) + t;
			double baseFunction01 = (-2 * t3) + (3 * t2);
			double baseFunction11 = t3 - t2;

			// Interpolate
			double tension = 1 - Tension;
			TMath[] interpolated = new TMath[typeProvider.AmountOfDimensions];
			for ( int i = 0; i < typeProvider.AmountOfDimensions; ++i )
			{
				// Calculate tangents.
				double tangentSmaller = tension * (values[ 2 ][ i ] - values[ 0 ][ i ]);
				double tangentBigger = tension * (values[ 3 ][ i ] - values[ 1 ][ i ]);

				// Multiply hermite base functions with the points (and tangents) and sum up.
				double result =
					(baseFunction00 * values[ 1 ][ i ])
						+ (baseFunction01 * values[ 2 ][ i ])
							+ (baseFunction10 * tangentSmaller)
								+ (baseFunction11 * tangentBigger);

				// Sum up all functions.
				interpolated[ i ] = Calculator.ConvertFrom( result );
			}

			return typeProvider.CreateInstance( interpolated );
		}

		protected override TValue TangentAt( int smallerIndex, int biggerIndex, double percentage )
		{
			AbstractTypeInterpolationProvider<TValue, TMath> typeProvider = KeyPoints.TypeProvider;

			// Retrieve values for all dimensions for all 4 control points.
			double[][] values = RetrieveValues( smallerIndex, biggerIndex, typeProvider );

			// Set up hermite base function derivatives.
			double t = percentage;
			double t2 = Math.Pow( t, 2 );
			double baseFunction00 = (6 * t2) - (6 * t);
			double baseFunction10 = (3 * t2) - (4 * t) + 1;
			double baseFunction01 = (6 * t) - (6 * t2);
			double baseFunction11 = (3 * t2) - (2 * t);

			// Get tangent by calculating derivative.
			double tension = 1 - Tension;
			TMath[] tangent = new TMath[typeProvider.AmountOfDimensions];
			for ( int i = 0; i < typeProvider.AmountOfDimensions; ++i )
			{
				// Original: (((2*t^3)-(3*t^2)+1)*b) + (((-2*t^3)+(3*t^2))*c) + ((t^3-(2*t^2)+t)*(n*(c-a))) + ((t^3-t^2)*(n*(d-b)))
				// First derivative: ((3*d+3*c-3*b-3*a)*n-6*c+6*b)*t^2 + ((-2*d-4*c+2*b+4*a)*n+6*c-6*b)*t + (c-a)*n

				// Calculate tangents.
				double tangentSmaller = tension * (values[ 2 ][ i ] - values[ 0 ][ i ]);
				double tangentBigger = tension * (values[ 3 ][ i ] - values[ 1 ][ i ]);

				// Multiply derived hermite base functions with the points (and tangents) and sum up.
				double result =
					(baseFunction00 * values[ 1 ][ i ])
						+ (baseFunction01 * values[ 2 ][ i ])
							+ (baseFunction10 * tangentSmaller)
								+ (baseFunction11 * tangentBigger);

				// Sum up all functions.
				tangent[ i ] = Calculator.ConvertFrom( result );
			}

			return typeProvider.CreateInstance( tangent );
		}

		/// <summary>
		///   Retrieve values for all dimensions for all 4 control points.
		/// </summary>
		/// <returns>
		///   A 2 dimensional array with all the dimension values for all 4 control points.
		///   The first dimension of the array indicates the control point, the second the dimension.
		/// </returns>
		double[][] RetrieveValues( int smallerIndex, int biggerIndex, AbstractTypeInterpolationProvider<TValue, TMath> typeProvider )
		{
			// Retrieve required values.
			TValue p0 = KeyPoints[ smallerIndex != 0 ? smallerIndex - 1 : smallerIndex ];
			TValue p1 = KeyPoints[ smallerIndex ];
			TValue p2 = KeyPoints[ biggerIndex ];
			TValue p3 = KeyPoints[ biggerIndex != KeyPoints.Count - 1 ? biggerIndex + 1 : biggerIndex ];

			// Retrieve required dimension values.
			Converter<TMath, double> toDoubleConverter = from => Calculator.ConvertToDouble( from );
			double[] p0Values = Array.ConvertAll( typeProvider.GetDimensionValues( p0 ), toDoubleConverter );
			double[] p1Values = Array.ConvertAll( typeProvider.GetDimensionValues( p1 ), toDoubleConverter );
			double[] p2Values = Array.ConvertAll( typeProvider.GetDimensionValues( p2 ), toDoubleConverter );
			double[] p3Values = Array.ConvertAll( typeProvider.GetDimensionValues( p3 ), toDoubleConverter );

			return new[] { p0Values, p1Values, p2Values, p3Values };
		}
	}
}