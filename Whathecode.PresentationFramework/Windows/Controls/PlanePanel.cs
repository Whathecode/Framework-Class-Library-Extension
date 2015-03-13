using System;
using System.Windows;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A panel which allows positioning elements on a plane, and showing a specified region of the plane.
	/// </summary>
	public class PlanePanel : AxesPanel<double, double, double, double>
	{
		static PlanePanel()
		{
			Type type = typeof( PlanePanel );
			DefaultStyleKeyProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( type ) );

			// Specify new default values.
			MaximaXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( double.MinValue, double.MaxValue ) ) );
			MaximaYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( double.MinValue, double.MaxValue ) ) );
			VisibleIntervalXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( 0, 100 ) ) );
			VisibleIntervalYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( 0, 100 ) ) );
			MinimumSizeXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MinValue ) );
			MaximumSizeXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MaxValue ) );
			MinimumSizeYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MinValue ) );
			MaximumSizeYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MaxValue ) );
		}


		protected override double ConvertFromIntervalXValue( double value )
		{
			return value;
		}

		protected override double ConvertToIntervalXValue( double value )
		{
			return value;
		}

		protected override double ConvertFromIntervalYValue( double value )
		{
			return value;
		}

		protected override double ConvertToIntervalYValue( double value )
		{
			return value;
		}

		protected override double ConvertFromXSizeValue( double value )
		{
			return value;
		}

		protected override double ConvertFromYSizeValue( double value )
		{
			return value;
		}
	}
}
