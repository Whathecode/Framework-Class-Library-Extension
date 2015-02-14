using System;
using System.ComponentModel;
using System.Globalization;


namespace Whathecode.System.ComponentModel
{
	/// <summary>
	///   A type converter which redirects its implementation to a type converter added to <see cref="TypeDescriptor" />.
	///   This can be used within environments where converters are loaded in a separate context
	///   which does not take <see cref="TypeDescriptor" /> into account, e.g. XAML.
	///   Converters added to <see cref="TypeDescriptor" /> will still be loaded for types which have this attribute applied.
	/// </summary>
	abstract class RedirectTypeConverter : TypeConverter
	{
		readonly Type _type;
		TypeConverter _converter;


		protected RedirectTypeConverter( Type type )
		{
			_type = type;
		}


		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			InitializeConverter();
			return _converter.CanConvertFrom( context, sourceType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
		{
			InitializeConverter();
			return _converter.ConvertFrom( context, culture, value );
		}

		public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
		{
			InitializeConverter();
			return _converter.CanConvertTo( context, destinationType );
		}

		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
		{
			InitializeConverter();
			return _converter.ConvertTo( context, culture, value, destinationType );
		}

		public void InitializeConverter()
		{
			if ( _converter != null )
			{
				return;
			}

			_converter = TypeDescriptor.GetConverter( _type );
			if ( _converter.GetType() == GetType() )
			{
				string message = string.Format( "Conversion failed. Converter for {0} is missing in TypeDescriptor.", _type );
				throw new InvalidOperationException( message );
			}
		}
	}
}
