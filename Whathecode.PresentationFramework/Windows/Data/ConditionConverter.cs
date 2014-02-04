using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using NCalc;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class ConditionConverter<TTo> : AbstractMultiValueConverter<object, TTo>, IConditionConverter<TTo>
	{
		/// <summary>
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, with 0-indexed numbers between brackets referring to the bindings of the <see cref = "MultiBinding" />. E.g. '[0] && [1]'.
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
		///   It should be formatted according to the NCalc library, with 0-indexed numbers between brackets referring to the bindings of the <see cref = "MultiBinding" />. E.g. '[0] && [1]'.
		/// </param>
		public ConditionConverter( string expression )
		{
			_expression = new Expression( expression );
		}


		public override TTo Convert( object[] values )
		{
			// Add the parameters to the expression.
			for ( int i = 0; i < values.Length; ++i )
			{
				_expression.Parameters[ i.ToString( CultureInfo.InvariantCulture ) ] = values[ i ];
			}

			return (bool)_expression.Evaluate() ? IfTrue : IfFalse;
		}

		public override object[] ConvertBack( TTo value )
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	[MarkupExtensionReturnType( typeof( IMultiValueConverter ) )]
	public class ConditionConverterExtension : AbstractConditionMarkupExtension
	{
		protected override object CreateConditionConverter( Type type )
		{
			return Activator.CreateInstance( typeof( ConditionConverter<> ).MakeGenericType( type ), Expression );
		}
	}
}
