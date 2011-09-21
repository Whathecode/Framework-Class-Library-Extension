using System.Collections.Generic;
using System.Windows;
using Whathecode.System.ComponentModel.Validation;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	/// <summary>
	///   Class with the dependency properties managed through the dependency property factory.
	/// </summary>
	public class TestControl : BaseTestControl
	{
		static readonly DependencyPropertyFactory<Property> PropertyFactory = new DependencyPropertyFactory<Property>( typeof( TestControl ) );

#pragma warning disable 169
		// Requirement of the naming conventions. http://whathecode.wordpress.com/2010/10/25/dependency-property-factory-part-2/
		public static readonly DependencyProperty StandardProperty = PropertyFactory[ Property.Standard ];
		public static readonly DependencyProperty ReadOnlyProperty = PropertyFactory[ Property.ReadOnly ];
		public static readonly DependencyProperty CallbackProperty = PropertyFactory[ Property.Callback ];
		public static readonly DependencyProperty MinimumProperty = PropertyFactory[ Property.Minimum ];
		public static readonly DependencyProperty MaximumProperty = PropertyFactory[ Property.Maximum ];
#pragma warning restore 169

		[DependencyProperty( Property.Standard, DefaultValue = 100 )]
		public int Standard
		{
			get { return (int)PropertyFactory.GetValue( this, Property.Standard ); }
			set { PropertyFactory.SetValue( this, Property.Standard, value ); }
		}

		[DependencyProperty( Property.ReadOnly, DefaultValue = false )]
		public bool ReadOnly
		{
			get { return (bool)PropertyFactory.GetValue( this, Property.ReadOnly ); }
			private set { PropertyFactory.SetValue( this, Property.ReadOnly, value ); }
		}

		[DependencyProperty( Property.Callback, DefaultValue = "default" )]
		[ValidationHandler( typeof( CallbackValidation ) )]
		public string Callback
		{
			get { return (string)PropertyFactory.GetValue( this, Property.Callback ); }
			set { PropertyFactory.SetValue( this, Property.Callback, value ); }
		}

		[DependencyProperty( Property.Minimum, DefaultValue = 0 )]
		public int Minimum
		{
			get { return (int)PropertyFactory.GetValue( this, Property.Minimum ); }
			set { PropertyFactory.SetValue( this, Property.Minimum, value ); }
		}

		[DependencyProperty( Property.Maximum, DefaultValue = 100 )]
		[CoercionHandler( typeof( CoerceValidation ), Property.Minimum )]
		public int Maximum
		{
			get { return (int)PropertyFactory.GetValue( this, Property.Maximum ); }
			set { PropertyFactory.SetValue( this, Property.Maximum, value ); }
		}


		// ReSharper disable UnusedMember.Local
		[DependencyPropertyChanged( Property.Callback )]
		static void OnChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			ChangedCallbackCalled( d, e );
		}

		[DependencyPropertyCoerce( Property.Callback )]
		static object OnCoerce( DependencyObject d, object value )
		{
			return CoerceCallbackCalled( d, value );
		}

		// ReSharper restore UnusedMember.Local


		class CallbackValidation : AbstractValidation<string>
		{
			public override bool IsValid( string value )
			{
				return ValidateCallbackCalled( value );
			}
		}


		class CoerceValidation : AbstractControlCoercion<Property, int>
		{
			public CoerceValidation( Property dependentProperties )
				: base( dependentProperties ) {}

			protected override int Coerce( Dictionary<Property, object> dependentValues, int value )
			{
				int minimum = (int)dependentValues[ Property.Minimum ];
				return value < minimum ? minimum : value;
			}
		}
	}
}