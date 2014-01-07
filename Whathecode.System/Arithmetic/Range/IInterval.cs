using System;
using System.Collections.Generic;
using Whathecode.System.Algorithm;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Class specifying an interval from a value, to a value. Borders may be included or excluded.
	/// </summary>
	/// <typeparam name = "TMath">The type used to specify the interval, and used for the calculations.</typeparam>
	/// <author>Steven Jeuris</author>
	public interface IInterval<TMath> : ICloneable
		where TMath : IComparable<TMath>
	{
		/// <summary>
		///   The start of the interval.
		/// </summary>
		TMath Start { get; }

		/// <summary>
		///   The end of the interval.
		/// </summary>
		TMath End { get; }

		/// <summary>
		///   Is the value at the start of the interval included in the interval.
		/// </summary>
		bool IsStartIncluded { get; }

		/// <summary>
		///   Is the value at the end of the interval included in the interval.
		/// </summary>
		bool IsEndIncluded { get; }

		/// <summary>
		///   Determines whether the start of the interval lies before or after the end of the interval. true when before, false when behind.
		/// </summary>
		bool IsReversed { get; }

		/// <summary>
		///   Get the value in the center of the interval. Rounded to the nearest correct value.
		/// </summary>
		TMath Center { get; }

		/// <summary>
		///   Get the size of the interval.
		/// </summary>
		TMath Size { get; }


		#region Get operations.

		/// <summary>
		///   Get the value at a given percentage within the interval, or on it's borders. Rounding to nearest occurs when needed.
		///   TODO: Would it be cleaner not to use a double for percentage, but a generic Percentage type?
		/// </summary>
		/// <param name = "percentage">The percentage in the range of which to return the value.</param>
		/// <returns>The value at the given percentage within the interval.</returns>
		TMath GetValueAt( double percentage );

		/// <summary>
		///   Get a percentage how far inside (or outside) the interval a certain value lies.
		/// </summary>
		/// <param name = "position">The position value to get the percentage for.</param>
		/// <returns>The percentage indicating how far inside (or outside) the interval the given value lies.</returns>
		double GetPercentageFor( TMath position );

		/// <summary>
		///   Map a value from this range, to a value in another range linearly.
		/// </summary>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		TMath Map( TMath value, IInterval<TMath> range );

		/// <summary>
		///   Map a value from this range, to a value in another range of another type linearly.
		/// </summary>
		/// <typeparam name = "TRange">The type of the other range.</typeparam>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		TRange Map<TRange>( TMath value, IInterval<TRange> range )
			where TRange : IComparable<TRange>;

		/// <summary>
		///   Does the given value lie in the interval or not.
		/// </summary>
		/// <param name = "value">The value to check for.</param>
		/// <returns>True when the value lies within the interval, false otherwise.</returns>
		bool LiesInInterval( TMath value );

		/// <summary>
		///   Does the given interval intersect the other interval.
		/// </summary>
		/// <param name = "interval">The interval to check for intersection.</param>
		/// <returns>True when the intervals intersect, false otherwise.</returns>
		bool Intersects( IInterval<TMath> interval );

		/// <summary>
		///   Limit a given value to this range. When the value is smaller/bigger than the range, snap it to the range border.
		/// </summary>
		/// <param name = "value">The value to limit.</param>
		/// <returns>The value limited to the range.</returns>
		TMath Clamp( TMath value );

		/// <summary>
		///   Limit a given range to this range.
		///   When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range.</returns>
		IInterval<TMath> Clamp( IInterval<TMath> range );

		/// <summary>
		///   Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		void Split( TMath atPoint, SplitOption option, out IInterval<TMath> before, out IInterval<TMath> after );

		/// <summary>
		///   Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		List<IInterval<TMath>> Subtract( IInterval<TMath> subtract );

		/// <summary>
		///   Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		IInterval<TMath> Intersection( IInterval<TMath> interval );

		#endregion // Get operations.


		#region Enumeration.

		/// <summary>
		///   Execute an action each step in an interval.
		/// </summary>
		/// <param name = "step">The size of the steps.</param>
		/// <param name = "stepAction">The operation to execute.</param>
		void EveryStepOf( TMath step, Action<TMath> stepAction );

		#endregion // Enumeration.


		#region Modifiers

		/// <summary>
		///   Expand the interval up to the given value (and including) when required.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		void ExpandTo( TMath value );

		/// <summary>
		///   Expand the interval up to the given value when required.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		void ExpandTo( TMath value, bool include );

		/// <summary>
		///   Move the interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		void Move( TMath amount );

		/// <summary>
		///   Scale the current interval.
		/// </summary>
		/// <param name="scale">
		///   Percentage to scale the interval up or down.
		///   Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		void Scale( double scale, double aroundPercentage = 0.5 );

		#endregion // Modifiers.
	}
}
