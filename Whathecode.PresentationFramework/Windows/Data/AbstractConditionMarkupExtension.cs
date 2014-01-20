using System;
using System.ComponentModel;
using System.Windows.Data;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
	/// <summary>
	///   Markup extension which returns a converter which can evaluate an expression resulting in a boolean and associates a value to each of its resulting states.
	///   The expressions should be formatted according to the NCalc library: http://ncalc.codeplex.com/
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractConditionMarkupExtension : AbstractMarkupExtension
	{
		/// <summary>
		///   The type of the associated values to the boolean result.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		///   The expression representing the condition resulting in a boolean.
		///   It should be formatted according to the NCalc library, with 0-indexed numbers between brackets referring to the bindings of the <see cref = "MultiBinding" />. E.g. '[0] || [1]'.
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
			object ifTrue = IfTrue;
			object ifFalse = IfFalse;

			// Optionally create a generic converter.
			// TODO: Can this behavior be extracted to a base class? Can the Type parameter be somehow determined automatically?
			if ( Type != null )
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter( Type );
				ifTrue = typeConverter.ConvertFrom( IfTrue );
				ifFalse = typeConverter.ConvertFrom( IfFalse );
			}

			object conditionConverter = CreateConditionConverter( Type ?? typeof( object ), Expression );
			if ( !conditionConverter.GetType().IsOfGenericType( typeof( IConditionConverter<> ) ) )
			{
				throw new InvalidImplementationException( String.Format( "CreateConditionConverter() should return a IConditionConverter<> but returns a '{0}'.", conditionConverter.GetType() ) );
			}
			conditionConverter.SetPropertyValue( "IfTrue", ifTrue );
			conditionConverter.SetPropertyValue( "IfFalse", ifFalse );

			return conditionConverter;
		}


		protected abstract object CreateConditionConverter( Type type, string expression );
	}
}
