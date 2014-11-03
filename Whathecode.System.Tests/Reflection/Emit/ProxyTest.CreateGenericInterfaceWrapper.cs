using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Emit;


namespace Whathecode.Tests.System.Reflection.Emit
{
	partial class ProxyTest
	{
		[TestClass]
		public class CreateGenericInterfaceWrapperTest
		{
			const string TestString = "test";
			const int TestInt = 10;


			/// <summary>
			///   Test a simple interface wrapper.
			/// </summary>
			[TestMethod]
			public void InterfaceWrapper()
			{
				var test = new Test<string>();
				var wrappedTest = Proxy.CreateGenericInterfaceWrapper<ITest<object>>( test );
				wrappedTest.SetTest( TestString );
				Assert.AreEqual( TestString, wrappedTest.GetTest() );
				Assert.AreEqual( TestString, test.GetTest() );
				AssertHelper.ThrowsException<InvalidCastException>( () => wrappedTest.SetTest( 0 ) );
			}

			/// <summary>
			///   Test wrapper for interface with multiple parameters.
			/// </summary>
			[TestMethod]
			public void MultipleParameters()
			{
				var multiple = new MultipleParametersTest();
				var wrappedMultiple = Proxy.CreateGenericInterfaceWrapper<IMultipleParametersTest<object, object>>( multiple );
				wrappedMultiple.SetT1( TestString );
				wrappedMultiple.SetT2( TestInt );
				Assert.AreEqual( TestString, wrappedMultiple.GetT1() );
				Assert.AreEqual( TestInt, wrappedMultiple.GetT2() );
				Assert.AreEqual( TestString, multiple.GetT1() );
				Assert.AreEqual( TestInt, multiple.GetT2() );
				AssertHelper.ThrowsException<InvalidCastException>( () => wrappedMultiple.SetT1( 0 ) );
				AssertHelper.ThrowsException<InvalidCastException>( () => wrappedMultiple.SetT2( "bleh" ) );
			}

			/// <summary>
			///   Test wrapper for extending interface.
			/// </summary>
			[TestMethod]
			public void ExtendingInterface()
			{
				var extending = new ExtendedInterfaceTest();
				var wrappedExtending = Proxy.CreateGenericInterfaceWrapper<IExtendedTest<object>>( extending );
				wrappedExtending.SetTest( TestString );
				Assert.AreEqual( TestString, wrappedExtending.GetTest() );
				Assert.AreEqual( TestString, extending.GetTest() );
				AssertHelper.ThrowsException<InvalidCastException>( () => wrappedExtending.SetTest( 0 ) );
			}

			/// <summary>
			///   Test wrapper for composition interface.
			/// </summary>
			[TestMethod]
			public void CompositionInterface()
			{
				var compositionTest = new CompositionTest();
				var wrappedComposition = Proxy.CreateGenericInterfaceWrapper<IComposition<object>>( compositionTest );
				ITest<object> innerWrapped = wrappedComposition.GetTest();
				ITest<object> innerWrappedCached = wrappedComposition.GetTest();
				Assert.IsTrue( innerWrapped == innerWrappedCached ); // Check whether the internal wrapper is cached.
				compositionTest.SetTest( new Test<string>() );
				ITest<object> innerWrappedUpdated = wrappedComposition.GetTest();
				Assert.IsTrue( innerWrapped != innerWrappedUpdated ); // Check whether cache is updated when wrapped object changes.
				innerWrappedUpdated.SetTest( TestString );
				Assert.AreEqual( TestString, wrappedComposition.GetTest().GetTest() );
				Assert.AreEqual( TestString, compositionTest.GetTest().GetTest() );
				AssertHelper.ThrowsException<InvalidCastException>( () => wrappedComposition.GetTest().SetTest( 0 ) );

			}

			/// <summary>
			///   Test interfaces containing generic methods.
			/// </summary>
			[TestMethod]
			public void GenericMethods()
			{
				var genericMethodTest = new GenericMethodTest();
				var wrappedGenericMethod = Proxy.CreateGenericInterfaceWrapper<IGenericMethod<object>>( genericMethodTest );
			}
		}
	}
}
