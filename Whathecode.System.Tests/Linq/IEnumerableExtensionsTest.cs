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
        [TestMethod]
        public void CountOfTest()
        {
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4 };
            Assert.IsFalse( numbers.CountOf( 0 ) );
            Assert.IsFalse( numbers.CountOf( 2 ) );
            Assert.IsTrue( numbers.CountOf( 5 ) );
            Assert.IsFalse( numbers.CountOf( 10 ) );            
        }
    }
}
