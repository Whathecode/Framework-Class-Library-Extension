using System;
using System.Linq;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic
{
	/// <summary>
	///   Class which allows to calculate the rate of change of a certain value over a certain interval. (E.g. velocity)
	/// </summary>
	/// <typeparam name = "TValue">The type of the value for which the rate of change is calculated.</typeparam>
	/// <typeparam name = "TOver">The type of the dimension of the interval over which the rate of change is calculated.</typeparam>
	/// <author>Steven Jeuris</author>
	public class RateOfChange<TValue, TOver>
		where TOver : IComparable<TOver>
	{
		/// <summary>
		///   The interval over which the rate of change should be calculated.
		/// </summary>
		public TOver Interval { get; private set; }

		readonly TupleList<TValue, TOver> _samples = new TupleList<TValue, TOver>();


		/// <summary>
		///   Create a new instance which allows calculating the rate of change.
		/// </summary>
		/// <param name = "interval">The interval over which the rate of change should be calculated.</param>
		public RateOfChange( TOver interval )
		{
			Interval = interval;
		}


		/// <summary>
		///   Add a new sample of a value at a given position.
		/// </summary>
		/// <param name = "sample">The value.</param>
		/// <param name="at">The position of the value.</param>
		public void AddSample( TValue sample, TOver at )
		{
			if ( _samples.Count > 0 && at.CompareTo( _samples.Last().Item2 ) < 0 )
			{
				throw new ArgumentException( "Samples are required to be added in order.", "at" );
			}

			_samples.Add( sample, at );

			// Clean up samples older than the specified interval.
			// TODO: This LINQ query probably isn't optimized for lists. What's the performance impact?
			if ( _samples.Count > 2 )
			{
				var toRemove = _samples
					.Reverse<Tuple<TValue, TOver>>()
					.Skip( 2 )
					.SkipWhile( s => Operator<TOver>.Subtract( at, s.Item2 ).CompareTo( Interval ) <= 0 )
					.ToList();
				toRemove.ForEach( r => _samples.Remove( r ) );
			}
		}

		/// <summary>
		///   Calculate the rate of change based on the samples currently added.
		/// </summary>
		/// <returns>
		///   The rate of change of the passed values, calculated over the indicated interval.
		///   If no samples are available, the rate of change is considered to be 0.
		/// </returns>
		public TValue GetCurrentRateOfChange()
		{
			if ( _samples.Count == 0 )
			{
				return CastOperator<double, TValue>.Cast( 0 );
			}

			Tuple<TValue, TOver> first = _samples.First();
			Tuple<TValue, TOver> last = _samples.Last();

			double valueDiff = CastOperator<TValue, double>.Cast( Operator<TValue>.Subtract( last.Item1, first.Item1 ) );
			double overDiff = CastOperator<TOver, double>.Cast( Operator<TOver>.Subtract( last.Item2, first.Item2 ) );

			return CastOperator<double, TValue>.Cast( valueDiff / overDiff );
		}
	}
}
