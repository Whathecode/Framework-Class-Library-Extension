using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using Whathecode.System.ComponentModel.Validation;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Aspects;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Class with the dependency properties managed through the dependency property factory simplified with an aspect by PostSharp.
    /// </summary>
    [WpfControl( typeof( Property ) )]
    public class TestControl : BaseTestControl
    {
        [DependencyProperty( Property.Standard, DefaultValue = 100 )]
        public int Standard { get; set; }

        [DependencyProperty( Property.ReadOnly, DefaultValue = false )]
        public bool ReadOnly { get; private set; }

        [DependencyProperty( Property.Callback, DefaultValue = "default" )]
        [ValidationHandler( typeof( CallbackValidation ) )]        
        public string Callback { get; set; }

        [DependencyProperty( Property.Minimum, DefaultValue =  0 )]
        public int Minimum { get; set; }

        [DependencyProperty( Property.Maximum, DefaultValue =  0 )]
        [CoercionHandler( typeof( CoerceValidation ), Property.Minimum )]
        public int Maximum { get; set; }


        // ReSharper disable UnusedMember.Local
        [DependencyPropertyChanged( Property.Callback )]
        private static void OnChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ChangedCallbackCalled( d, e );
        }

        [DependencyPropertyCoerce( Property.Callback )]
        private static object OnCoerce( DependencyObject d, object value )
        {
            return CoerceCallbackCalled( d, value );
        }
        // ReSharper restore UnusedMember.Local

        private class CallbackValidation : AbstractValidation<string>
        {
            public override bool IsValid( string value )
            {
                return ValidateCallbackCalled( value );
            }
        }

        private class CoerceValidation : AbstractControlCoercion<TestControl, Property, int>
        {
            public CoerceValidation( Property dependentProperties )
                : base( dependentProperties ) { }

            protected override int Coerce( Dictionary<Property, object> dependentValues, int value )
            {
                Contract.Requires( dependentValues != null );

                int minimum = (int)dependentValues[ Property.Minimum ];
                return value < minimum ? minimum : value;
            }
        }
    }
}
