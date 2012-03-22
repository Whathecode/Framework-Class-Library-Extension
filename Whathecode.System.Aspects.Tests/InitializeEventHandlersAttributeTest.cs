using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Aspects;


namespace Whathecode.Tests.System
{
	/// <summary>
	///   Unit tests for <see href = "InitializeEventHandlersAttribute" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class InitializeEventHandlersAttributeTest
	{
		[InitializeEventHandlers]
		class TestClass
		{
			public event Action Action;
			public void InvokeActionUnsafe()
			{
				Action();
			}

			public event Action<object, object> ParameterAction;
			public void InvokeParameterActionUnsafe()
			{
				ParameterAction( null, null );
			}

			public event EventHandler EventHandler;
			public void InvokeEventHandlerUnsafe()
			{
				EventHandler( this, null );
			}

			public event Action Initialized = delegate { };
			public void InvokeInitialized()
			{
				Initialized();
			}
		}


		[TestMethod]
		public void NullEventHandlersTest()
		{
			// When the aspect works, this shouldn't throw NullReferenceException's.
			var test = new TestClass();
			test.InvokeActionUnsafe();
			test.InvokeParameterActionUnsafe();
			test.InvokeEventHandlerUnsafe();			
		}

		[TestMethod]
		public void InitializedEventHandlerTest()
		{
			var test = new TestClass();
			test.InvokeInitialized();
		}

		[TestMethod]
		public void InvokeHandlersTest()
		{
			var test = new TestClass();
			bool handler1 = false;
			test.Action += () => handler1 = true;
			bool handler2 = false;
			test.Action += () => handler2 = true;
			test.InvokeActionUnsafe();

			Assert.IsTrue( handler1 && handler2 );
		}

		[TestMethod]
		public void GarbageCollectorTest()
		{
			TestClass testClass = new TestClass();
			AssertHelper.IsGarbageCollected( ref testClass );
		}
	}
}
