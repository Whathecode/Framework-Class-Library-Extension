using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
	[TestClass]
	public class FieldInfoExtensionsTest
	{
		#pragma warning disable 219, 169, 414
		public class Fields
		{
			public const string TestString = "Test";

			public string Public = TestString;
			string _private = TestString;
			public object StringObject = TestString;
		}
		#pragma warning restore 219, 169, 414


		readonly Fields _fields = new Fields();
		readonly Type _type = typeof( Fields );
		readonly FieldInfo _publicField = typeof( Fields ).GetField( "Public" );
		

		[TestMethod]
		public void CreateGetterDelegateTest()
		{
			// Public/private.
			Func<string> publicGetter = _publicField.CreateGetterDelegate<string>( _fields );
			Assert.AreEqual( Fields.TestString, publicGetter() );
			Func<string> privateGetter = _type
				.GetField("_private", BindingFlags.Instance | BindingFlags.NonPublic )
				.CreateGetterDelegate<string>( _fields );
			Assert.AreEqual( Fields.TestString, privateGetter() );

			// Delegate types which don't correspond, but allowed variance.
			Func<object> upcasting = _publicField.CreateGetterDelegate<object>( _fields );
			Assert.AreEqual( Fields.TestString, upcasting() );

			// Delegate types which don't correspond, and no variance possible.
			AssertHelper.ThrowsException<ArgumentException>( () => _type.GetField( "StringObject" ).CreateGetterDelegate<string>( _fields ) );
		}

		[TestMethod]
		public void CreateOpenInstanceGetterDelegateTest()
		{
			Func<Fields, string> openInstance = _publicField.CreateOpenInstanceGetterDelegate<Fields, string>();
			Assert.AreEqual( Fields.TestString, openInstance( _fields ) );
		}
	}
}