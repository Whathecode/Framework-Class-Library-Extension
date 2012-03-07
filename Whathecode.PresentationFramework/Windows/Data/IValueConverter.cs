using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Provides a way to apply custom logic to a binding. This is the generic version of <see cref = "IValueConverter" />.
	///   TODO: Could this belong in ComponentModel?
	/// </summary>
	/// <typeparam name = "TFrom">The type to convert from.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public interface IValueConverter<TFrom, TTo>
	{
		TTo Convert( TFrom value );

		TFrom ConvertBack( TTo value );
	}
}
