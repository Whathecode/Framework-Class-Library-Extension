using System;
using System.Linq.Expressions;
using BinaryOperation
	= System.Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, System.Linq.Expressions.BinaryExpression>;


namespace Whathecode.System.Arithmetic.Operators
{
	/// <summary>
	///   Helper class to construct operators for the generic operator classes.
	/// </summary>
	/// <author>Steven Jeuris</author>
	static class OperatorHelper
	{
		/// <summary>
		///   Compile a delegate which performs a binary operation.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="operation">The binary operation to perform.</param>
		public static Func<TArg1, TArg2, TResult> CompileBinaryExpression<TArg1, TArg2, TResult>( BinaryOperation operation )
		{
			ParameterExpression arg1 = Expression.Parameter( typeof( TArg1 ), "arg1" );
			ParameterExpression arg2 = Expression.Parameter( typeof( TArg2 ), "arg2" );

			return Expression.Lambda<Func<TArg1, TArg2, TResult>>( operation( arg1, arg2 ), arg1, arg2 ).Compile();
		}
	}
}
