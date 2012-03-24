using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
	[TestClass]
	public class MethodInfoExtensionsTest
	{
		#region Common test members

		const string TestString = "bleh";
		readonly MethodInfo _toUpperMethod = typeof( string ).GetMethod( "ToUpper", Type.EmptyTypes );

		#endregion // Common test members


		[TestMethod]
		public void CreateDelegateTest()
		{
			Func<string> toUpper = _toUpperMethod.CreateDelegate<Func<string>>( TestString );
			Assert.AreEqual( TestString.ToUpper(), toUpper() );
		}

		[TestMethod]
		public void CreateDynamicInstanceDelegateTest()
		{
			Func<string, string> toUpper = _toUpperMethod.CreateOpenInstanceDelegate<Func<string, string>>();
			Assert.AreEqual( TestString.ToUpper(), toUpper( TestString ) );
		}
	}
}