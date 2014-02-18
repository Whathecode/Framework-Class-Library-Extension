using System;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Reflection.Extensions;
using BinaryOperation
	= System.Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, System.Linq.Expressions.BinaryExpression>;
using UnaryOperation
	= System.Func<System.Linq.Expressions.Expression, System.Linq.Expressions.UnaryExpression>;


namespace Whathecode.System.Operators
{
	/// <summary>
	///   Allows access to bitwise operations when the type isn't known at compile time (including enum types).
	///   Type inference is used at run time.
	///   The generic <see cref = "BitwiseOperator{T}" /> is more efficient when the type is known.
	/// </summary>
	/// <author>Steven Jeuris</author>
	/// <author>
	///   Steven Jeuris
	///   Based on work by Marc Gravell contributed to the MiscUtils library and the Arithmetic library written by Rüdiger Klaehn.
	/// </author>
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

		/// <summary>
		///   Evaluates bitwise not (~) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Not<T>( T value )
		{
			return BitwiseOperator<T>.Not( value );
		}
	}


	/// <summary>
	///   Allows access to bitwise operations for a generic type, including enum types.
	/// </summary>
	/// <remarks>
	///   Lazy backing fields are used to prevent initializing an operator which isn't supported by the type.
	/// </remarks>
	public static class BitwiseOperator<T> 
	{
		static readonly Lazy<Func<T, T, T>> AndLazy;
		/// <summary>
		///   A delegate to evaluate bitwise and (&amp;) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> And
		{
			get { return AndLazy.Value; }
		}

		static readonly Lazy<Func<T, T, T>> OrLazy;
		/// <summary>
		///   A delegate to evaluate bitwise inclusive or (|) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> Or
		{
			get { return OrLazy.Value; }
		}		

		static readonly Lazy<Func<T, T, T>> ExclusiveOrLazy;
		/// <summary>
		///   A delegate to evaluate bitwise exclusive or (^) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> ExclusiveOr
		{
			get { return ExclusiveOrLazy.Value; }
		}

		static readonly Lazy<Func<T, T>> NotLazy;
		/// <summary>
		///   A delegate to evaluate bitwise not (~) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type T does not provide this operator.
		/// </summary>
		public static Func<T, T> Not
		{
			get { return NotLazy.Value; }
		}


		static BitwiseOperator()
		{
			// Use underlying type for calculations when it is an enum.
			Type type = typeof( T );
			type = type.IsEnum ? type.GetEnumUnderlyingType() : type;

			// Create delegates which will allow to create the operator delegates.
			// This is necessary since enum's need to be supported, and the compile time T can't be used.
			Func<UnaryOperation, Func<T, T>> unaryDelegate = OperatorHelper.CompileUnaryExpression<T, T>;
			MethodInfo unaryMethod = unaryDelegate.Method.GetGenericMethodDefinition( type, type );
			Func<UnaryOperation, Func<T, T>> compileUnary
				= DelegateHelper.CreateDelegate<Func<UnaryOperation, Func<T, T>>>(
					unaryMethod,
					null,
					DelegateHelper.CreateOptions.Downcasting );
			Func<BinaryOperation, Func<T, T, T>> binaryDelegate = OperatorHelper.CompileBinaryExpression<T, T, T>;
			MethodInfo binaryMethod = binaryDelegate.Method.GetGenericMethodDefinition( type, type, type );
			Func<BinaryOperation, Func<T, T, T>> compileBinary
				= DelegateHelper.CreateDelegate<Func<BinaryOperation, Func<T, T, T>>>(
					binaryMethod,
					null,
					DelegateHelper.CreateOptions.Downcasting );

			AndLazy = new Lazy<Func<T, T, T>>( () => compileBinary( Expression.And ) );
			OrLazy = new Lazy<Func<T, T, T>>( () => compileBinary( Expression.Or ) );
			ExclusiveOrLazy = new Lazy<Func<T, T, T>>( () => compileBinary( Expression.ExclusiveOr ) );
			NotLazy = new Lazy<Func<T, T>>( () => compileUnary( Expression.Not ) );
		}
	}
}