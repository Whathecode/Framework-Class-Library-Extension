using System;
using Whathecode.System.Collections.Generic;


namespace Whathecode.System.Experimental
{
	/// <summary>
	///   EXPERIMENTAL! More info: http://whathecode.wordpress.com/2011/06/13/beyond-private-accessibility/
	///   Class which serves as a workaround to support variables scoped even beyond private.
	/// </summary>
	/// <typeparam name = "TValue">The type of the local value.</typeparam>
	/// <author>Steven Jeuris</author>
	public class Private<TValue>
	{
		static readonly CachedDictionary<Func<TValue>, Private<TValue>> StaticScope = CreateDictionary();

		static readonly CachedDictionary<object, CachedDictionary<Func<TValue>, Private<TValue>>> InstanceScope
			= new CachedDictionary<object, CachedDictionary<Func<TValue>, Private<TValue>>>( o => CreateDictionary() );


		public TValue Value { get; set; }


		static CachedDictionary<Func<TValue>, Private<TValue>> CreateDictionary()
		{
			return new CachedDictionary<Func<TValue>, Private<TValue>>(
				key => new Private<TValue>
				{
					Value = key()
				} );
		}

		/// <summary>
		///   Create a new static local value which can only be accessed from the scope it was created in.
		///   The value is shared among all instances, like an ordinary static.
		/// </summary>
		/// <param name = "initialValue">Initializer for the value.</param>
		/// <returns>An instance of <see cref = "Private{T}" /> through which the value can be accessed.</returns>
		public static Private<TValue> Static( Func<TValue> initialValue )
		{
			return StaticScope[ initialValue ];
		}

		/// <summary>
		///   Create a new local value bound to a single instance, but only accessible from within the scope it was created in.
		/// </summary>
		/// <typeparam name = "TScope">The type of the class of the instance scope.</typeparam>
		/// <param name = "initialValue">Initializer for the value.</param>
		/// <param name = "instance">The instance to which the value is bound.</param>
		/// <returns>An instance of <see cref = "Private{T}" /> through which the value can be accessed.</returns>
		public static Private<TValue> Instance<TScope>( Func<TValue> initialValue, TScope instance )
		{
			return InstanceScope[ instance ][ initialValue ];
		}
	}
}