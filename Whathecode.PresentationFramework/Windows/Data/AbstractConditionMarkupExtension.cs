using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Markup extension which returns a converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractConditionMarkupExtension : AbstractComparisonMarkupExtension
	{
		/// <summary>
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, with 0-indexed numbers between brackets referring to the bindings of the <see cref = "MultiBinding" />. E.g. '[0] || [1]'.
		/// </summary>
		public string Expression { get; set; }
	}
}
