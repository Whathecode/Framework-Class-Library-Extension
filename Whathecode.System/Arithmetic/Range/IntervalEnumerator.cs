using Lambda.Generic.Arithmetic;
using Whathecode.System.Collections.Generic;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Enumerator which allows you to walk across values inside an interval.
	/// </summary>
	/// <typeparam name = "TMath">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public class IntervalEnumerator<TMath> : AbstractEnumerator<TMath>
	{
		readonly Interval<TMath> _interval;
		readonly IMath<TMath> _calculator;
		readonly TMath _step;


		/// <summary>
		///   Create a new enumerator which traverses across an interval in specified steps.
		/// </summary>
		/// <param name = "interval">The interval which to traverse.</param>
		/// <param name = "calculator">Calculator which can be used for arithmetic operations on the interval.</param>
		/// <param name = "step">The steps to step forward each time.</param>
		public IntervalEnumerator( Interval<TMath> interval, IMath<TMath> calculator, TMath step )
		{
			_interval = interval;
			_calculator = calculator;
			_step = step;
		}


		protected override TMath GetFirst()
		{
			// When first value doesn't lie in interval, immediately step.
			return _interval.IsStartIncluded ? _interval.Start : _calculator.Add( _interval.Start, _step );
		}

		protected override TMath GetNext( int enumeratedAlready, TMath previous )
		{
			return _calculator.Add( previous, _step );
		}

		protected override bool HasElements()
		{
			bool nextInInterval = _interval.LiesInInterval( _calculator.Add( _interval.Start, _step ) );
			return _interval.IsStartIncluded || nextInInterval;
		}

		protected override bool HasMoreElements( int enumeratedAlready, TMath previous )
		{
			return _interval.LiesInInterval( _calculator.Add( previous, _step ) );
		}

		public override void Dispose()
		{
			// TODO: Nothing to do?
		}
	}
}