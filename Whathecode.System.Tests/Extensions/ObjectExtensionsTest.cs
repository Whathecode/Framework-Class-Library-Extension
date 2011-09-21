using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Extensions;


namespace Whathecode.Tests.System.Extensions
{
	[TestClass]
	public class ObjectExtensionsTest
	{
		class DummyObject {}


		[TestMethod]
		public void ReferenceOrBoxedValueEqualsTest()
		{
			// ints
			object one = 1;
			object alsoOne = 1;
			object notOne = 2;
			Assert.AreEqual( true, one.ReferenceOrBoxedValueEquals( alsoOne ) );
			Assert.AreEqual( false, one.ReferenceOrBoxedValueEquals( notOne ) );

			// strings
			object bleh = "bleh";
			object equalBleh = "bleh";
			object notBleh = "notbleh";
			Assert.AreEqual( true, bleh.ReferenceOrBoxedValueEquals( equalBleh ) );
			Assert.AreEqual( false, bleh.ReferenceOrBoxedValueEquals( notBleh ) );

			// Reference types.
			object dummy1 = new DummyObject();
			object dummy2 = new DummyObject();
			Assert.AreEqual( true, dummy1.ReferenceOrBoxedValueEquals( dummy1 ) );
			Assert.AreEqual( false, dummy1.ReferenceOrBoxedValueEquals( dummy2 ) );
		}
	}
}