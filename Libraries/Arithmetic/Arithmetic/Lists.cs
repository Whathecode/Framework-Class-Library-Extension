using System.Collections.Generic;


namespace Lambda.Generic.Arithmetic
{
	/// <summary>
	///   Provides methods like Sum, Average, Max, Min, Sigma for Lists of <typeparamref name = "T" />.
	/// </summary>
	/// <typeparam name = "T">The type that is used in the lists.</typeparam>
	/// <typeparam name = "C">The type providing the arithmetic operations for <typeparamref name = "T"></typeparamref>.</typeparam>
	public static class Lists<T, C>
		where C :
			IAdder<T>, ISubtracter<T>, IDivider<T>, IHasRoot<T>,
			IMinValueProvider<T>, IMaxValueProvider<T>, IZeroProvider<T>,
			IConversionProvider<T>, IComparer<T>, new()
	{
		/// <summary>
		///   An instance of the calculator. We will use this to perform the calculations.
		/// </summary>
		static readonly C c = new C();

		/// <summary>
		///   The sum of all elements in the <paramref name = "list" />
		/// </summary>
		/// <param name = "list">The list containing the elements to be summed up</param>
		/// <returns>The sum</returns>
		public static T Sum( List<T> list )
		{
			T sum = c.Zero;
			for ( int i = 0; i < list.Count; i++ )
			{
				sum = c.Add( sum, list[ i ] );
			}
			return sum;
		}

		/// <summary>
		///   The average of all elements in the <paramref name = "list" />
		/// </summary>
		/// <param name = "list">The list containing the elements to be averaged</param>
		/// <returns>The average</returns>
		public static T Average( List<T> list )
		{
			return c.Divide( Sum( list ), c.ConvertFrom( (ulong)list.Count ) );
		}

		/// <summary>
		///   The maximum of all elements in the <paramref name = "list" />
		/// </summary>
		/// <param name = "list">The list containing the elements</param>
		/// <returns>The biggest element</returns>
		public static T Max( List<T> list )
		{
			T max = c.MinValue;
			for ( int i = 0; i < list.Count; i++ )
			{
				if ( c.Compare( max, list[ i ] ) > 0 )
				{
					max = list[ i ];
				}
			}
			return max;
		}

		/// <summary>
		///   The minimum of all elements in the <paramref name = "list" />.
		/// </summary>
		/// <param name = "list">The list containing the elements</param>
		/// <returns>The smallest element</returns>
		public static T Min( List<T> list )
		{
			T min = c.MaxValue;
			for ( int i = 0; i < list.Count; i++ )
			{
				if ( c.Compare( min, list[ i ] ) < 0 )
				{
					min = list[ i ];
				}
			}
			return min;
		}

		/// <summary>
		///   The standard deviation of all elements in the <paramref name = "list" />.
		/// </summary>
		/// <param name = "list">The list containing the elements</param>
		/// <param name = "avg">The average</param>
		/// <returns>The standard deviation</returns>
		public static T Sigma( List<T> list, out T avg )
		{
			avg = Average( list );
			T rms = c.Zero;
			for ( int i = 0; i < list.Count; i++ )
			{
				rms = c.Add( rms, c.Sqr( c.Subtract( list[ i ], avg ) ) );
			}
			rms = c.Sqrt( c.Divide( rms, c.ConvertFrom( (ulong)list.Count ) ) );
			return rms;
		}

		/// <summary>
		///   The standard deviation of all elements in the <paramref name = "list" />.
		/// </summary>
		/// <param name = "list">The list containing the elements</param>
		/// <returns>The standard deviation</returns>
		public static T Sigma( List<T> list )
		{
			T dummy;
			return Sigma( list, out dummy );
		}
	}
}