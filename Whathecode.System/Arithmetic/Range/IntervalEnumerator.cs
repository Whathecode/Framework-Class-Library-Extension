using System;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Operators;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Enumerator which allows you to walk across values inside an interval.
	/// </summary>
	/// <typeparam name = "TMath">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <typeparam name = "TSize">The type used to specify distances in between two values of <see cref="TMath" />.</typeparam>
	/// <author>Steven Jeuris</author>
	public class IntervalEnumerator<TMath, TSize> : AbstractEnumerator<TMath>
		where TMath : IComparable<TMath>
	{
		readonly IInterval<TMath, TSize> _interval;
		readonly TSize _step;


		/// <summary>
		///   Create a new enumerator which traverses across an interval in specified steps.
		/// </summary>
		/// <param name = "interval">The interval which to traverse.</param>
		/// <param name = "step">The steps to step forward each time.</param>
		public IntervalEnumerator( IInterval<TMath, TSize> interval, TSize step )
		{
			_interval = interval;
			_step = step;
		}


		protected override TMath GetFirst()
		{
			// When first value doesn't lie in interval, immediately step.
			return _interval.IsStartIncluded ? _interval.Start : Operator<TMath, TSize>.AddSize( _interval.Start, _step );
		}

		protected override TMath GetNext( int enumeratedAlready, TMath previous )
		{
			return Operator<TMath, TSize>.AddSize( previous, _step );
		}

		protected override bool HasElements()
		{
			bool nextInInterval = _interval.LiesInInterval( Operator<TMath, TSize>.AddSize( _interval.Start, _step ) );
			return _interval.IsStartIncluded || nextInInterval;
		}

		protected override bool HasMoreElements( int enumeratedAlready, TMath previous )
		{
			return _interval.LiesInInterval( Operator<TMath, TSize>.AddSize( previous, _step ) );
		}

		public override void Dispose()
		{
			// TODO: Nothing to do?
		}
	}
}