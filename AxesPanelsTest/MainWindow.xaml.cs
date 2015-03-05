using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using Whathecode.System.Windows.Controls;


namespace AxesPanelsTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public ObservableCollection<object> Snowflakes { get; set; }


		public MainWindow()
		{
			Snowflakes = new ObservableCollection<object>();

			InitializeComponent();

			// Add month names to bottom.
			string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			foreach ( string month in months )
			{
				var label = new Label { Content = month, Foreground = Brushes.White, FontSize = 14 };
				label.SetValue( TimePanel.XProperty, DateTime.Parse( month + " 2015" ) );
				label.SetValue( TimePanel.YProperty, Timeline.VisibleIntervalY.End );
				Timeline.Children.Add( label );
			}
		}
	}
}
