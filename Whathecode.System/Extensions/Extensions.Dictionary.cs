using System;
using System.Collections.Generic;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Gets the value associated with the specified key and passes it to delegate which can use it.
		/// </summary>
		/// <typeparam name = "TKey">The type of the keys in the dictionary.</typeparam>
		/// <typeparam name = "TValue">The type of the values in the dictionary.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "key">The key of the element to use the value of if it's contained within the dictionary.</param>
		/// <param name = "useValue">The action to perform with the value if the key is contained within the dictionary.</param>
		/// <returns>true when the key was present in the dictionary and the action was performed, false otherwise.</returns>
		public static bool TryUseValue<TKey, TValue>( this Dictionary<TKey, TValue> source, TKey key, Action<TValue> useValue )
		{
			TValue value;
			if ( source.TryGetValue( key, out value ) )
			{
				useValue( value );

				return true;
			}

			return false;
		}
	}
}