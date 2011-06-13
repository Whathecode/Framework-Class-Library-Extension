using System;
using System.Collections.Generic;


namespace Whathecode.System
{
    /// <summary>
    ///   EXPERIMENTAL: Do not use!
    ///   Class which serves as a workaround to support variables scoped even beyond private.
    /// </summary>
    /// <typeparam name="TValue">The type of the local value.</typeparam>
    /// <author>Steven Jeuris</author>
    public class Private<TValue>
    {
        static readonly Dictionary<object, object> StaticScope = new Dictionary<object, object>();
        static readonly Dictionary<object, Dictionary<object, object>> InstanceScope = new Dictionary<object, Dictionary<object, object>>();


        public TValue Value { get; set; }


        private Private( TValue value )
        {
            Value = value;
        }


        /// <summary>
        ///   Create a new static local value which can only be accessed from the scope it was created in.
        ///   The value is shared among all instances, like an ordinary static.
        /// </summary>
        /// <param name="initialValue">Initializer for the value.</param>
        /// <returns>An instance of <see cref="Private{T}" /> through which the value can be accessed.</returns>
        public static Private<TValue> Static( Func<TValue> initialValue )
        {
            if ( !StaticScope.ContainsKey( initialValue ) )
            {
                Private<TValue> newInstance = new Private<TValue>( initialValue() );
                StaticScope.Add( initialValue, newInstance );
            }

            return StaticScope[ initialValue ] as Private<TValue>;
        }

        /// <summary>
        ///   Create a new local value bound to a single instance, but only accessible from within the scope it was created in.
        /// </summary>
        /// <typeparam name="TScope">The type of the class of the instance scope.</typeparam>
        /// <param name="initialValue">Initializer for the value.</param>
        /// <param name="instance">The instance to which the value is bound.</param>
        /// <returns>An instance of <see cref="Private{T}" /> through which the value can be accessed.</returns>
        public static Private<TValue> Instance<TScope>( Func<TValue> initialValue, TScope instance )
        {
            if ( !InstanceScope.ContainsKey( instance ) )
            {
                InstanceScope.Add( instance, new Dictionary<object, object>() );

                if ( !InstanceScope[ instance ].ContainsKey( instance ) )
                {
                    Private<TValue> newInstance = new Private<TValue>( initialValue() );
                    InstanceScope[ instance ].Add( initialValue, newInstance );
                }
            }

            return InstanceScope[ instance ][ initialValue ] as Private<TValue>;
        }
    }
}
