using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Extensions;


namespace Whathecode.Tests.System.Extensions
{
	[TestClass]
	public class ObjectExtensionsTest
	{
		class DummyObject
		{
			public int Value { get; set; }
		}

		class MainClass
		{
			public InnerClass Inner { get; set; }
		}

		class InnerClass
		{
			public DummyObject Dummy { get; set; }
		}


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

		[TestMethod]
		public void IfNotNullTest()
		{
			// Nothing null.
			DummyObject dummy = new DummyObject();
			MainClass notNull = new MainClass
			{
				Inner = new InnerClass
				{
					Dummy = dummy
				}
			};
			DummyObject inner = notNull.IfNotNull( x => x.Inner ).IfNotNull( x => x.Dummy );
			Assert.AreEqual( dummy, inner );

			// Halfway null.
			MainClass halfwayNull = new MainClass
			{
				Inner = new InnerClass()
			};
			inner = halfwayNull.IfNotNull( x => x.Inner ).IfNotNull( x => x.Dummy );
			Assert.IsNull( inner );

			// All null.
			MainClass isNull = null;
			inner = isNull.IfNotNull( x => x.Inner ).IfNotNull( x => x.Dummy );
			Assert.IsNull( inner );

			// Value type.
			const int setValue = 10;
			MainClass value = new MainClass
			{
				Inner = new InnerClass
				{
					Dummy = new DummyObject
					{
						Value = setValue
					}
				}
			};
			int innerValue = value.IfNotNull( x => x.Inner ).IfNotNull( x => x.Dummy ).IfNotNull( x => x.Value );
			Assert.AreEqual( setValue, innerValue );
		}
	}
}