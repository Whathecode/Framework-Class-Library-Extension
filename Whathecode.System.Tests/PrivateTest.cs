using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;


namespace Whathecode.Tests.System
{
	[TestClass]
	public class PrivateTest
	{
		class PrivateTestClass
		{
			// ReSharper disable MemberCanBeMadeStatic.Local
			public static int StaticTest( int setValue )
				// ReSharper restore MemberCanBeMadeStatic.Local
			{
				Private<int> test = Private<int>.Static( () => 0 );

				int before = test.Value;
				test.Value = setValue;
				return before;
			}

			public int InstanceTest( int setValue )
			{
				Private<int> test = Private<int>.Instance( () => 0, this );

				int before = test.Value;
				test.Value = setValue;
				return before;
			}
		}


		[TestMethod]
		public void PrivateStaticTest()
		{
			PrivateTestClass instance1 = new PrivateTestClass();
			PrivateTestClass instance2 = new PrivateTestClass();

			PrivateTestClass.StaticTest( 10 );
			Assert.AreEqual( 10, PrivateTestClass.StaticTest( 20 ) );
			Assert.AreEqual( 20, PrivateTestClass.StaticTest( 30 ) );
		}

		[TestMethod]
		public void PrivateInstanceTest()
		{
			PrivateTestClass instance1 = new PrivateTestClass();
			PrivateTestClass instance2 = new PrivateTestClass();

			instance1.InstanceTest( 10 );
			Assert.AreEqual( 10, instance1.InstanceTest( 20 ) );
			instance2.InstanceTest( 50 );
			Assert.AreEqual( 20, instance1.InstanceTest( 30 ) );
		}
	}
}