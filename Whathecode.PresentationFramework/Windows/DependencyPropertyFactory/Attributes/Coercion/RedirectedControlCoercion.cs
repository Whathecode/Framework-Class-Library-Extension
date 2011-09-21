using System.Collections.Generic;
using Whathecode.System.ComponentModel.Coercion;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion
{
	/// <summary>
	///   Coercion which redirects its coercion to an <see cref = "AbstractCoercion{TContext,TValue}" />
	///   and uses itself as the context for coercion.
	/// </summary>
	/// <typeparam name = "TEnum">The type of the enum values which link to properties.</typeparam>
	/// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
	/// <author>Steven Jeuris</author>
	public class RedirectedControlCoercion<TEnum, TValue> : AbstractControlCoercion<TEnum, TValue>
	{
		readonly AbstractCoercion<RedirectedControlCoercion<TEnum, TValue>, TValue> _coercion;

		public Dictionary<TEnum, object> Values { get; private set; }


		public RedirectedControlCoercion(
			AbstractCoercion<RedirectedControlCoercion<TEnum, TValue>, TValue> coercion,
			TEnum dependentProperties )
			: base( dependentProperties )
		{
			_coercion = coercion;
		}


		protected override TValue Coerce( Dictionary<TEnum, object> dependentValues, TValue value )
		{
			// Copy values to this context before redirecting coercion.
			Values = dependentValues;

			return _coercion.Coerce( this, value );
		}
	}
}