namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public interface IConditionConverter<TTo>
	{
		/// <summary>
		///   The value to return in case the condition is true.
		/// </summary>
		TTo IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the condition is false.
		/// </summary>
		TTo IfFalse { get; set; }
	}
}
