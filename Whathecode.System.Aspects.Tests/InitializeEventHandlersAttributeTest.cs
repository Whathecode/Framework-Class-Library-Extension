using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Aspects;


namespace Whathecode.Tests.System.Aspects
{
	/// <summary>
	///   Unit tests for <see href = "InitializeEventHandlersAttribute" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class InitializeEventHandlersAttributeTest
	{
		[InitializeEventHandlersAttribute]
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

		[InitializeEventHandlers]
		class GenericTestClass<T>
			where T : class
		{
			public event Action<T> Action;
			public void InvokeActionUnsafe()
			{
				Action( null );
			}
		}


		/// <summary>
		///   TODO: Verify whether this works properly in all cases.
		///   Determines whether this constructor calls another constructor which is specified in this class.
		///   Base constructors aren't considered.
		/// </summary>
		/// <param name = "constructor">The constructor to verify whether it calls another constructor.</param>
		/// <returns>True when this constructor calls another constructor within the class; false otherwise.</returns>
		public static bool CallsOtherConstructor( ConstructorInfo constructor )
		{
			MethodBody body = constructor.GetMethodBody();
			if ( body == null )
			{
				throw new ArgumentException( "Constructors are expected to always contain byte code." );
			}

			// Constructors at the end of the invocation chain start with 'call' immediately.
			var untilCall = body.GetILAsByteArray().TakeWhile( b => b != OpCodes.Call.Value ).ToList();
			return untilCall.Count != 0 && !untilCall.All( b =>
				b == OpCodes.Nop.Value ||     // Never encountered, but my intuition tells me a no-op would be valid.
				b == OpCodes.Ldarg_0.Value || // Seems to always precede Call immediately.
				b == OpCodes.Ldarg_1.Value    // Seems to be added when calling base constructor.
				);
		}

		[TestMethod]
		public void NullEventHandlersTest()
		{
			// When the aspect works, this shouldn't throw NullReferenceException's.

			var test = new TestClass();
			test.InvokeActionUnsafe();
			test.InvokeParameterActionUnsafe();
			test.InvokeEventHandlerUnsafe();

			var generic = new GenericTestClass<object>();
			generic.InvokeActionUnsafe();
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
			var testClass = new TestClass();
			AssertHelper.IsGarbageCollected( ref testClass );
		}
	}
}
