using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Creates a delegate of a specified type that retrieves a field value of an instance.
		/// </summary>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="field">The field which to create a delegate for.</param>
		/// <param name="instance"></param>
		/// <returns>A delegate which can be used to retrieve the field value of the passed instance.</returns>
		public static Func<TField> CreateGetterDelegate<TField>( this FieldInfo field, object instance )
		{
			var constantInstance = Expression.Constant( instance );
			return Expression.Lambda<Func<TField>>( Expression.Field( constantInstance, field ) ).Compile();
		}

		/// <summary>
		///   Creates a delegate of a specified type that retrieves a field value of an instance passed as parameter.
		/// </summary>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <typeparam name="TField">The field type.</typeparam>
		/// <param name="field">The field which to create a delegate for.</param>
		/// <returns>A delegate which can be used to retrieve a field value of an instance.</returns>
		public static Func<TInstance, TField> CreateOpenInstanceGetterDelegate<TInstance, TField>( this FieldInfo field )
		{
			var instance = Expression.Parameter( field.DeclaringType );
			return Expression.Lambda<Func<TInstance, TField>>( Expression.Field( instance, field ), instance ).Compile();
		}
	}
}
