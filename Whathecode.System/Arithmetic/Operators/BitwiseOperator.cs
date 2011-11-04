using System;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Reflection.Extensions;
using BinaryOperation
	= System.Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, System.Linq.Expressions.BinaryExpression>;


namespace Whathecode.System.Arithmetic.Operators
{
	/// <summary>
	///   Allows access to bitwise operations when the type isn't known at compile time (including enum types).
	///   Type inference is used at run time.
	///   The generic <see cref = "BitwiseOperator{T}" /> is more efficient when the type is known.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class BitwiseOperator
	{
		/// <summary>
		///   Evaluates bitwise and (&amp;) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T And<T>( T value1, T value2 )
		{
			return BitwiseOperator<T>.And( value1, value2 );
		}

		/// <summary>
		///   Evaluates bitwise inclusive or (|) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Or<T>( T value1, T value2 )
		{
			return BitwiseOperator<T>.Or( value1, value2 );
		}

		/// <summary>
		///   Evaluates bitwise exclusive or (^) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T ExclusiveOr<T>( T value1, T value2 )
		{
			return BitwiseOperator<T>.ExclusiveOr( value1, value2 );
		}
	}


	/// <summary>
	///   Allows access to bitwise operations for a generic type, including enum types.
	/// </summary>
	public static class BitwiseOperator<T> 
	{
		/// <summary>
		///   A delegate to evaluate bitwise and (&amp;) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static readonly Func<T, T, T> And;

		/// <summary>
		///   A delegate to evaluate bitwise inclusive or (|) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static readonly Func<T, T, T> Or;

		/// <summary>
		///   A delegate to evaluate bitwise exclusive or (^) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static readonly Func<T, T, T> ExclusiveOr;


		static BitwiseOperator()
		{
			// Use underlying type for calculations when it is an enum.
			Type type = typeof( T );
			type = type.IsEnum ? type.GetEnumUnderlyingType() : type;

			// Create delegate which will allow to create the operator delegates.
			// This is necessary since enum's need to be supported, and the compile time T can't be used.
			Func<BinaryOperation, Func<T, T, T>> compileDelegate = OperatorHelper.CompileBinaryExpression<T, T, T>;
			MethodInfo compileMethod = compileDelegate.Method.GetGenericMethodDefinition( type, type, type );
			Func<BinaryOperation, Func<T, T, T>> compileBinary
				= DelegateHelper.CreateDelegate<Func<BinaryOperation, Func<T, T, T>>>(
					compileMethod,
					null,
					DelegateHelper.CreateOptions.Downcasting );

			And = compileBinary( Expression.And );
			Or = compileBinary( Expression.Or );
			ExclusiveOr = compileBinary( Expression.ExclusiveOr );
		}
	}
}