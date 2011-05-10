using System.Collections.Generic;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Collections.Generic
{
    /// <summary>
    ///   A dictionary which can be used to store values for a set of keys, which are only initialized the first time they are requested.
    ///   After the first initialization, next Get() calls will return the cached value.
    /// </summary>
    /// <typeparam name = "TKey">The type of the key, used to receive values.</typeparam>
    /// <typeparam name = "TValue">The type for the stored cached values.</typeparam>
    /// <author>Steven Jeuris</author>
    public class CachedDictionary<TKey, TValue>
    {
        /// <summary>
        ///   Delegate which is used to initialize a requested value which isn't cached yet.
        /// </summary>
        /// <param name = "key">The key of the requested value.</param>
        /// <returns>The value for the requested key, which will be cached.</returns>
        public delegate TValue InitializeUncachedValueDelegate( TKey key );


        readonly Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();
        readonly InitializeUncachedValueDelegate _valueInitializer;


        /// <summary>
        ///   Initialize a new empty CachedDictionary.
        /// </summary>
        /// <param name = "valueInitializer">The delegate to be called to initialize a uncached value.</param>
        public CachedDictionary( InitializeUncachedValueDelegate valueInitializer )
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
            get { return Get( key ); }
        }

        /// <summary>
        ///   Get the value for a given key. Initializes the value first when it's not yet initialized.
        /// </summary>
        /// <param name = "key">The key to get the matching value from.</param>
        /// <returns>The value of the given key.</returns>
        public TValue Get( TKey key )
        {
            // Initialize value for the given key when it's not cached yet.
            if ( !_values.ContainsKey( key ) )
            {
                _values[ key ] = _valueInitializer( key );
            }

            return _values[ key ];
        }
    }
}