using System;
using System.Windows.Data;
using System.Windows.Markup;
using NCalc;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which can evaluate an expression using one bound value, resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class SingleConditionConverter<TTo> : AbstractValueConverter<object, TTo>, IConditionConverter<TTo>
	{
		/// <summary>
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, using '[0]' to refer to the bound value. E.g. '[0] > 1'.
		/// </summary>
		readonly Expression _expression;

		/// <summary>
		///   The value to return in case the condition is true.
		/// </summary>
		public TTo IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the condition is false.
		/// </summary>
		public TTo IfFalse { get; set; }


		/// <summary>
		///   Create a new converter which evaluates the passed expression.
		/// </summary>
		/// <param name = "expression">
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, using '[0]' to refer to the bound value. E.g. '[0] > 1'.
		/// </param>
		public SingleConditionConverter( string expression )
		{
			_expression = new Expression( expression );
		}


		public override TTo Convert( object value )
		{
			// Add the parameter to the expression.
			_expression.Parameters[ "0" ] = value;

			return (bool)_expression.Evaluate() ? IfTrue : IfFalse;
		}

		public override object ConvertBack( TTo value )
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which can evaluate an expression using one bound value, resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	[MarkupExtensionReturnType( typeof( IValueConverter ) )]
	public class SingleConditionConverterExtension : AbstractConditionMarkupExtension
	{
		protected override object CreateConditionConverter( Type type, string expression )
		{
			return Activator.CreateInstance( typeof( SingleConditionConverter<> ).MakeGenericType( type ), expression );
		}
	}
}
