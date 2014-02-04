using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which returns a different value depending whether all objects are equal or not.
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class EqualsConverter<TTo> : AbstractMultiValueConverter<object, TTo>, IConditionConverter<TTo>
	{
		public override TTo Convert( object[] values )
		{
			object toMatch = values[ 0 ];

			// Early out in case null values are included to prevent NullReferenceException's.
			if ( toMatch == null )
			{
				return values.All( v => v == null ) ? IfTrue : IfFalse;
			}

			return values.All( toMatch.Equals ) ? IfTrue : IfFalse;
		}

		public override object[] ConvertBack( TTo value )
		{
			throw new NotImplementedException();
		}

		public TTo IfTrue { get; set; }
		public TTo IfFalse { get; set; }
	}

	/// <summary>
	///   Markup extension which returns a converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	[MarkupExtensionReturnType( typeof( IMultiValueConverter ) )]
	public class EqualsConverterExtension : AbstractComparisonMarkupExtension
	{
		protected override object CreateConditionConverter( Type type )
		{
			return Activator.CreateInstance( typeof( EqualsConverter<> ).MakeGenericType( type ) );
		}
	}
}
