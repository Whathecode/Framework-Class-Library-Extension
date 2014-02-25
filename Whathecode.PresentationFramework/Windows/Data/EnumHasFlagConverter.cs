using System;
using System.Windows.Data;
using System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Converter which returns one value or another, depending on whether or not the bound flag enum value contains a certain flag.
	/// </summary>
	/// <typeparam name = "TEnum">The type of the enum.</typeparam>
	/// <typeparam name = "TTo">The type to convert to.</typeparam>
	public class EnumHasFlagConverter<TEnum, TTo> : AbstractValueConverter<object, TTo>, IConditionConverter<TTo>
	{
		readonly Enum _flag;

		/// <summary>
		///   The value to return in case the condition is true.
		/// </summary>
		public TTo IfTrue { get; set; }

		/// <summary>
		///   The value to return in case the condition is false.
		/// </summary>
		public TTo IfFalse { get; set; }


		public EnumHasFlagConverter( TEnum flag )
		{
			_flag = (Enum)(object)flag;
		}


		public override TTo Convert( object value )
		{
			var state = (Enum)value;
			return state.HasFlag( _flag ) ? IfTrue : IfFalse;
		}

		public override object ConvertBack( TTo value )
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	///   Markup extension which returns a converter which can evaluate whether or not the bound flag enum value contains a certain flag.
	/// </summary>
	[MarkupExtensionReturnType( typeof( IValueConverter ) )]
	public class EnumHasFlagConverterExtension : AbstractComparisonMarkupExtension
	{
		/// <summary>
		///   The flag to check for whether it is set.
		/// </summary>
		public object Flag { get; set; }


		protected override object CreateConditionConverter( Type type )
		{
			return Activator.CreateInstance( typeof( EnumHasFlagConverter<,> ).MakeGenericType( Flag.GetType(), type ), Flag );
		}
	}
}
