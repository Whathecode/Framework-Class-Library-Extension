using System;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Reflection.Expressions;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Create an expression from which field getters can be created.
		/// </summary>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="field">The info about the field.</param>
		public static GetterExpression<TField> CreateGetter<TField>( this FieldInfo field )
		{
			return new GetterExpression<TField>( field );
		}

		/// <summary>
		///   Create an expression from which field setters can be created.
		/// </summary>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="field">The info about the field.</param>
		public static SetterExpression<TField> CreateSetter<TField>( this FieldInfo field )
		{
			return new SetterExpression<TField>( field );
		}
	}
}
