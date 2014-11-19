using System;
using System.Diagnostics.Contracts;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections.Algorithm;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic.Interpolation
{
	/// <summary>
	///   An abstract class for the computation of values between ones that are known using the surrounding values.
	/// </summary>
	/// <typeparam name = "TValue">The type of the values to interpolate between.</typeparam>
	/// <typeparam name = "TMath">The value type to use for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	[ContractClass( typeof( AbstractInterpolationContract<,> ) )]
	public abstract class AbstractInterpolation<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   The list of data between which is interpolated.
		/// </summary>
		public AbstractKeyPointCollection<TValue, TMath> KeyPoints { get; set; }


		/// <summary>
		///   Create a new object to do interpolation, initialized with a given list of data.
		/// </summary>
		/// <param name = "keyPoints">The list of key points to interpolate between.</param>
		protected AbstractInterpolation( AbstractKeyPointCollection<TValue, TMath> keyPoints )
		{
			KeyPoints = keyPoints;
		}


		#region Abstract definitions

		/// <summary>
		///   Interpolate between two values, with the given neighboring indices.
		/// </summary>
		/// <param name = "smallerIndex">The index of the smaller value.</param>
		/// <param name = "biggerIndex">The index of the larger value.</param>
		/// <param name = "position">The position within the data range of the key points.</param>
		/// <param name = "percentage">The percentage in between the two values.</param>
		/// <returns>The interpolated value between the two values.</returns>
		protected abstract TValue Interpolate( int smallerIndex, int biggerIndex, TMath position, double percentage );

		/// <summary>
		///   Get the tangent in between two values, with the given neighbouring indices.
		/// </summary>
		/// <param name = "smallerIndex">The index of the smaller value.</param>
		/// <param name = "biggerIndex">The index of the larger value.</param>
		/// <param name= "position">The position within the data range of the key points.</param>
		/// <param name = "percentage">The percentage in between the two values.</param>
		/// <returns>A value representing the tangent between the two values.</returns>
		protected abstract TValue TangentAt( int smallerIndex, int biggerIndex, TMath position, double percentage );

		#endregion


		/// <summary>
		///   Get interpolated data for a given percentage within the total range of the key points.
		///   TODO: Would it be cleaner not to use a double for percentage, but a generic Percentage type?
		/// </summary>
		/// <param name = "percentage">The percentage in between the first and the last value to get interpolated data for.</param>
		/// <returns>The interpolated data.</returns>
		public TValue Interpolate( double percentage )
		{
			Contract.Requires( KeyPoints.Count > 0 );

			// TODO: Allow extrapolation?
			if ( percentage < 0 )
			{
				percentage = 0;
			}
			else if ( percentage > 1 )
			{
				percentage = 1;
			}

			// Find in between which two keypoints the desired position lies.
			TMath position = KeyPoints.DataRange.GetValueAt( percentage );
			BinarySearchResult<TMath> searchResult = KeyPoints.BinarySearch( position );

			// Return exact value when found, or interpolated when not found.
			TValue result;
			if ( searchResult.IsObjectFound )
			{
				result = KeyPoints[ searchResult.Found.Object ];
			}
			else
			{
				// Use double math to calculate the desired value inside the interval. (percentage)				
				double smallerValue = CastOperator<TMath, double>.Cast( searchResult.NotFound.Smaller );
				double biggerValue = CastOperator<TMath, double>.Cast( searchResult.NotFound.Bigger );

				result = Interpolate(
					KeyPoints.IndexAtPosition( searchResult.NotFound.Smaller ),
					KeyPoints.IndexAtPosition( searchResult.NotFound.Bigger ),
					position,
					new Interval<double>( smallerValue, biggerValue ).GetPercentageFor( CastOperator<TMath, double>.Cast( position ) ) );
			}

			return result;
		}

		/// <summary>
		///   Get interpolated data at a certain position in the range.
		///   TODO: This position is based on the distance measure as provided by the type interpolation provider.
		///         This might only make sense for AbsoluteKeyPointCollection's.
		/// </summary>
		/// <param name = "at">The position of which to get the interpolated data.</param>
		/// <returns>The interpolated data.</returns>
		public TValue ValueAt( TMath at )
		{
			double percentage = KeyPoints.DataRange.GetPercentageFor( at );

			return Interpolate( percentage );
		}

		/// <summary>
		///   Get the tangent at a given percentage within the range of the key points.
		/// </summary>
		/// <param name = "percentage"></param>
		/// <returns>The tangent at a given percentage within the range of the key points.</returns>
		public TValue TangentAt( double percentage )
		{
			// TODO: Allow extrapolation?
			Contract.Requires( percentage >= 0 && percentage <= 1 );
			Contract.Requires( KeyPoints.Count > 0 );

			// Find in between which two keypoints the desired position lies.
			TMath position = KeyPoints.DataRange.GetValueAt( percentage );
			BinarySearchResult<TMath> searchResult = KeyPoints.BinarySearch( position );

			// Use double math to calculate percentage of desired value inside 
			double smallerValue = CastOperator<TMath, double>.Cast( searchResult.NotFound.Smaller );
			double biggerValue = CastOperator<TMath, double>.Cast( searchResult.NotFound.Bigger );

			TValue result = TangentAt(
				KeyPoints.IndexAtPosition( searchResult.NotFound.Smaller ),
				KeyPoints.IndexAtPosition( searchResult.NotFound.Bigger ),
				position,
				new Interval<double>( smallerValue, biggerValue ).GetPercentageFor( CastOperator<TMath, double>.Cast( position ) ) );

			return result;
		}
	}


	[ContractClassFor( typeof( AbstractInterpolation<,> ) )]
	abstract class AbstractInterpolationContract<TValue, TMath> : AbstractInterpolation<TValue, TMath>
		where TMath : IComparable<TMath>
	{
		protected AbstractInterpolationContract( AbstractKeyPointCollection<TValue, TMath> keyPoints )
			: base( keyPoints ) {}


		protected override TValue Interpolate( int smallerIndex, int biggerIndex, TMath position, double percentage )
		{
			Contract.Requires( smallerIndex >= 0 );
			Contract.Requires( smallerIndex + 1 == biggerIndex );
			Contract.Requires( percentage >= 0 && percentage <= 1 );

			return default(TValue);
		}

		protected override TValue TangentAt( int smallerIndex, int biggerIndex, TMath position, double percentage )
		{
			Contract.Requires( smallerIndex >= 0 );
			Contract.Requires( smallerIndex + 1 == biggerIndex );
			Contract.Requires( percentage >= 0 && percentage <= 1 );

			return default(TValue);
		}
	}
}