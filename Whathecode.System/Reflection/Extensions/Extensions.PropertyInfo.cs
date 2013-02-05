using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Creates a delegate of a specified type that retrieves a property value of an instance passed as parameter.
		/// </summary>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <typeparam name="TProperty">The property type.</typeparam>
		/// <param name="property">The property which to create a delegate for.</param>
		/// <returns>A delegate which can be used to retrieve a property value of an instance.</returns>
		public static Func<TInstance, TProperty> CreateOpenInstanceGetterDelegate<TInstance, TProperty>( this PropertyInfo property )
		{
			var instance = Expression.Parameter( property.DeclaringType );
			return Expression.Lambda<Func<TInstance, TProperty>>( Expression.Property( instance, property ), instance ).Compile();
		}

		/// <summary>
		///   Creates a delegate of a specified type that sets a property value of an instance passed as parameter and returns the updated instance.
		/// </summary>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <typeparam name="TProperty">The property type.</typeparam>
		/// <param name="property">The property which to create a delegate for.</param>
		/// <returns>A delegate which can be used to set a property value of an instance, after which the instance is returned.</returns>
		public static Func<TInstance, TProperty, TInstance> CreateOpenInstanceReturningSetterDelegate<TInstance, TProperty>( this PropertyInfo property )
		{
			var instance = Expression.Parameter( property.DeclaringType );
			var value = Expression.Parameter( property.PropertyType );
			var propertyExpr = Expression.Property( instance, property );
			return Expression.Lambda<Func<TInstance, TProperty, TInstance>>(
				Expression.Block(
					Expression.Assign( propertyExpr, value ),
					instance
				),
				instance, value ).Compile();
		}
	}
}
