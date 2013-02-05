using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Whathecode.System.Reflection.Expressions
{
	/// <summary>
	///   An expression from which open instance and closed instance setters for fields and properties can be created.
	/// </summary>
	/// <typeparam name="T">The type of the field or property.</typeparam>
	/// <author>Steven Jeuris</author>
	public class SetterExpression<T>
	{
		static ParameterExpression _value = Expression.Parameter( typeof( T ) );

		readonly ParameterExpression _instance;
		readonly string _memberName;		


		public SetterExpression( FieldInfo field )
		{
			_instance = Expression.Parameter( field.DeclaringType );
			_memberName = field.Name;
		}

		public SetterExpression( PropertyInfo property )
		{
			_instance = Expression.Parameter( property.DeclaringType );
			_memberName = property.Name;
		}


		/// <summary>
		///   Creates a delegate of a specified type that sets the value of this setter on an instance passed as parameter.
		/// </summary>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <returns>A delegate which can be used to set a field the value of this setter an instance.</returns>
		public Action<TInstance, T> OpenInstance<TInstance>()
		{			
			var member = Expression.PropertyOrField( _instance, _memberName );
			return Expression.Lambda<Action<TInstance, T>>( Expression.Assign( member, _value ), _instance, _value ).Compile();
		}

		/// <summary>
		///   Creates a delegate of a specified type that sets the value of this setter on an instance.
		/// </summary>
		/// <param name="instance">The instance on which to set the value.</param>
		/// <returns>A delegate which can be used to set the value of this setter on the passed instance.</returns>
		public Action<T> ClosedOver<TInstance>( TInstance instance )
		{
			var constantInstance = Expression.Constant( instance );
			var member = Expression.PropertyOrField( constantInstance, _memberName );
			return Expression.Lambda<Action<T>>( Expression.Assign( member, _value ), _value ).Compile();
		}

		/// <summary>
		///   The setter which will be created returns the instance after having set the value.
		/// </summary>
		public ReturningSetter Returning()
		{
			return new ReturningSetter( this );
		}

		/// <summary>
		///   An expression from which open instance and closed instance setters for fields and properties can be created which will return the instance after setting the value.
		/// </summary>
		public class ReturningSetter
		{
			readonly ParameterExpression _instance;
			readonly string _memberName;
			readonly ParameterExpression  _value = SetterExpression<T>._value;			


			internal ReturningSetter( SetterExpression<T> setter )
			{
				_instance = setter._instance;
				_memberName = setter._memberName;
			}


			/// <summary>
			///   Creates a delegate of a specified type that sets the value of this setter on an instance passed as parameter and returns the updated instance.
			/// </summary>
			/// <typeparam name="TInstance">The type of the instance.</typeparam>
			/// <returns>A delegate which can be used to set the value of this setter on an instance, after which the instance is returned.</returns>
			public Func<TInstance, T, TInstance> OpenInstance<TInstance>()
			{
				var member = Expression.PropertyOrField( _instance, _memberName );
				return Expression.Lambda<Func<TInstance, T, TInstance>>(
					Expression.Block(
						Expression.Assign( member, _value ),
						_instance
					),
					_instance, _value ).Compile();
			}

			/// <summary>
			///   Creates a delegate of a specified type that sets the value of this setter on an instance.
			/// </summary>
			/// <typeparam name="TInstance">The type of the instance.</typeparam>
			/// <param name="instance">The instance on which to set the value.</param>
			public Func<T, TInstance> ClosedOver<TInstance>( TInstance instance )
			{
				var constantInstance = Expression.Constant( instance );
				var member = Expression.PropertyOrField( constantInstance, _memberName );
				return Expression.Lambda<Func<T, TInstance>>(
					Expression.Block(
						Expression.Assign( member, _value ),
						_instance
					),
					_value ).Compile();
			}
		}
	}
}
