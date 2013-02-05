using System;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Reflection.Expressions;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Create an expression from which property getters can be created.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="property">The info about the property.</param>
		public static GetterExpression<TProperty> CreateGetter<TProperty>( this PropertyInfo property )
		{
			return new GetterExpression<TProperty>( property );
		}

		/// <summary>
		///   Create an expression from which property setters can be created.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="property">The info about the property.</param>
		public static SetterExpression<TProperty> CreateSetter<TProperty>( this PropertyInfo property )
		{
			return new SetterExpression<TProperty>( property );
		}
	}
}
