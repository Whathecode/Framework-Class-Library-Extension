using System;
using System.Globalization;
using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which converts a given object to a string,
	///   replacing one item in a specified string passed as converter paramater with the string representation of the object.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SingleStringFormatConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string format = parameter as string;

			return format != null
				? String.Format( format, value )
				: value.ToString();
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
