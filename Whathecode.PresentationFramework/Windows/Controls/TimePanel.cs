using System;
using System.Windows;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A timeline panel on which user controls can be outlined in time.
	/// </summary>
	public class TimePanel : AxesPanel<DateTime, TimeSpan, double, double>
	{
		static TimePanel()
		{
			Type type = typeof( TimePanel );
			DefaultStyleKeyProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( type ) );

			// Specify new default values.
			MaximaXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<DateTime, TimeSpan>( DateTime.MinValue, DateTime.MaxValue ) ) );
			MaximaYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( 0, 100 ) ) );
			DateTime now = DateTime.Now;
			TimeSpan span = TimeSpan.FromHours( 12 );
			VisibleIntervalXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<DateTime, TimeSpan>( now - span, now + span ) ) );
			VisibleIntervalYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( new Interval<double, double>( 0, 100 ) ) );
			MinimumSizeXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( TimeSpan.MinValue ) );
			MaximumSizeXProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( TimeSpan.MaxValue ) );
			MinimumSizeYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MinValue ) );
			MaximumSizeYProperty.OverrideMetadata( type, new FrameworkPropertyMetadata( double.MaxValue ) );

			// Set conversion functions for datetime intervals.
			Interval<DateTime, TimeSpan>.ConvertDoubleToSize = d => new TimeSpan( (long)Math.Round( d ) );
			Interval<DateTime, TimeSpan>.ConvertSizeToDouble = s => s.Ticks;
		}


		protected override double ConvertFromIntervalXValue( DateTime value )
		{
			return value.Ticks;
		}

		protected override DateTime ConvertToIntervalXValue( double value )
		{
			// Prevent bigger values than the maximum.
			long ticks = (long)value;
			if ( ticks > DateTime.MaxValue.Ticks )
			{
				ticks = DateTime.MaxValue.Ticks;
			}

			return new DateTime( ticks );
		}

		protected override double ConvertFromIntervalYValue( double value )
		{
			return value;
		}

		protected override double ConvertToIntervalYValue( double value )
		{
			return value;
		}

		protected override double ConvertFromXSizeValue( TimeSpan value )
		{
			return value.Ticks;
		}

		protected override double ConvertFromYSizeValue( double value )
		{
			return value;
		}
	}
}
