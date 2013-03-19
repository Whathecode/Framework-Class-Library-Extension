using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using NCalc;
using Whathecode.System.Windows.Markup;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class ConditionConverter<TTo> : AbstractMultiValueConverter<object, TTo>
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
	public class ConditionConverterExtension : AbstractMarkupExtension
	{
		/// <summary>
		///   The type of the associated values to the boolean result.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, with 0-indexed numbers between brackets referring to the bindings of the <see cref = "MultiBinding" />. E.g. '[0] && [1]'.
		/// </summary>
		public string Expression { get; set; }

		/// <summary>
		///   The value to return in case the boolean is true.
		/// </summary>
		public object IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the boolean is false.
		/// </summary>
		public object IfFalse { get; set; }


		protected override object ProvideValue( object targetObject, object targetProperty )
		{
			// Optionally create a generic converter.
			// TODO: Can this behavior be extracted to a ase class? Can the Type parameter be somehow determined automatically?
			if ( Type != null )
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter( Type );
				object ifTrue = typeConverter.ConvertFrom( IfTrue );
				object ifFalse = typeConverter.ConvertFrom( IfFalse );

				object conditionConverter = Activator.CreateInstance( typeof( ConditionConverter<> ).MakeGenericType( Type ), Expression );
				conditionConverter.SetPropertyValue( "IfTrue", ifTrue );
				conditionConverter.SetPropertyValue( "IfFalse", ifFalse );

				return conditionConverter;
			}

			return new ConditionConverter<object>( Expression )
			{
				IfTrue = IfTrue,
				IfFalse = IfFalse
			};
		}
	}
}
