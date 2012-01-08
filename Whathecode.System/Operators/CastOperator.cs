using System;
using System.Linq.Expressions;


namespace Whathecode.System.Operators
{
	/// <summary>
	///   Allows access to cast operator for a generic type.
	/// </summary>
	/// <typeparam name="T">The type which to cast.</typeparam>
	/// <typeparam name="TResult">The type to cast to.</typeparam>
	/// <author>Steven Jeuris</author>
	public static class CastOperator<T, TResult>
	{
		/// <summary>
		///   A delegate to cast the given type to the desired resulting type.
		/// </summary>
		public static Func<T, TResult> Cast { get; private set; }


		static CastOperator()
		{
			ParameterExpression arg = Expression.Parameter( typeof( T ), "arg" );
			Cast = Expression.Lambda<Func<T, TResult>>( Expression.Convert( arg, typeof( TResult ) ), arg ).Compile();
		}
	}
}
