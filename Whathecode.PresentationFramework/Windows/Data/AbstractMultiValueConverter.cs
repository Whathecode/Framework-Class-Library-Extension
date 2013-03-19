using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Class which can be used as a base to create generic <see cref = "IMultiValueConverter" />'s, assuming all the values used during the conversion are of the same type.
	/// </summary>
	/// <typeparam name = "TFrom">The types to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractMultiValueConverter<TFrom, TTo> : IMultiValueConverter, IMultiValueConverter<TFrom, TTo>
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			return Convert( values.Cast<TFrom>().ToArray() );
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			return ConvertBack( (TTo)value ).Cast<object>().ToArray();
		}


		public abstract TTo Convert( TFrom[] values );
		public abstract TFrom[] ConvertBack( TTo value );
	}


	/// <summary>
	///   Class which can be used as a base to create generic <see cref = "IMultiValueConverter" />'s, assuming all the values used during the conversion are of the same type.
	/// </summary>
	/// <typeparam name = "TFrom">The types to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <typeparam name = "TParam">The type of the converter parameter.</typeparam>
	public abstract class AbstractMultiValueConverter<TFrom, TTo, TParam> : IMultiValueConverter, IMultiValueConverter<TFrom, TTo, TParam>
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			return Convert( values.Cast<TFrom>().ToArray(), (TParam)parameter );
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			return ConvertBack( (TTo)value, (TParam)parameter ).Cast<object>().ToArray();
		}


		public abstract TTo Convert( TFrom[] values, TParam parameter );
		public abstract TFrom[] ConvertBack( TTo value, TParam parameter );
	}
}
