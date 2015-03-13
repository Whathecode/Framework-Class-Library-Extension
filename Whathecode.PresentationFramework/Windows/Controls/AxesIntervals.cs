using System;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   Represents intervals along two axes.
	/// </summary>
	/// <typeparam name="TX">The type of the values along the x-axis.</typeparam>
	/// <typeparam name="TXSize">The type of the size between values along the x-axis.</typeparam>
	/// <typeparam name="TY">The type of the values along the y-axis.</typeparam>
	/// <typeparam name="TYSize">The type of the size between values along the y-axis.</typeparam>
	public class AxesIntervals<TX, TXSize, TY, TYSize>
		where TX : IComparable<TX>
		where TXSize : IComparable<TXSize>
		where TY : IComparable<TY>
		where TYSize : IComparable<TYSize>
	{
		public readonly Interval<TX, TXSize> IntervalX;
		public readonly Interval<TY, TYSize> IntervalY;


		public AxesIntervals( Interval<TX, TXSize> intervalX, Interval<TY, TYSize> intervalY )
		{
			IntervalX = intervalX;
			IntervalY = intervalY;
		}
	}
}
