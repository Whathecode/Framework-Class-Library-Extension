using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;


namespace Whathecode.Tests.System
{
	[TestClass]
	public class VariableTest
	{
		[TestMethod]
		public void SwapTest()
		{
			// Value type.
			int a = 0;
			int b = 100;
			Variable.Swap( ref a, ref b );
			Assert.AreEqual( a, 100 );
			Assert.AreEqual( b, 0 );

			// Reference type.
			object object1 = new object();
			object object2 = new object();
			object oldObject1 = object1;
			object oldObject2 = object2;
			Variable.Swap( ref object1, ref object2 );
			Assert.AreEqual( object1, oldObject2 );
			Assert.AreEqual( object2, oldObject1 );
		}
	}
}
