using System;
using System.Windows.Data;
using System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which returns the opposite boolean value.
	/// </summary>
	public class InverseBooleanConverter : AbstractValueConverter<bool, bool>
	{
		public override bool Convert( bool value )
		{
			return !value;
		}

		public override bool ConvertBack( bool value )
		{
			return !value;
		}
	}

	/// <summary>
	///   Markup extension which returns a converter which returns the opposite of the bound boolean value.
	/// </summary>
	[MarkupExtensionReturnType( typeof( IValueConverter ) )]
	public class InverseBooleanConverterExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			return new InverseBooleanConverter();
		}
	}
}
