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

		public static void IsGarbageCollected<TObject>( ref TObject @object )
			where TObject : class
		{
			Action<TObject> emptyAction = o => { };
			IsGarbageCollected( ref @object, emptyAction );
		}

		public static void IsGarbageCollected<TObject>( ref TObject @object, Action<TObject> useObject )
			where TObject : class
		{
			if ( typeof( TObject ) == typeof( string ) )
			{
				return;
			}

			int generation = GC.GetGeneration( @object );
			useObject( @object );
			WeakReference reference = new WeakReference( @object, true );
			@object = null;

			GC.Collect( generation, GCCollectionMode.Forced );
			GC.WaitForPendingFinalizers();

			Assert.IsNull( reference.Target );
		}
	}
}