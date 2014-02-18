using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting.Tests
{
	/// <summary>
	///   Unit tests for <see href = "InitializeEventHandlersAttribute" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class AssertHelperTest
	{
		class Subscriber
		{
			public Subscriber( Publisher observable )
			{
				observable.Event += EmptyAction;
			}

			// ReSharper disable MemberCanBeMadeStatic.Local
			// This needs to be an instance method for the memory leak to occur!
			void EmptyAction()
			{
			}
			// ReSharper restore MemberCanBeMadeStatic.Local
		}

		class Publisher
		{
			public event Action Event;

			public Publisher()
			{
				if ( Event != null )
				{
					Event();
				}
			}
		}


		[TestMethod]
		public void IsGarbageCollectedTest()
		{
			// Empty object without any references which are held.
			object empty = new object();
			AssertHelper.IsGarbageCollected( ref empty );

			// Strings are copied by value, but are collectable!
			string @string = "";
			AssertHelper.IsGarbageCollected( ref @string );

			// Keep reference around.
			object hookedEvent = new object();
			#pragma warning disable 168
			object referenceCopy = hookedEvent;
			#pragma warning restore 168
			AssertHelper.ThrowsException<AssertFailedException>( () => AssertHelper.IsGarbageCollected( ref hookedEvent ) );

			// Still attached as event.
			Publisher publisher = new Publisher();
			Subscriber subscriber = new Subscriber( publisher );
			AssertHelper.ThrowsException<AssertFailedException>( () => AssertHelper.IsGarbageCollected( ref subscriber ) );
		}
	}
}
