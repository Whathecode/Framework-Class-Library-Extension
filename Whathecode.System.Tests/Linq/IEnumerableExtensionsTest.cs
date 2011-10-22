using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Linq;


namespace Whathecode.Tests.System.Linq
{
	// ReSharper disable InconsistentNaming    
	[TestClass]
	public class IEnumerableExtensionsTest
		// ReSharper restore InconsistentNaming
	{
		#region Common Test Members

		List<int> _list;

		[TestInitialize]
		public void Initialize()
		{
			_list = new List<int> { 0, 1, 2, 3, 4 };
		}

		#endregion // Common Test Members


		[TestMethod]
		public void CountOfTest()
		{
			Assert.IsFalse( _list.CountOf( 0 ) );
			Assert.IsFalse( _list.CountOf( 2 ) );
			Assert.IsTrue( _list.CountOf( 5 ) );
			Assert.IsFalse( _list.CountOf( 10 ) );
		}
	}
}