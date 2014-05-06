using System;
using System.Windows.Data;
using System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which returns a different value depending on whether a bound value equals a given value. Equals() is used for equality comparison.
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class EqualsValueConverter<TTo> : AbstractValueConverter<object, TTo>, IConditionConverter<TTo>
	{
		/// <summary>
		///   The value which the bound value is compared against.
		/// </summary>
		readonly object _value;

		/// <summary>
		///   The value to return in case the condition is true.
		/// </summary>
		public TTo IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the condition is false.
		/// </summary>
		public TTo IfFalse { get; set; }


		public EqualsValueConverter( object value )
		{
			_value = value;
		}


		public override TTo Convert( object value )
		{
			if ( value == null )
			{
				return _value == null ? IfTrue : IfFalse;
			}

			return value.Equals( _value ) ? IfTrue : IfFalse;
		}

		public override object ConvertBack( TTo value )
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	///   Markup extension which returns a converter which can evaluate whether or not the bound value equals a given value.
	/// </summary>
	[MarkupExtensionReturnType( typeof( IValueConverter ) )]
	public class EqualsValueConverterExtension : AbstractComparisonMarkupExtension
	{
		/// <summary>
		///   The value whih the bound value is compared against.
		/// </summary>
		public object Value { get; set; }


		protected override object CreateConditionConverter( Type type )
		{
			return Activator.CreateInstance( typeof( EqualsValueConverter<> ).MakeGenericType( type ), Value );
		}
	}
}
