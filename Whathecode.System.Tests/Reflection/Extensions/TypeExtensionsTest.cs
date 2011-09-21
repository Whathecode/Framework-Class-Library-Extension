using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
	[TestClass]
	public class TypeExtensionsTest
	{
		#region Common test members

		interface IOne<T> {}


		class One<T> : IOne<T> {}


		class ExtendingOne<T> : One<T> {}


		readonly Type _baseType = typeof( One<int> );
		readonly Type _interfaceType = typeof( IOne<int> );
		readonly Type _incompleteInterfaceType = typeof( IOne<> );
		readonly Type _incompleteBaseType = typeof( One<> );
		readonly Type _extendingType = typeof( ExtendingOne<int> );

		#endregion // Common test members


		[TestMethod]
		public void GetMatchingGenericTypeTest()
		{
			// Base types.
			Assert.AreEqual( _baseType, _extendingType.GetMatchingGenericType( _baseType ) );
			Assert.AreEqual( _baseType, _extendingType.GetMatchingGenericType( _incompleteBaseType ) );

			// Interfaces.
			Assert.AreEqual( _interfaceType, _baseType.GetMatchingGenericType( _interfaceType ) );
			Assert.AreEqual( _interfaceType, _baseType.GetMatchingGenericType( _incompleteInterfaceType ) );
		}
	}
}