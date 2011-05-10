using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Tests.Windows.DependencyPropertyFactory
{
    /// <summary>
    ///   Unit tests for <see cref="DependencyPropertyFactory{T}">DependencyPropertyFactory</see>.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [TestClass]
    public class DependencyPropertyFactoryTest
    {
        #region Common Test Members

        /// <summary>
        ///   Class with the dependency properties managed through the dependency property factory.
        /// </summary>
        private class TestClass : DependencyObject
        {
            /// <summary>
            ///   Enum specifying all the dependency properties to be managed by the factory.
            /// </summary>
            public enum Property
            {
                Standard
            }


            static readonly DependencyPropertyFactory<Property> PropertyFactory = new DependencyPropertyFactory<Property>();

#pragma warning disable 169
            // Requirement of the naming conventions. http://whathecode.wordpress.com/2010/10/25/dependency-property-factory-part-2/
            public static readonly DependencyProperty StandardProperty = PropertyFactory[ Property.Standard ];
#pragma warning restore 169

            [DependencyProperty( Property.Standard )]
            public int Standard
            {
                get { return (int)PropertyFactory.GetValue( this, Property.Standard ); }
                set { PropertyFactory.SetValue( this, Property.Standard, value ); }
            }
        }


        TestClass _test;

        /// <summary>
        ///   Getters which can be used to get the values through the CLR getters.
        /// </summary>
        Dictionary<TestClass.Property, Func<object>> _propertyGetters;

        /// <summary>
        ///   Setters which can be used to set values through the CLR setters.
        /// </summary>
        Dictionary<TestClass.Property, Action<object>> _propertySetters;

        /// <summary>
        ///   Contains values with which the dependency properties are initialized.
        /// </summary>
        static readonly Dictionary<TestClass.Property, object> InitializeValues = new Dictionary<TestClass.Property, object>
        {
            { TestClass.Property.Standard, 100 }
        };

        /// <summary>
        ///   Contains values which are used to set a new value for the dependency properties.
        /// </summary>
        static readonly Dictionary<TestClass.Property, object> SetValues = new Dictionary<TestClass.Property, object>
        {
            { TestClass.Property.Standard, 500 }
        };        

        [TestInitialize]
        public void InitializeTest()
        {
            _test = new TestClass
            {
                Standard = (int)InitializeValues[ TestClass.Property.Standard ]
            };
            
            _propertyGetters = new Dictionary<TestClass.Property, Func<object>>
            {
                { TestClass.Property.Standard, () => _test.Standard }
            };

            _propertySetters = new Dictionary<TestClass.Property, Action<object>>
            {
                { TestClass.Property.Standard, o => _test.Standard = (int)o }
            };
        }

        #endregion  // Common Test Members


        /// <summary>
        ///   Getter and setter test for the common language runtime properties.
        /// </summary>
        [TestMethod]
        public void ClrGetterSetterTest()
        {
            // Check whether the values received through the getter are the same as the original values.
            foreach ( TestClass.Property property in EnumHelper<TestClass.Property>.GetValues() )
            {
                Assert.AreEqual( InitializeValues[ property ], _propertyGetters[ property ]() );
            }

            // Set new values and compare afterwards.
            foreach ( TestClass.Property property in _propertySetters.Keys )
            {
                _propertySetters[ property ]( SetValues[ property ] );
            }
            foreach ( TestClass.Property property in _propertySetters.Keys )
            {
                Assert.AreEqual( SetValues[ property ], _propertyGetters[ property ]() );
            }
        }
    }
}
