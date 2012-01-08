using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Whathecode.System.Reflection
{
	/// <summary>
	///   Identifies an operator on a type.
	///   TODO: Assignment operators are excluded. Are they useful at any point?
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class Operator
	{		
		protected enum OperatorInner
		{
			Addition,
			Subtraction,
			Multiplication,
			Division,
			Modulo,
			ExclusiveOr,
			BitwiseAnd,
			BitwiseOr,
			LogicalAnd,
			LogicalOr,
			LeftShift,
			RightShift,
			Equality,
			GreaterThan,
			LessThan,
			Inequality,
			GreaterThanOrEqual,
			LessThanOrEqual,
			Decrement,
			Increment,
			Negation,
			OnesComplement
		}


		/// <summary>
		///   All operators which a numerical type is expected to implement.
		/// </summary>
		public static IEnumerable<Operator> NumericalOperators
		{
			get
			{
				yield return BinaryOperator.Addition;
				yield return BinaryOperator.Subtraction;
				yield return BinaryOperator.Multiplication;
				yield return BinaryOperator.Division;
				yield return BinaryOperator.Modulo;
				yield return BinaryOperator.Equality;
				yield return BinaryOperator.GreaterThan;
				yield return BinaryOperator.LessThan;
				yield return BinaryOperator.Inequality;
				yield return BinaryOperator.GreaterThanOrEqual;
				yield return BinaryOperator.LessThanOrEqual;
				yield return UnaryOperator.Decrement;
				yield return UnaryOperator.Increment;
				yield return UnaryOperator.Negation;
			}
		}


		protected OperatorInner OperatorType { get; private set; }


		protected Operator( OperatorInner @operator )
		{
			OperatorType = @operator;
		}
	}


	public class BinaryOperator : Operator
	{
		public static readonly BinaryOperator Addition = new BinaryOperator( OperatorInner.Addition );
		public static readonly BinaryOperator Subtraction = new BinaryOperator( OperatorInner.Subtraction );
		public static readonly BinaryOperator Multiplication = new BinaryOperator( OperatorInner.Multiplication );
		public static readonly BinaryOperator Division = new BinaryOperator( OperatorInner.Division );
		public static readonly BinaryOperator Modulo = new BinaryOperator( OperatorInner.Modulo );
		public static readonly BinaryOperator ExclusiveOr = new BinaryOperator( OperatorInner.ExclusiveOr );
		public static readonly BinaryOperator BitwiseAnd = new BinaryOperator( OperatorInner.BitwiseAnd );
		public static readonly BinaryOperator BitwiseOr = new BinaryOperator( OperatorInner.BitwiseOr );
		public static readonly BinaryOperator LogicalAnd = new BinaryOperator( OperatorInner.LogicalAnd );
		public static readonly BinaryOperator LogicalOr = new BinaryOperator( OperatorInner.LogicalOr );
		public static readonly BinaryOperator LeftShift = new BinaryOperator( OperatorInner.LeftShift );
		public static readonly BinaryOperator RightShift = new BinaryOperator( OperatorInner.RightShift );
		public static readonly BinaryOperator Equality = new BinaryOperator( OperatorInner.Equality );
		public static readonly BinaryOperator GreaterThan = new BinaryOperator( OperatorInner.GreaterThan );
		public static readonly BinaryOperator LessThan = new BinaryOperator( OperatorInner.LessThan );
		public static readonly BinaryOperator Inequality = new BinaryOperator( OperatorInner.Inequality );
		public static readonly BinaryOperator GreaterThanOrEqual = new BinaryOperator( OperatorInner.GreaterThanOrEqual );
		public static readonly BinaryOperator LessThanOrEqual = new BinaryOperator( OperatorInner.LessThanOrEqual );


		BinaryOperator( OperatorInner @operator )
			: base( @operator ) {}


		public Func<Expression, Expression, BinaryExpression> GetExpression()
		{
			switch ( OperatorType )
			{
				case OperatorInner.Addition:
					return Expression.Add;
				case OperatorInner.Subtraction:
					return Expression.Subtract;
				case OperatorInner.Multiplication:
					return Expression.Multiply;
				case OperatorInner.Division:
					return Expression.Divide;
				case OperatorInner.Modulo:
					return Expression.Modulo;
				case OperatorInner.ExclusiveOr:
					return Expression.ExclusiveOr;
				case OperatorInner.BitwiseAnd:
					return Expression.And;
				case OperatorInner.BitwiseOr:
					return Expression.Or;
				case OperatorInner.LogicalAnd:
					return Expression.AndAlso;
				case OperatorInner.LogicalOr:
					return Expression.OrElse;
				case OperatorInner.LeftShift:
					return Expression.LeftShift;
				case OperatorInner.RightShift:
					return Expression.RightShift;
				case OperatorInner.Equality:
					return Expression.Equal;
				case OperatorInner.GreaterThan:
					return Expression.GreaterThan;
				case OperatorInner.LessThan:
					return Expression.LessThan;
				case OperatorInner.Inequality:
					return Expression.NotEqual;
				case OperatorInner.GreaterThanOrEqual:
					return Expression.GreaterThanOrEqual;
				case OperatorInner.LessThanOrEqual:
					return Expression.LessThanOrEqual;
				default:
					throw new NotSupportedException( String.Format( "No expression supported for operator \"{0}\"", OperatorType ) );
			}
		}
	}


	public class UnaryOperator : Operator
	{
		public static readonly UnaryOperator Decrement = new UnaryOperator( OperatorInner.Decrement );
		public static readonly UnaryOperator Increment = new UnaryOperator( OperatorInner.Increment );
		public static readonly UnaryOperator Negation = new UnaryOperator( OperatorInner.Negation );
		public static readonly UnaryOperator OnesComplement = new UnaryOperator( OperatorInner.OnesComplement );


		UnaryOperator( OperatorInner @operator )
			: base( @operator ) {}


		public Func<Expression, UnaryExpression> GetExpression()
		{
			switch ( OperatorType )
			{
				case OperatorInner.Decrement:
					return Expression.Decrement;
				case OperatorInner.Increment:
					return Expression.Increment;
				case OperatorInner.Negation:
					return Expression.Negate;
				case OperatorInner.OnesComplement:
					return Expression.OnesComplement;
				default:
					throw new NotSupportedException( String.Format( "No expression supported for operator \"{0}\"", OperatorType ) );
			}
		}
	}
}
