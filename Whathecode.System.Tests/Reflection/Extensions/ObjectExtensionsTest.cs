using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
    [TestClass]
    public class ObjectExtensionsTest
    {
        #region Common test members

        class MainClass
        {
            public SubClass Sub { get; private set; }

            public MainClass( string value )
            {
                Sub = new SubClass( value );
            }
        }

        class SubClass
        {
            public SubSubClass SubSub { get; private set; }

            public SubClass( string value )
            {
                SubSub = new SubSubClass( value );
            }
        }

        class SubSubClass
        {
            public string Value { get; private set; }

            public SubSubClass( string value )
            {
                Value = value;
            }
        }

        #endregion // Common test members


        [TestMethod]
        public void GetValueTest()
        {
            // Get value by path.
            const string testString = "test";
            MainClass test = new MainClass( testString );
            Assert.AreEqual( testString, test.GetValue( "Sub.SubSub.Value" ) );

            // Get value of non-existing path.
            AssertHelper.ThrowsException<ArgumentException>( () => test.GetValue( "NonExisting" ) );
        }
    }
}
