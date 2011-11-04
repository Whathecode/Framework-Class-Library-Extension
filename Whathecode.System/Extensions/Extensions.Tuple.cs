using System;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Extensions
{
	public partial class Extensions
	{
		#region Tuple Actions

		/// <summary>
		///   Performs the given action on the elements of a 2-tuple, or pair.
		/// </summary>
		/// <param name = "source">The tuple to perform the action on.</param>
		/// <param name = "action">The action to perform.</param>
		public static void Action<T1, T2>( this Tuple<T1, T2> source, Action<T1, T2> action )
		{
			Contract.Requires( source != null );

			action( source.Item1, source.Item2 );
		}

		/// <summary>
		///   Performs the given action on the elements of a 3-tuple, or triple.
		/// </summary>
		/// <param name = "source">The tuple to perform the action on.</param>
		/// <param name = "action">The action to perform.</param>
		public static void Action<T1, T2, T3>( this Tuple<T1, T2, T3> source, Action<T1, T2, T3> action )
		{
			Contract.Requires( source != null );

			action( source.Item1, source.Item2, source.Item3 );
		}

		/// <summary>
		///   Performs the given action on the elements of a 4-tuple, or quadruple.
		/// </summary>
		/// <param name = "source">The tuple to perform the action on.</param>
		/// <param name = "action">The action to perform.</param>
		public static void Action<T1, T2, T3, T4>( this Tuple<T1, T2, T3, T4> source, Action<T1, T2, T3, T4> action )
		{
			Contract.Requires( source != null );

			action( source.Item1, source.Item2, source.Item3, source.Item4 );
		}

		#endregion // Tuple Actions


		#region Tuple Funcs

		/// <summary>
		///   Performs the given function on the elements of a 2-tuple, or pair.
		/// </summary>
		/// <param name = "source">The tuple to perform the function on.</param>
		/// <param name = "func">The function to execute.</param>
		public static TReturn Func<T1, T2, TReturn>( this Tuple<T1, T2> source, Func<T1, T2, TReturn> func )
		{
			Contract.Requires( source != null );

			return func( source.Item1, source.Item2 );
		}

		/// <summary>
		///   Performs the given function on the elements of a 3-tuple, or triple.
		/// </summary>
		/// <param name = "source">The tuple to perform the function on.</param>
		/// <param name = "func">The function to execute.</param>
		public static TReturn Func<T1, T2, T3, TReturn>( this Tuple<T1, T2, T3> source, Func<T1, T2, T3, TReturn> func )
		{
			Contract.Requires( source != null );

			return func( source.Item1, source.Item2, source.Item3 );
		}

		/// <summary>
		///   Performs the given function on the elements of a 4-tuple, or quadruple.
		/// </summary>
		/// <param name = "source">The tuple to perform the function on.</param>
		/// <param name = "func">The function to execute.</param>
		public static TReturn Func<T1, T2, T3, T4, TReturn>( this Tuple<T1, T2, T3, T4> source, Func<T1, T2, T3, T4, TReturn> func )
		{
			Contract.Requires( source != null );

			return func( source.Item1, source.Item2, source.Item3, source.Item4 );
		}

		#endregion // Tuple Funcs
	}
}