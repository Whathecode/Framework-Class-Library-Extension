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
		readonly Interval<TMath, TSize> _interval;
		readonly TSize _step;

		readonly bool _isAnchorSet;
		readonly TMath _anchor;


		/// <summary>
		///   Create a new enumerator which traverses across an interval in specified steps.
		/// </summary>
		/// <param name = "interval">The interval which to traverse.</param>
		/// <param name = "step">The steps to step forward each time.</param>
		public IntervalEnumerator( Interval<TMath, TSize> interval, TSize step )
		{
			_interval = interval;
			_step = step;
		}

		public IntervalEnumerator( Interval<TMath, TSize> interval, TSize step, TMath anchorAt )
			: this( interval, step )
		{
			_isAnchorSet = true;
			_anchor = anchorAt;
		}


		protected override TMath GetFirst()
		{
			Interval<TMath, TSize> interval = _interval;

			// When anchor is set, start the interval at the next anchor position.
			if ( _isAnchorSet )
			{
				TSize anchorDiff = Operator<TMath, TSize>.Subtract( _interval.Start, _anchor );
				double stepSize = Interval<TMath, TSize>.ConvertSizeToDouble( _step );
				double diff = Math.Abs( Interval<TMath, TSize>.ConvertSizeToDouble( anchorDiff ) ) % stepSize;
				if ( diff > 0 )
				{
					if ( _anchor.CompareTo( _interval.Start ) < 0 )
					{
						diff = stepSize - diff;
					}
					TSize addition = Interval<TMath, TSize>.ConvertDoubleToSize( diff );
					interval = new Interval<TMath, TSize>(
						Operator<TMath, TSize>.AddSize( _interval.Start, addition ), true,
						_interval.End, _interval.IsEndIncluded );
				}
			}

			// When first value doesn't lie in interval, immediately step.
			return interval.IsStartIncluded ? interval.Start : Operator<TMath, TSize>.AddSize( interval.Start, _step );
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
			if ( Interval<TMath, TSize>.ConvertSizeToDouble( _step ) == 0 && enumeratedAlready == 1 )
			{
				return false;
			}

			return _interval.LiesInInterval( Operator<TMath, TSize>.AddSize( previous, _step ) );
		}

		public override void Dispose()
		{
			// TODO: Nothing to do?
		}
	}
}