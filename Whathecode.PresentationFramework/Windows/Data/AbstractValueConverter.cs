using System;
using System.Globalization;
using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Class which can be used as a base to create generic <see cref = "IValueConverter" />'s.
	/// </summary>
	/// <typeparam name = "TFrom">The type to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractValueConverter<TFrom, TTo> : IValueConverter, IValueConverter<TFrom, TTo>
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return Convert( (TFrom)value );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return ConvertBack( (TTo)value );
		}


		public abstract TTo Convert( TFrom value );
		public abstract TFrom ConvertBack( TTo value );
	}
}
