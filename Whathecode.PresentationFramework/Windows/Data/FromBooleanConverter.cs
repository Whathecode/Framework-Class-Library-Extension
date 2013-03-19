using System.Windows.Data;
using System.Windows.Markup;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter to convert a boolean to a value associated to each of its two states.
	/// </summary>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	/// <author>Steven Jeuris</author>
	public class FromBooleanConverter<TTo> : AbstractValueConverter<bool, TTo>
	{
		/// <summary>
		///   The value to return in case the boolean is true.
		/// </summary>
		public TTo IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the boolean is false.
		/// </summary>
		public TTo IfFalse { get; set; }


		public override TTo Convert( bool value )
		{
			return value ? IfTrue : IfFalse;
		}

		public override bool ConvertBack( TTo value )
		{
			return IfTrue.Equals( value );
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which converts a boolean to a value associated to each of its two states.
	/// </summary>
	[MarkupExtensionReturnType( typeof( IValueConverter ) )]
	public class FromBooleanConverterExtension : AbstractMarkupExtension
	{
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
			return new FromBooleanConverter<object>
			{
				IfTrue = IfTrue,
				IfFalse = IfFalse
			};
		}
	}
}