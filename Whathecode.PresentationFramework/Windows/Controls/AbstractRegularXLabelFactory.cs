using System;
using System.Collections.Generic;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A factory which facilitates generating recurrent labels at regular intervals which need to be represented on
	///   an <see cref="AxesPanel{TX, TXSize, TY, TYSize}"/> at a certain X position and a fixed Y position.
	/// </summary>
	public abstract class AbstractRegularXLabelFactory<TX, TXSize, TY, TYSize> : AbstractXAxisLabelFactory<TX, TXSize, TY, TYSize>
		where TX : IComparable<TX>
		where TY : IComparable<TY>
	{
		/// <summary>
		///   An anchor value which should contain a label, and from which other labels are spaced multiples of <see cref="StepSize" /> away from.
		/// </summary>
		public TX Anchor { get; set; }

		TXSize _stepSize;
		/// <summary>
		///   The size along the x-axis between each label.
		/// </summary>
		public TXSize StepSize
		{
			get { return _stepSize; }
			set
			{
				_stepSize = value;
				MaximumLabelSize = _stepSize;
			}
		}


		protected override IEnumerable<TX> GetXValues( AxesIntervals<TX, TXSize, TY, TYSize> intervals )
		{
			return intervals.IntervalX.GetValues( StepSize, Anchor );
		}
	}
}
