using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting
{
	public class AssertHelper
	{
		/// <summary>
		///   Verify whether a certain exception is thrown.
		///   Assertion fails when no exception or the wrong exception is thrown.
		/// </summary>
		/// <typeparam name = "T">The type of expection which is expected to be thrown.</typeparam>
		/// <param name = "action">The action to be executed which should throw an exception.</param>
		/// <param name = "allowDerivedTypes">Also allow exceptions which derive from the expected exception type.</param>
		public static void ThrowsException<T>( Action action, bool allowDerivedTypes = true )
		{
			Type type = typeof( T );

			try
			{
				action();
			}
			catch ( Exception e )
			{
				if ( allowDerivedTypes ? !(e is T) : e.GetType() != type )
				{
					Assert.Fail( "Incorrect exception is thrown. Expected \"" + type + "\", thrown \"" + e + "\"" );
				}

				return;
			}

			Assert.Fail( "Expected exception \"" + type + "\" was not thrown." );
		}

        /// <summary>
        ///   Verify whether a passed object can be garbage collected.
        ///   Assertion fails when the object could not be garbage collected.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to attempt to garbage collect.</typeparam>
        /// <param name="@object">The object which to attempt to garbage collect.</param>
		public static void IsGarbageCollected<TObject>( ref TObject @object )
			where TObject : class
		{
			Action<TObject> emptyAction = o => { };
			IsGarbageCollected( ref @object, emptyAction );
		}

        /// <summary>
        ///   Verify whether a passed object can be garbage collected after the execution of an operation.
        ///   Assertion fails when the object could not be garbage collected.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to attempt to garbage collect.</typeparam>
        /// <param name="@object">The object which to attempt to garbage collect.</param>
        /// <param name="useObject">The operation to be performed using the object.</param>
		public static void IsGarbageCollected<TObject>( ref TObject @object, Action<TObject> useObject )
			where TObject : class
		{
			if ( typeof( TObject ) == typeof( string ) )
			{
				// Strings are copied by value, and don't leak anyhow.
				return;
			}

			int generation = GC.GetGeneration( @object );
			useObject( @object );
			WeakReference reference = new WeakReference( @object );
			@object = null;

			GC.Collect( generation, GCCollectionMode.Forced );
			GC.WaitForPendingFinalizers();

			Assert.IsFalse( reference.IsAlive );
		}
	}
}