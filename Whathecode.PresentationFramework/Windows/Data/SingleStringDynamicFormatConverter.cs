using System;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which converts a given object to a string,
	///   replacing one item in a bound format string with the string representation of the object.
	///   The first bound object is the object to format, the second bound value is the format string.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SingleStringDynamicFormatConverter : AbstractMultiValueConverter<object, string>
	{
		public override string Convert( object[] values )
		{
			object toFormat = values[ 0 ];
			string format = values[ 1 ] as string;

			return format != null
				? String.Format( format, toFormat )
				: toFormat.ToString();
		}

		public override object[] ConvertBack( string value )
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which converts a given object to a string,
	///   replacing one item in a bound format string with the string representation of the object.
	///   The first bound object is the object to format, the second bound value is the format string.
	/// </summary>
	public class SingleStringDynamicFormatConverterExtension : AbstractMarkupExtension
	{
		protected override object ProvideValue( object targetObject, object targetProperty )
		{
			return new SingleStringDynamicFormatConverter();
		}
	}
}
