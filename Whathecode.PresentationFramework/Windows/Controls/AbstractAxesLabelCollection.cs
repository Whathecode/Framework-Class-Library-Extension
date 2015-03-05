using System;
using System.Collections.ObjectModel;
using System.Windows;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A collection of labels which can be visualized in an <see cref="AxesPanel{TX,TXSize,TY,TYSize}" />.
	///   Extending classes which set attached properties of <see cref="AxesPanel{TX,TXSize,TY,TYSize}" /> need to do so before adding the elements to the collection,
	///   to guarantee expected behavior when using <see cref="OverrideGroup" />.
	/// </summary>
	public abstract class AbstractAxesLabelCollection<TX, TXSize, TY, TYSize> : ObservableCollection<FrameworkElement>
		where TX : IComparable<TX>
		where TY : IComparable<TY>
	{
		/// <summary>
		///   Labels within collections with the same assigned <see cref="OverrideGroup" /> override each other in case they are positioned at the same location.
		///   Labels of collections defined later in XAML take precedence over those defined earlier.
		/// </summary>
		public string OverrideGroup { get; set; }


		/// <summary>
		///   Called whenever either the visible interval has changed, or the size within which it is presented has changed.
		/// </summary>
		/// <param name="visible">The visible interval.</param>
		/// <param name="panelSize">The size within which the intervals are presented.</param>
		public abstract void VisibleIntervalChanged( AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize );

		/// <summary>
		///   Called whenever a label added by the collection was resized.
		///   When you position labels within a collection based on their size, this callback can be used to update their position.
		/// </summary>
		/// <param name="label">The label which was resized.</param>
		/// <param name="visible">The visible interval.</param>
		/// <param name="panelSize">The size within which the intervals are presented.</param>
		public abstract void LabelResized( FrameworkElement label, AxesIntervals<TX, TXSize, TY, TYSize> visible, Size panelSize );
	}
}
