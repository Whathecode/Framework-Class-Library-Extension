using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Extensions;
using Whathecode.System.Windows.Controls.Internal;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   Defines an area in which you can position child elements at a certain position along two axes.
	/// </summary>
	/// <typeparam name = "TX">The type which determines the X-axis.</typeparam>
	/// <typeparam name = "TXSize">The type used to specify distances in between two values of <see cref="TX" />.</typeparam>
	/// <typeparam name = "TY">The type which determines the Y-axis.</typeparam>
	/// <typeparam name = "TYSize">The type used to specify distances in between two values of <see cref="TY" />.</typeparam>
	abstract public class AxesPanel<TX, TXSize, TY, TYSize> : Panel
		where TX : IComparable<TX>
		where TXSize : IComparable<TXSize>
		where TY : IComparable<TY>
		where TYSize : IComparable<TYSize>
	{
		static readonly Type Type = typeof( AxesPanel<TX, TXSize, TY, TYSize> );
		public static readonly DependencyPropertyFactory<AxesPanelBinding> PropertyFactory 
			= new DependencyPropertyFactory<AxesPanelBinding>( typeof( AxesPanel<TX, TXSize, TY, TYSize> ) );

		/// <summary>
		///   Label factories grouped per override groups, which determine which labels override others.
		/// </summary>
		IEnumerable<IGrouping<string, AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>>> _labelFactoryGroups;

		// ReSharper disable StaticMemberInGenericType
		public static readonly DependencyProperty MaximaXProperty = PropertyFactory[ AxesPanelBinding.MaximaX ];
		public static readonly DependencyProperty MaximaYProperty = PropertyFactory[ AxesPanelBinding.MaximaY ];
		public static readonly DependencyProperty MinimumSizeXProperty = PropertyFactory[ AxesPanelBinding.MinimumSizeX ];
		public static readonly DependencyProperty MaximumSizeXProperty = PropertyFactory[ AxesPanelBinding.MaximumSizeX ];
		public static readonly DependencyProperty MinimumSizeYProperty = PropertyFactory[ AxesPanelBinding.MinimumSizeY ];
		public static readonly DependencyProperty MaximumSizeYProperty = PropertyFactory[ AxesPanelBinding.MaximumSizeY ];
		public static readonly DependencyProperty VisibleIntervalXProperty = PropertyFactory[ AxesPanelBinding.VisibleIntervalX ];
		public static readonly DependencyProperty VisibleIntervalYProperty = PropertyFactory[ AxesPanelBinding.VisibleIntervalY ];
		public static readonly DependencyProperty LabelFactoriesProperty = PropertyFactory[ AxesPanelBinding.LabelFactories ];
		// ReSharper restore StaticMemberInGenericType


		/// <summary>
		///   The maximum range within which all values on the X-axis must lie.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MaximaX, DefaultValueProvider = typeof( EmptyIntervalProvider ) )]
		public Interval<TX, TXSize> MaximaX
		{
			get { return (Interval<TX, TXSize>)PropertyFactory.GetValue( this, AxesPanelBinding.MaximaX ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MaximaX, value ); }
		}

		/// <summary>
		///   The maximum range within which all values on the Y-axis must lie.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MaximaY, DefaultValueProvider = typeof( EmptyIntervalProvider ) )]
		public Interval<TY, TYSize> MaximaY
		{
			get { return (Interval<TY, TYSize>)PropertyFactory.GetValue( this, AxesPanelBinding.MaximaY ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MaximaY, value ); }
		}

		/// <summary>
		///   The minimum size of <see cref="VisibleIntervalX" />.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MinimumSizeX )]
		public TXSize MinimumSizeX
		{
			get { return (TXSize)PropertyFactory.GetValue( this, AxesPanelBinding.MinimumSizeX ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MinimumSizeX, value ); }
		}

		/// <summary>
		///   The maximum size of <see cref="VisibleIntervalX" />.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MaximumSizeX )]
		public TXSize MaximumSizeX
		{
			get { return (TXSize)PropertyFactory.GetValue( this, AxesPanelBinding.MaximumSizeX ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MaximumSizeX, value ); }
		}

		/// <summary>
		///   The minimum size of <see cref="VisibleIntervalY" />.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MinimumSizeY )]
		public TYSize MinimumSizeY
		{
			get { return (TYSize)PropertyFactory.GetValue( this, AxesPanelBinding.MinimumSizeY ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MinimumSizeY, value ); }
		}

		/// <summary>
		///   The maximum size of <see cref="VisibleIntervalY" />.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.MaximumSizeY )]
		public TYSize MaximumSizeY
		{
			get { return (TYSize)PropertyFactory.GetValue( this, AxesPanelBinding.MaximumSizeY ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.MaximumSizeY, value ); }
		}

		/// <summary>
		///   The visible interval along the X-axis.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.VisibleIntervalX, DefaultValueProvider = typeof( EmptyIntervalProvider ), AffectsMeasure = true )]
		[CoercionHandler( typeof( VisibleIntervalCoercion ), Axis.X )]
		public Interval<TX, TXSize> VisibleIntervalX
		{
			get { return (Interval<TX, TXSize>)PropertyFactory.GetValue( this, AxesPanelBinding.VisibleIntervalX ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.VisibleIntervalX, value ); }
		}

		/// <summary>
		///   The visible interval along the Y-axis.
		/// </summary>
		[DependencyProperty( AxesPanelBinding.VisibleIntervalY, DefaultValueProvider = typeof( EmptyIntervalProvider ), AffectsMeasure = true )]
		[CoercionHandler( typeof( VisibleIntervalCoercion ), Axis.Y )]
		public Interval<TY, TYSize> VisibleIntervalY
		{
			get { return (Interval<TY, TYSize>)PropertyFactory.GetValue( this, AxesPanelBinding.VisibleIntervalY ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.VisibleIntervalY, value ); }
		}

		[DependencyProperty( AxesPanelBinding.LabelFactories )]
		public AxesLabelFactories<TX, TXSize, TY, TYSize> LabelFactories
		{
			get { return (AxesLabelFactories<TX, TXSize, TY, TYSize>)PropertyFactory.GetValue( this, AxesPanelBinding.LabelFactories ); }
			set { PropertyFactory.SetValue( this, AxesPanelBinding.LabelFactories, value ); }
		}


		#region Attached properties

		/// <summary>
		///   Identifies the X property which indicates where the element should be positioned on the X-axis.
		/// </summary>
		public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached(
			@"X", typeof( TX ), Type,
			new FrameworkPropertyMetadata( default( TX ), FrameworkPropertyMetadataOptions.AffectsParentArrange ) );
		public static TX GetX( FrameworkElement element )
		{
			return (TX)element.GetValue( XProperty );
		}
		public static void SetX( FrameworkElement element, TX value )
		{
			element.SetValue( XProperty, value );
		}

		/// <summary>
		///   Identifies the Y property which indicates where the element should be positioned on the Y-axis.
		/// </summary>
		public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached(
			@"Y", typeof( TY ), Type,
			new FrameworkPropertyMetadata( default( TY ), FrameworkPropertyMetadataOptions.AffectsParentArrange ) );
		public static TY GetY( FrameworkElement element )
		{
			return (TY)element.GetValue( YProperty );
		}
		public static void SetY( FrameworkElement element, TY value )
		{
			element.SetValue( YProperty, value );
		}

		/// <summary>
		///   Identifies the AlignmentX property which indicates how the element should be aligned along the X-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.RegisterAttached(
			@"AlignmentX", typeof( AxisAlignment ), Type,
			new FrameworkPropertyMetadata( AxisAlignment.AfterValue, FrameworkPropertyMetadataOptions.AffectsParentArrange ) );
		public static AxisAlignment GetAlignmentX( FrameworkElement element )
		{
			return (AxisAlignment)element.GetValue( AlignmentXProperty );
		}
		public static void SetAlignmentX( FrameworkElement element, AxisAlignment value )
		{
			element.SetValue( AlignmentXProperty, value );
		}

		/// <summary>
		///   Identifies the AlignmentY property which indicates how the element should be aligned along the Y-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty AlignmentYProperty = DependencyProperty.RegisterAttached(
			@"AlignmentY", typeof( AxisAlignment ), Type,
			new FrameworkPropertyMetadata( AxisAlignment.AfterValue, FrameworkPropertyMetadataOptions.AffectsParentArrange ) );
		public static AxisAlignment GetAlignmentY( FrameworkElement element )
		{
			return (AxisAlignment)element.GetValue( AlignmentYProperty );
		}
		public static void SetAlignmentY( FrameworkElement element, AxisAlignment value )
		{
			element.SetValue( AlignmentYProperty, value );
		}

		/// <summary>
		///   Identifies the SizeX property which indicates the desired size of the element along the X-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty SizeXProperty = DependencyProperty.RegisterAttached(
			@"SizeX", typeof( TXSize ), Type,
			new FrameworkPropertyMetadata( default( TXSize ), FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
		public static TXSize GetSizeX( FrameworkElement element )
		{
			return (TXSize)element.GetValue( SizeXProperty );
		}
		public static void SetSizeX( FrameworkElement element, TXSize value )
		{
			element.SetValue( SizeXProperty, value );
		}

		/// <summary>
		///   Identifies the SizeY property which indicates the desired size of the element along the Y-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty SizeYProperty = DependencyProperty.RegisterAttached(
			@"SizeY", typeof( TYSize ), Type,
			new FrameworkPropertyMetadata( default( TYSize ), FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
		public static TYSize GetSizeY( FrameworkElement element )
		{
			return (TYSize)element.GetValue( SizeYProperty );
		}
		public static void SetSizeY( FrameworkElement element, TYSize value )
		{
			element.SetValue( SizeYProperty, value );
		}

		/// <summary>
		///   Identifies the IntervalX property which indicates the desired size and position of the element along the X-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty IntervalXProperty = DependencyProperty.RegisterAttached(
			@"IntervalX", typeof( Interval<TX, TXSize> ), Type,
			new FrameworkPropertyMetadata( OnIntervalXChanged ) );
		static void OnIntervalXChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
		{
			var interval = (Interval<TX, TXSize>)args.NewValue;
			element.SetValue( XProperty, interval.Start );
			element.SetValue( SizeXProperty, interval.Size );
		}
		public static Interval<TX, TXSize> GetIntervalX( FrameworkElement element )
		{
			return (Interval<TX, TXSize>)element.GetValue( IntervalXProperty );
		}
		public static void SetIntervalX( FrameworkElement element, Interval<TX, TXSize> value )
		{
			element.SetValue( IntervalXProperty, value );
		}

		/// <summary>
		///   Identifies the IntervalY property which indicates the desired size and position of the element along the Y-axis.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty IntervalYProperty = DependencyProperty.RegisterAttached(
			@"IntervalY", typeof( Interval<TY, TYSize> ), Type,
			new FrameworkPropertyMetadata( OnIntervalYChanged ) );
		static void OnIntervalYChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
		{
			var interval = (Interval<TY, TYSize>)args.NewValue;
			element.SetValue( YProperty, interval.Start );
			element.SetValue( SizeYProperty, interval.Size );
		}
		public static Interval<TY, TYSize> GetIntervalY( FrameworkElement element )
		{
			return (Interval<TY, TYSize>)element.GetValue( IntervalYProperty );
		}
		public static void SetIntervalY( FrameworkElement element, Interval<TY, TYSize> value )
		{
			element.SetValue( IntervalYProperty, value );
		}

		#endregion // Attached properties


		static AxesPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata( Type, new FrameworkPropertyMetadata( Type ) );
		}

		protected AxesPanel()
		{
			SizeChanged += ( sender, args ) => UpdateFactoriesVisibleInterval();
			Unloaded += ( sender, args ) =>
			{
				if ( !IsItemsHost )
				{
					// Remove label factory labels.
					LabelFactories.CollectionChanged -= OnLabelFactoriesChanged;
					foreach ( var factory in LabelFactories )
					{
						factory.CollectionChanged -= OnLabelsChanged;
						factory.ForEach( RemoveLabel );
					}
				}
			};
		}


		protected override Size MeasureOverride( Size availableSize )
		{
			// This panel needs a requested size in order for its children to be able to be positioned.
			if ( double.IsInfinity( availableSize.Width ) || double.IsInfinity( availableSize.Height ) )
			{
				return Size.Empty;
			}

			// Allow children to take up as much room as they want.
			foreach ( UIElement child in Children )
			{
				// Verify desired width and height, and set when needed.
				FrameworkElement element = child as FrameworkElement;
				if ( element != null )
				{
					// Resize in case size is specified.
					ValueSource sizeXSource = DependencyPropertyHelper.GetValueSource( child, SizeXProperty );
					object sizeX = sizeXSource.BaseValueSource == BaseValueSource.Default ? null : child.GetValue( SizeXProperty );
					ValueSource sizeYSource = DependencyPropertyHelper.GetValueSource( child, SizeYProperty );
					object sizeY = sizeYSource.BaseValueSource == BaseValueSource.Default ? null : child.GetValue( SizeYProperty );
					if ( sizeX != null )
					{
						element.Width = IntervalSize( VisibleIntervalX, (TXSize)sizeX, availableSize.Width );
					}
					if ( sizeY != null )
					{
						element.Height = IntervalSize( VisibleIntervalY, (TYSize)sizeY, availableSize.Height );
					}
				}

				// Child elements are given as much space as they want.
				child.Measure( new Size( double.PositiveInfinity, double.PositiveInfinity ) );
			}

			// The idea behind this panel is to display the specified plane area in the available size.
			return availableSize;
		}

		protected override Size ArrangeOverride( Size finalSize )
		{
			// Position children.
			foreach ( UIElement child in Children )
			{
				// Add translate transform.
				var translate = child.RenderTransform as TranslateTransform;
				if ( translate == null )
				{
					translate = new TranslateTransform();
					child.RenderTransform = translate;
				}

				// Get positioning information.
				ValueSource xSource = DependencyPropertyHelper.GetValueSource( child, XProperty );
				object x = xSource.BaseValueSource == BaseValueSource.Default ? null : child.GetValue( XProperty );
				ValueSource ySource = DependencyPropertyHelper.GetValueSource( child, YProperty );
				object y = ySource.BaseValueSource == BaseValueSource.Default ? null : child.GetValue( YProperty );
				AxisAlignment alignmentX = (AxisAlignment)child.GetValue( AlignmentXProperty );
				AxisAlignment alignmentY = (AxisAlignment)child.GetValue( AlignmentYProperty );

				// Position.
				if ( x != null )
				{
					translate.X = PositionInInterval( VisibleIntervalX, finalSize.Width, child.DesiredSize.Width, (TX)x, alignmentX );
				}
				if ( y != null )
				{
					translate.Y = PositionInInterval( VisibleIntervalY, finalSize.Height, child.DesiredSize.Height, (TY)y, alignmentY );
				}

				// Arrange.
				child.Arrange( new Rect( new Point( 0, 0 ), child.DesiredSize ) );
			}

			return finalSize;
		}

		[DependencyPropertyChanged( AxesPanelBinding.VisibleIntervalX )]
		static void OnVisibleIntervalXChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args )
		{
			var panel = (AxesPanel<TX, TXSize, TY, TYSize>)dependencyObject;
			panel.UpdateFactoriesVisibleInterval();
		}
		[DependencyPropertyChanged( AxesPanelBinding.VisibleIntervalY )]
		static void OnVisibleIntervalYChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args )
		{
			var panel = (AxesPanel<TX, TXSize, TY, TYSize>)dependencyObject;
			panel.UpdateFactoriesVisibleInterval();
		}
		void UpdateFactoriesVisibleInterval()
		{
			if ( LabelFactories != null )
			{
				foreach ( var factory in LabelFactories.Reverse() ) // Factories need to be updated in reverse order, since the later ones have precedence.
				{
					factory.VisibleIntervalChanged(
						new AxesIntervals<TX, TXSize, TY, TYSize>( VisibleIntervalX, VisibleIntervalY ),
						new AxesIntervals<TX, TXSize, TY, TYSize>( MaximaX, MaximaY ),
						new Size( ActualWidth, ActualHeight ) );
				}
			}
		}

		double IntervalSize<T, TSize>( Interval<T, TSize> visible, TSize desiredSize, double visiblePixels )
			where T : IComparable<T>
			where TSize : IComparable<TSize>
		{
			double intervalSize = Interval<T, TSize>.ConvertSizeToDouble( visible.Size );
			double desired = Interval<T, TSize>.ConvertSizeToDouble( desiredSize );

			return ( desired / intervalSize ) * visiblePixels;
		}

		double PositionInInterval<T, TSize>( Interval<T, TSize> interval, double panelSize, double elementSize, T value, AxisAlignment alignment )
			where T : IComparable<T>
			where TSize : IComparable<TSize>
		{
			double percentage = interval.GetPercentageFor( value );
			double position = percentage * panelSize;
			switch ( alignment )
			{
				case AxisAlignment.AfterValue:
					return interval.IsReversed ? position - elementSize : position;
				case AxisAlignment.Center:
					return position - ( elementSize / 2 );
				case AxisAlignment.BeforeValue:
					return interval.IsReversed ? position : position - elementSize;
				default:
					throw new NotSupportedException( alignment + " is not supported by the AxesPanel." );
			}
		}

		[DependencyPropertyChanged( AxesPanelBinding.LabelFactories )]
		static void OnLabelFactoriesChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args )
		{
			var panel = (AxesPanel<TX, TXSize, TY, TYSize>)dependencyObject;
			var oldCollection = (AxesLabelFactories<TX, TXSize, TY, TYSize>)args.OldValue;
			var newCollection = (AxesLabelFactories<TX, TXSize, TY, TYSize>)args.NewValue;

			if ( oldCollection != null )
			{
				oldCollection.CollectionChanged -= panel.OnLabelFactoriesChanged;
				foreach ( var factory in oldCollection )
				{
					// ReSharper disable once AccessToForEachVariableInClosure
					factory.ForEach( l => panel.RemoveLabel( l ) );
				}
			}

			newCollection.CollectionChanged += panel.OnLabelFactoriesChanged;
			panel._labelFactoryGroups = newCollection.GroupBy( f => f.OverrideGroup );
			foreach ( var factory in newCollection.Reverse() ) // Factories need to be updated in reverse order, since the later ones have precedence.
			{
				factory.CollectionChanged += panel.OnLabelsChanged;
				// ReSharper disable once AccessToForEachVariableInClosure
				factory.ForEach( l => panel.AddLabel( factory, l ) );
			}
		}
		void OnLabelFactoriesChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			_labelFactoryGroups = LabelFactories.GroupBy( f => f.OverrideGroup );

			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					e.NewItems.Cast<AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>>().ForEach( f =>
					{
						f.CollectionChanged += OnLabelsChanged;
					} );
					break;
				case NotifyCollectionChangedAction.Remove:
					e.OldItems.Cast<AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>>().ForEach( f =>
					{
						f.CollectionChanged -= OnLabelsChanged;
						f.ForEach( RemoveLabel );
					} );
					break;
			}
		}
		void OnLabelsChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			var factory = (AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>)sender;

			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					e.NewItems.Cast<FrameworkElement>().ForEach( l => AddLabel( factory, l ) );
					break;
				case NotifyCollectionChangedAction.Remove:
					e.OldItems.Cast<FrameworkElement>().ForEach( RemoveLabel );
					break;
			}
		}

		readonly Dictionary<FrameworkElement, AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>> _labelAssociations
			= new Dictionary<FrameworkElement, AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>>();
		void AddLabel( AbstractAxesLabelCollection<TX, TXSize, TY, TYSize> hostFactory, FrameworkElement label )
		{
			Func<FrameworkElement, Tuple<TX, TY>> getPosition = l => Tuple.Create( (TX)l.GetValue( XProperty ), (TY)l.GetValue( YProperty ) );

			// Get positions already taken up by precending factories within the same override group.
			HashSet<Tuple<TX, TY>> positioned = new HashSet<Tuple<TX, TY>>();
			List<AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>> precedingFactories = _labelFactoryGroups
				.First( g => g.Key == hostFactory.OverrideGroup )
				.Reverse() // Factories need to be updated in reverse order, since the later ones have precedence.
				.TakeWhile( f => f != hostFactory )
				.ToList();
			if ( hostFactory.OverrideGroup != null )
			{
				foreach ( var factory in precedingFactories )
				{
					foreach ( var taken in factory.Select( getPosition ) )
					{
						positioned.Add( taken );
					}
				}
			}

			// Only add children positioned on locations which have not been populated yet by preceding factories within the same override group.
			Tuple<TX, TY> desiredPosition = getPosition( label );
			if ( !positioned.Contains( desiredPosition ) )
			{
				// Set Z index to position later defined factories more on top.
				var index = LabelFactories.IndexOf( hostFactory );
				label.SetValue( ZIndexProperty, index );

				_labelAssociations[ label ] = hostFactory;
				label.Loaded += LabelOnLoaded;
				label.SizeChanged += LabelOnSizeChanged;
				Children.Add( label );
			}
		}

		void RemoveLabel( FrameworkElement label )
		{
			label.Loaded -= LabelOnLoaded;
			label.SizeChanged -= LabelOnSizeChanged;
			_labelAssociations.Remove( label );
			Children.Remove( label );
		}

		void LabelOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			var label = (FrameworkElement)sender;
			AbstractAxesLabelCollection<TX, TXSize, TY, TYSize> factory = _labelAssociations[ label ];
			factory.LabelResized( label, new AxesIntervals<TX, TXSize, TY, TYSize>( VisibleIntervalX, VisibleIntervalY ), new Size( ActualWidth, ActualHeight ) );
		}

		void LabelOnSizeChanged( object sender, SizeChangedEventArgs sizeChangedEventArgs )
		{
			var label = (FrameworkElement)sender;
			AbstractAxesLabelCollection<TX, TXSize, TY, TYSize> factory = _labelAssociations[ label ];
			factory.LabelResized( label, new AxesIntervals<TX, TXSize, TY, TYSize>( VisibleIntervalX, VisibleIntervalY ), new Size( ActualWidth, ActualHeight ) );
		}

		protected Interval<double> ConvertToInternalIntervalX( Interval<TX, TXSize> interval )
		{
			double start = ConvertFromIntervalXValue( interval.Start );
			double end = ConvertFromIntervalXValue( interval.End );
			return new Interval<double>( start, interval.IsStartIncluded, end, interval.IsEndIncluded );
		}

		protected Interval<TX, TXSize> ConvertToIntervalX( Interval<double> internalInterval )
		{
			TX start = ConvertToIntervalXValue( internalInterval.Start );
			TX end = ConvertToIntervalXValue( internalInterval.End );
			return new Interval<TX, TXSize>( start, internalInterval.IsStartIncluded, end, internalInterval.IsEndIncluded );
		}

		protected Interval<double> ConvertToInternalIntervalY( Interval<TY, TYSize> interval )
		{
			double start = ConvertFromIntervalYValue( interval.Start );
			double end = ConvertFromIntervalYValue( interval.End );
			return new Interval<double>( start, interval.IsStartIncluded, end, interval.IsEndIncluded );
		}

		protected Interval<TY, TYSize> ConvertToIntervalY( Interval<double> internalInterval )
		{
			TY start = ConvertToIntervalYValue( internalInterval.Start );
			TY end = ConvertToIntervalYValue( internalInterval.End );
			return new Interval<TY, TYSize>( start, internalInterval.IsStartIncluded, end, internalInterval.IsEndIncluded );
		}


		protected abstract double ConvertFromIntervalXValue( TX value );
		protected abstract TX ConvertToIntervalXValue( double value );
		protected abstract double ConvertFromIntervalYValue( TY value );
		protected abstract TY ConvertToIntervalYValue( double value );
		protected abstract double ConvertFromXSizeValue( TXSize value );
		protected abstract double ConvertFromYSizeValue( TYSize value );
	}
}
