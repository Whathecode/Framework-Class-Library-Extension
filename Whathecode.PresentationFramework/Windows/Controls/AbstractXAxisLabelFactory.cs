using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A factory which facilitates generating recurrent labels which need to be represented on an <see cref="AxesPanel{TX, TXSize, TY, TYSize}"/>
	///   at a certain X position and a fixed Y position.
	/// </summary>
	public abstract class AbstractXAxisLabelFactory<TX, TXSize, TY, TYSize> : AbstractAxesLabelFactory<TX, TXSize, TY, TYSize>
		where TX : IComparable<TX>
		where TXSize : IComparable<TXSize>
		where TY : IComparable<TY>
		where TYSize : IComparable<TYSize>
	{
		/// <summary>
		///   The maximum size along the x-axis of labels provided by this factory.
		/// </summary>
		public TXSize MaximumLabelSize { get; set; }

		/// <summary>
		///   The minimum amount of pixels in between labels before they are hidden.
		/// </summary>
		public double MinimumPixelsBetweenLabels { get; set; }

		/// <summary>
		///   Set to true when <see cref="MinimumPixelsBetweenLabels" /> has been exceeded.
		/// </summary>
		public bool MinimumPixelsExceeded { get; private set; }

		/// <summary>
		///   A fixed Y position at which all labels are placed.
		///   TODO: Instead, perhaps it makes sense to be able to choose not to set the Y position.
		/// </summary>
		public TY FixedY { get; set; }


		protected override Tuple<TXSize, TYSize> GetMaximumLabelSize( AxesIntervals<TX, TXSize, TY, TYSize> visible )
		{
			return new Tuple<TXSize, TYSize>( MaximumLabelSize, default( TYSize ) );
		}

		protected override IEnumerable<Tuple<TX, TY>> GetPositions( AxesIntervals<TX, TXSize, TY, TYSize> intervals, Size panelSize )
		{
			// When not enough pixels in between labels, do not show any labels.
			double pixelsBetween = SizeToPixels( MaximumLabelSize, intervals, panelSize );
			if ( pixelsBetween < MinimumPixelsBetweenLabels )
			{
				MinimumPixelsExceeded = true;
				return new Tuple<TX, TY>[] { };
			}

			MinimumPixelsExceeded = false;
			return GetXValues( intervals ).Select( x => new Tuple<TX, TY>( x, FixedY ) );
		}

		/// <summary>
		///   Converts a given size of the x-axis to the amount of pixels required to display it.
		/// </summary>
		/// <param name="size">The size on the x-axis.</param>
		/// <param name="intervals">The currently visible interval.</param>
		/// <param name="panelSize">The amount of pixels within which the interval is shown.</param>
		/// <returns></returns>
		protected static double SizeToPixels( TXSize size, AxesIntervals<TX, TXSize, TY, TYSize> intervals, Size panelSize )
		{
			double intervalSize = Interval<TX, TXSize>.ConvertSizeToDouble( intervals.IntervalX.Size );
			double requestedSize = Interval<TX, TXSize>.ConvertSizeToDouble( size );
			return panelSize.Width * ( requestedSize / intervalSize );
		}

		/// <summary>
		///   Converts a given amount of pixels to the equivalent size along the x-axis which that amount of pixels represents.
		/// </summary>
		/// <param name="pixels">The amount of pixels for which to know the size along the x-axis.</param>
		/// <param name="intervals">The currently visible interval.</param>
		/// <param name="panelSize">The amount of pixels within which the interval is shown.</param>
		/// <returns></returns>
		protected static TXSize PixelsToSize( double pixels, AxesIntervals<TX, TXSize, TY, TYSize> intervals, Size panelSize )
		{
			double intervalSize = Interval<TX, TXSize>.ConvertSizeToDouble( intervals.IntervalX.Size );
			double size = ( pixels / panelSize.Width ) * intervalSize;
			return Interval<TX, TXSize>.ConvertDoubleToSize( size );
		}

		/// <summary>
		///   Returns all x positions on which to place labels within the specified interval.
		/// </summary>
		/// <param name="intervals">The currently visible interval.</param>
		protected abstract IEnumerable<TX> GetXValues( AxesIntervals<TX, TXSize, TY, TYSize> intervals );
	}
}
