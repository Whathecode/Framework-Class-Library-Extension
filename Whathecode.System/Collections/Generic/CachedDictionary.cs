using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Collections.Generic
{
	/// <summary>
	///   A dictionary which can be used to store values for a set of keys, which are only set the first time they are requested.
	///   After the first initialization, next Get() calls will return the cached value.
	/// </summary>
	/// <typeparam name = "TKey">The type of the key, used to receive values.</typeparam>
	/// <typeparam name = "TValue">The type for the stored cached values.</typeparam>
	/// <author>Steven Jeuris</author>
	public class CachedDictionary<TKey, TValue>
	{
		readonly Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();
		readonly Func<TKey, TValue> _valueInitializer;


		/// <summary>
		///   Initialize a new empty CachedDictionary.
		/// </summary>
		/// <param name = "valueInitializer">The delegate to be called to initialize an uncached value for a given key.</param>
		public CachedDictionary( Func<TKey, TValue> valueInitializer )
		{
			Contract.Requires( valueInitializer != null );

			_valueInitializer = valueInitializer;
		}


		/// <summary>
		///   Get the value for a given key. Initializes the value first when it's not yet initialized.
		/// </summary>
		/// <param name = "key">The key to get the matching value from.</param>
		/// <returns>The value of the given key.</returns>
		public TValue this[ TKey key ]
		{
			get
			{
				TValue value;
				if ( !_values.TryGetValue( key, out value ) )
				{
					value = _valueInitializer( key );
					_values[ key ] = value;
				}

				return value;
			}
		}
	}
}