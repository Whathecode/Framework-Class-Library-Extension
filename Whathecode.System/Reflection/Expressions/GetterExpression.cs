using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Whathecode.System.Reflection.Expressions
{
	/// <summary>
	///   An expression from which open instance and closed instance getters for fields and properties can be created.
	/// </summary>
	/// <typeparam name="T">The type of the field or property.</typeparam>
	/// <author>Steven Jeuris</author>
	public class GetterExpression<T>
	{
		ParameterExpression _instance;
		string _memberName;


		public GetterExpression( FieldInfo field )
		{
			_instance = Expression.Parameter( field.DeclaringType );
			_memberName = field.Name;
		}

		public GetterExpression( PropertyInfo property )
		{
			_instance = Expression.Parameter( property.DeclaringType );
			_memberName = property.Name;
		}


		/// <summary>
		///   Creates a delegate of a specified type that retrieves the value of this getter of an instance passed as parameter.
		/// </summary>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <returns>A delegate which can be used to retrieve the value of this getter of a passed instance.</returns>
		public Func<TInstance, T> OpenInstance<TInstance>()
		{
			return Expression.Lambda<Func<TInstance, T>>( Expression.PropertyOrField( _instance, _memberName ), _instance ).Compile();
		}

		/// <summary>
		///   Creates a delegate of a specified type that retrieves the value of this getter of an instance.
		/// </summary>
		/// <param name="instance">The instance from which to get the value.</param>
		/// <returns>A delegate which can be used to retrieve the value of this getter of the passed instance.</returns>
		public Func<T> ClosedOver<TInstance>( TInstance instance )
		{
			var constantInstance = Expression.Constant( instance );
			return Expression.Lambda<Func<T>>( Expression.PropertyOrField( constantInstance, _memberName ) ).Compile();
		}
	}
}
