using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Provides a way to apply custom logic to a binding. This is a generic version of <see cref = "IMultiValueConverter" />.
	///   It assumes all the values to which are bound are of the same type, TFrom.
	///   TODO: Could this belong in ComponentModel?
	/// </summary>
	/// <typeparam name = "TFrom">The type to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	interface IMultiValueConverter<TFrom, TTo>
	{
		TTo Convert( TFrom[] values );

		TFrom[] ConvertBack( TTo value );
	}


	/// <summary>
	///   Provides a way to apply custom logi to a binding. This is a generic version of <see cref = "IMultiValueConverter" />.
	///   It assumes all the values to which are bound are of the same type, TFrom.
	/// </summary>
	/// <typeparam name = "TFrom">The type to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <typeparam name = "TParam">The type of the converter parameter.</typeparam>
	interface IMultiValueConverter<TFrom, TTo, in TParam>
	{
		TTo Convert( TFrom[] values, TParam parameter );

		TFrom[] ConvertBack( TTo value, TParam parameter );
	}
}
