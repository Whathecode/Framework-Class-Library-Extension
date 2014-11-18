using System;
using System.Linq.Expressions;


namespace Whathecode.System.Operators
{
	/// <summary>
	///   Allows access to standard operators (e.g. addition) when the type isn't known at compile time.
	///   Type inference is used at run time.
	///   The generic <see cref = "Operator{T}" /> is more efficient when the type is known.
	/// </summary>
	/// <author>Steven Jeuris</author>
	/// <author>Based on work by Marc Gravell contributed to the MiscUtils library.</author>
	public static class Operator
	{
		#region Field operators.

		/// <summary>
		///   Evaluates binary addition (+) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Add<T>( T value1, T value2 )
		{
			return Operator<T>.Add( value1, value2 );
		}

		/// <summary>
		///   Evaluates binary subtraction (-) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Subtract<T>( T value1, T value2 )
		{
			return Operator<T>.Subtract( value1, value2 );
		}

		/// <summary>
		///   Evaluates binary multiplication (*) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Multiply<T>( T value1, T value2 )
		{
			return Operator<T>.Multiply( value1, value2 );
		}

		/// <summary>
		///   Evaluates binary division (/) for the given type.
		/// </summary>
		/// <exception cref = "InvalidOperationException">The generic type does not provide this operator.</exception>
		public static T Divide<T>( T value1, T value2 )
		{
			return Operator<T>.Divide( value1, value2 );
		}

		#endregion // Field operators.


		#region // Bitwise operators.

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

		#endregion	// Bitwise operators.
	}

	/// <summary>
	///   Allows access to standard operators (e.g. addition) for a generic type.
	/// </summary>
	/// <remarks>
	///   Lazy backing fields are used to prevent initializing an operator which isn't supported by the type.
	/// </remarks>
	public static class Operator<T>
	{
		#region Field operators.

		static readonly Lazy<Func<T, T, T>> AddLazy;
		/// <summary>
		///   A delegate to evaluate binary addition (+) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> Add
		{
			get { return AddLazy.Value; }
		}

		static readonly Lazy<Func<T, T, T>> SubtractLazy;
		/// <summary>
		///   A delegate to evaluate binary subtraction (-) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> Subtract
		{
			get { return SubtractLazy.Value; }
		}

		static readonly Lazy<Func<T, T, T>> MultiplyLazy;
		/// <summary>
		///   A delegate to evaluate binary multiplication (*) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> Multiply
		{
			get { return MultiplyLazy.Value; }
		}

		static readonly Lazy<Func<T, T, T>> DivideLazy;
		/// <summary>
		///   A delegate to evaluate binary division (/) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, T> Divide
		{
			get { return DivideLazy.Value; }
		}

		#endregion // Field operators.


		#region Bitwise operators.

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

		#endregion // Bitwise operators.


		static Operator()
		{
			// Field operators.
			AddLazy = new Lazy<Func<T, T, T>>( () => OperatorHelper.CompileBinaryExpression<T, T, T>( Expression.Add ) );
			SubtractLazy = new Lazy<Func<T, T, T>>( () => OperatorHelper.CompileBinaryExpression<T, T, T>( Expression.Subtract ) );
			MultiplyLazy = new Lazy<Func<T, T, T>>( () => OperatorHelper.CompileBinaryExpression<T, T, T>( Expression.Multiply ) );
			DivideLazy = new Lazy<Func<T, T, T>>( () => OperatorHelper.CompileBinaryExpression<T, T, T>( Expression.Divide ) );

			// Bitwise operators.
			AndLazy = new Lazy<Func<T, T, T>>( () => BitwiseOperator<T>.And );
			OrLazy = new Lazy<Func<T, T, T>>( () => BitwiseOperator<T>.Or );
			ExclusiveOrLazy = new Lazy<Func<T, T, T>>( () => BitwiseOperator<T>.ExclusiveOr );
			NotLazy = new Lazy<Func<T, T>>( () => BitwiseOperator<T>.Not );
		}
	}


	/// <summary>
	///   Allows access to standard operators (e.g. addition) for a generic type,
	///   where the type which identifies distances between two instances is different. E.g. DateTime and TimeSpan.
	///   TODO: Could the other operator class extend from this one?
	/// </summary>
	/// <remarks>
	///   Lazy backing fields are used to prevent initializing an operator which isn't supported by the type.
	/// </remarks>
	public static class Operator<T, TSize>
	{
		#region Field operators.

		static readonly Lazy<Func<T, TSize, T>> AddSizeLazy;
		/// <summary>
		///   A delegate to evaluate binary addition (+) of a size to the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, TSize, T> AddSize
		{
			get { return AddSizeLazy.Value; }
		}

		static readonly Lazy<Func<T, T, TSize>> SubtractLazy;
		/// <summary>
		///   A delegate to evaluate binary subtraction (-) for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, T, TSize> Subtract
		{
			get { return SubtractLazy.Value; }
		}

		static readonly Lazy<Func<T, TSize, T>> SubtractSizeLazy;
		/// <summary>
		///   A delegate to evaluate binary subtraction (-) of a size for the given type.
		///   This delegate will throw an <see cref = "InvalidOperationException" /> if the type does not provide this operator.
		/// </summary>
		public static Func<T, TSize, T> SubtractSize
		{
			get { return SubtractSizeLazy.Value; }
		}

		#endregion // Field operators.


		static Operator()
		{
			// Field operators.
			AddSizeLazy = new Lazy<Func<T, TSize, T>>( () => OperatorHelper.CompileBinaryExpression<T, TSize, T>( Expression.Add ) );
			SubtractLazy = new Lazy<Func<T, T, TSize>>( () => OperatorHelper.CompileBinaryExpression<T, T, TSize>( Expression.Subtract ) );
			SubtractSizeLazy = new Lazy<Func<T, TSize, T>>( () => OperatorHelper.CompileBinaryExpression<T, TSize, T>( Expression.Subtract ) );
		}
	}
}
