using System;
using System.Collections.Generic;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Perform an action on each item in the collection.
		/// </summary>
		/// <typeparam name = "T">The type of the collection.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "action">The action to perform.</param>
		public static void ForEach<T>( this IEnumerable<T> source, Action<T> action )
		{
			foreach ( var item in source )
			{
				action( item );
			}
		}
	}
}