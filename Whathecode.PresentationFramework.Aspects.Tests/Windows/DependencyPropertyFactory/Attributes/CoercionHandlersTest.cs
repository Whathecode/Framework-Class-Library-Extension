using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory.Aspects;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Attributes
{
    [TestClass]
    public class CoercionHandlersTest
    {
        #region Common test members

        CoercionControl _control;

        [WpfControl( typeof( Property ) )]
        private class CoercionControl : DependencyObject
        {
            [Flags]
            enum Property
            {
                StartRange,
                EndRange,
                RangeCoercion
            }

            [DependencyProperty( Property.StartRange, DefaultValue = 0 )]
            public int StartRange { get; set; }

            [DependencyProperty( Property.EndRange, DefaultValue = 100 )]
            public int EndRange { get; set; }

            [DependencyProperty( Property.RangeCoercion )]
            [RangeCoercion( typeof( int ), Property.StartRange, Property.EndRange )]
            public int RangeCoercion { get; set; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _control = new CoercionControl();
        }

        #endregion  // Common test members


        [TestMethod]
        public void RangeCoercionTest()
        {           
            // Test coercion left of range.
            _control.RangeCoercion = -100;
            Assert.AreEqual( 0, _control.RangeCoercion );
            _control.StartRange = -100; // Recoercion.
            Assert.AreEqual( -100, _control.RangeCoercion );

            // Test coercion right of range.
            _control.RangeCoercion = 200;
            Assert.AreEqual( 100, _control.RangeCoercion );
            _control.EndRange = 200; // Recoercion.
            Assert.AreEqual( 200, _control.RangeCoercion );
        }
    }
}
