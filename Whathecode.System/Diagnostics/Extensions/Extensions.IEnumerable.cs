using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Diagnostics.Extensions
{
	public static class Extensions
	{
		/// <summary>
		///   Returns all elements on which a performed action throws an exception.
		/// </summary>
		/// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "action">The action to verify whether it throws an exception.</param>
		/// <param name = "exceptionType">When not null, a specific exception type to look for.</param>
		/// <returns>A sequence of all elements on which the performed action threw an exception.</returns>
		public static IEnumerable<T> ThrowsException<T>( this IEnumerable<T> source, Action<T> action, Type exceptionType = null )
		{
			Contract.Requires( exceptionType == null || typeof( Exception ).IsAssignableFrom( exceptionType ) );

			using ( IEnumerator<T> iterator = source.GetEnumerator() )
			{
				while ( iterator.MoveNext() )
				{
					bool threwException = false;
					try
					{
						action( iterator.Current );
					}
					catch ( Exception e )
					{
						if ( exceptionType == null || exceptionType.IsAssignableFrom( e.GetType() ) )
						{
							threwException = true;
						}
					}

					if ( threwException )
					{
						yield return iterator.Current;
					}
				}
			}
		}
	}
}