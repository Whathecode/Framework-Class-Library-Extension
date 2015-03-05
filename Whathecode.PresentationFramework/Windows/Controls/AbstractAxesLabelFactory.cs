using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Operators;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A factory which facilitates generating recurrent labels which need to be represented on an <see cref="AxesPanel{TX, TXSize, TY, TYSize}"/> at a certain X and Y position.
	/// </summary>
	public abstract class AbstractAxesLabelFactory<TX, TXSize, TY, TYSize> : AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>
		where TX : IComparable<TX>
		where TY : IComparable<TY>
	{
		protected class PositionedElement
		{
			public readonly FrameworkElement Element;
			public readonly Tuple<TX, TY> Position;

			public PositionedElement( FrameworkElement element, Tuple<TX, TY> position )
			{
				Element = element;
				Position = position;
			}
		}


		readonly List<PositionedElement> _visibleLabels = new List<PositionedElement>();
		protected IReadOnlyList<PositionedElement> VisibleLabels
		{
			get {  return _visibleLabels; }
		}

		readonly Stack<FrameworkElement> _availableLabels = new Stack<FrameworkElement>(); 


		public override void VisibleIntervalChanged( AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize )
		{
			// Create extended intervals.
			Interval<TX, TXSize> intervalX = visible.IntervalX;
			Interval<TY, TYSize> intervalY = visible.IntervalY;
			Tuple<TXSize, TYSize> maxLabelSize = GetMaximumLabelSize( visible );
			var additionalX = Operator<TXSize>.Add( maxLabelSize.Item1, maxLabelSize.Item1 );
			var additionalY = Operator<TYSize>.Add( maxLabelSize.Item2, maxLabelSize.Item2 );
			var extendedX = Operator<TXSize>.Add( intervalX.Size, additionalX );
			var extendedY = Operator<TYSize>.Add( intervalY.Size, additionalY );
			double scaleX = Interval<TX, TXSize>.ConvertSizeToDouble( extendedX ) / Interval<TX, TXSize>.ConvertSizeToDouble( intervalX.Size );
			double scaleY = Interval<TY, TYSize>.ConvertSizeToDouble( extendedY ) / Interval<TY, TYSize>.ConvertSizeToDouble( intervalY.Size );
			var extendedIntervals = new AxesIntervals<TX, TXSize, TY, TYSize>( intervalX.Scale( scaleX ), intervalY.Scale( scaleY ) );

			var toPosition = new HashSet<Tuple<TX, TY>>( GetPositions( extendedIntervals, panelSize ) );

			// Free up labels which are no longer visible, and update those already positioned.
			var toRemove = new List<PositionedElement>();
			var toUpdate = new List<PositionedElement>();
			foreach ( var positioned in _visibleLabels )
			{
				if ( toPosition.Contains( positioned.Position ) )
				{
					toUpdate.Add( positioned );
					toPosition.Remove( positioned.Position );
				}
				else
				{
					Remove( positioned.Element );
					_availableLabels.Push( positioned.Element );
					toRemove.Add( positioned );
				}
			}
			toRemove.ForEach( r => _visibleLabels.Remove( r ) );
			toUpdate.ForEach( u => UpdateLabel( u, visible, panelSize ) );

			// Position new labels.
			var toInitialize = new List<PositionedElement>();
			foreach ( var position in toPosition )
			{
				// Create a new label when needed, or retrieve existing one.
				FrameworkElement toPlace;
				if ( _availableLabels.Count == 0 )
				{
					toPlace = CreateLabel();
					toPlace.CacheMode = new BitmapCache();
				}
				else
				{
					toPlace = _availableLabels.Pop();
				}
				toPlace.SetValue( AxesPanel<TX, TXSize, TY, TYSize>.XProperty, position.Item1 );
				toPlace.SetValue( AxesPanel<TX, TXSize, TY, TYSize>.YProperty, position.Item2 );
				Add( toPlace );

				var positioned = new PositionedElement( toPlace, position );
				_visibleLabels.Add( positioned );
				toInitialize.Add( positioned );
			}
			toInitialize.ForEach( i => InitializeLabel( i, visible, panelSize ) );
		}

		/// <summary>
		///   Returns the maximum size a label can have given specified visible intervals.
		/// </summary>
		protected abstract Tuple<TXSize, TYSize> GetMaximumLabelSize( AxesIntervals<TX, TXSize, TY, TYSize> visible );

		/// <summary>
		///   Returns all positions on which to place labels within the specified interval.
		/// </summary>
		protected abstract IEnumerable<Tuple<TX, TY>> GetPositions( AxesIntervals<TX, TXSize, TY, TYSize> intervals, Size panelSize );

		/// <summary>
		///   Create a new label which can be positioned later.
		/// </summary>
		protected abstract FrameworkElement CreateLabel();

		/// <summary>
		///   Initializes a label which has just been repositioned to a certain position.
		/// </summary>
		protected abstract void InitializeLabel( PositionedElement positioned, AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize );

		/// <summary>
		///   Called for already positioned elements which might potentionally need to be updated based on a new visible interval.
		/// </summary>
		protected abstract void UpdateLabel( PositionedElement label, AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize );

		public override void LabelResized( FrameworkElement label, AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize )
		{
			// TODO: Can this be handled locally within this class, rather than relying on a callback from AbstractAxesLabelCollection?
			//       Main problems seems to be 'visible' and 'panelSize' are needed to update.
			var positioned = _visibleLabels.First( v => v.Element == label );
			UpdateLabel( positioned, visible, panelSize );
		}
	}
}
