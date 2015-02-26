using System;
using System.Globalization;
using System.Windows.Data;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which converts a given object to a string,
	///   replacing one item in a specified string passed as converter parameter with the string representation of the object.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SingleStringFormatConverter : IValueConverter
	{
		readonly bool _useParameter;
		readonly string _format;


		public SingleStringFormatConverter()
		{
			_useParameter = true;
		}

		public SingleStringFormatConverter( string format )
		{
			_format = format;
		}


		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string format = _useParameter ? parameter as string : _format;

			return format != null
				? String.Format( format, value )
				: value.ToString();
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which converts a given object to a string,
	///   replacing one item in a specified string passed as converter parameter with the string representation of the object.
	/// </summary>
	public class SingleStringFormatConverterExtension : AbstractMarkupExtension
	{
		/// <summary>
		///   A composite format string used to format the bound value.
		/// </summary>
		public string Format { get; set; }


		protected override object ProvideValue( object targetObject, object targetProperty )
		{
			return new SingleStringFormatConverter( Format );
		}
	}
}
