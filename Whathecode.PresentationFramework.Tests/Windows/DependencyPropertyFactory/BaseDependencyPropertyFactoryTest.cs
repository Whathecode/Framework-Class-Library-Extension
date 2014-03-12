using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	/// <summary>
	///   Base class for unit tests for <see cref = "DependencyPropertyFactory{T}">DependencyPropertyFactory</see>.
	///   When extending from this test, the <see cref = "Property">Property</see> enum needs to be used to define the properties.
	/// </summary>
	/// <typeparam name = "TTestClass">The type of the class with dependency properties to be tested.</typeparam>
	/// <author>Steven Jeuris</author>
	public abstract class BaseDependencyPropertyFactoryTest<TTestClass>
		where TTestClass : BaseTestControl, new()
	{	
		#region Common Test Members

		TTestClass _test;

		/// <summary>
		///   List of all available dependency properties inside the class.
		/// </summary>
		Dictionary<PropertyInfo, DependencyPropertyAttribute> _propertyAttributes;

		/// <summary>
		///   List of all dependency property descriptors.
		/// </summary>
		Dictionary<Property, PropertyDescriptor> _propertyDescriptors;

		/// <summary>
		///   Getters which can be used to get the values through the CLR getters.
		/// </summary>
		Dictionary<Property, Func<object>> _propertyGetters;

		/// <summary>
		///   Setters which can be used to set values through the CLR setters.
		/// </summary>
		Dictionary<Property, Action<object>> _propertySetters;

		/// <summary>
		///   Contains values with which the dependency properties are initialized.
		/// </summary>
		static Dictionary<Property, object> _initializeValues;

		/// <summary>
		///   Contains values which are used to set a new value for the dependency properties.
		/// </summary>
		static readonly Dictionary<Property, object> SetValues = new Dictionary<Property, object>
		{
			{ Property.Standard, 500 },
			{ Property.ReadOnly, true },
			{ Property.Callback, "test" },
			{ Property.Minimum, 50 },
			{ Property.Maximum, 150 }
		};

		static readonly Property[] CoercedProperties = { Property.Maximum };

		[TestInitialize]
		public void InitializeTest()
		{
			_test = new TTestClass();

			// Get dependency properties.
			_propertyAttributes = (from p in typeof( TTestClass ).GetProperties()
				let attributes = p.GetAttributes<DependencyPropertyAttribute>()
				where attributes.Count() > 0
				select new
				{
					Property = p,
					Attribute = attributes.First()
				}).ToDictionary( p => p.Property, p => p.Attribute );

			// Get all dependency property descriptors, linked to the property enum.
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties( _test );
			_propertyDescriptors = (from property in _propertyAttributes
				from descriptor in properties.OfType<PropertyDescriptor>()
				where property.Key.Name == descriptor.Name
				select new
				{
					EnumValue = (Property)property.Value.GetId(),
					Descriptor = descriptor
				}).ToDictionary( p => p.EnumValue, p => p.Descriptor );

			// Verify whether all dependency properties where created.
			Assert.IsTrue( _propertyAttributes.Count == _propertyDescriptors.Count );

			// Verify whether all set values are specified in the dictionary.
			Assert.IsTrue( SetValues.Count == EnumHelper<Property>.GetValues().Count() );

			// Get default values from the attributes applied to the properties.
			_initializeValues = _propertyAttributes.ToDictionary( p => (Property)p.Value.GetId(), p => p.Value.DefaultValue );

			// Create getters and setters for the CLR properties.
			_propertyGetters = _propertyAttributes.ToDictionary(
				p => (Property)p.Value.GetId(),
				p => p.Key.GetGetMethod().CreateDelegate<Func<object>>( _test, DelegateHelper.CreateOptions.Downcasting ) );
			_propertySetters = _propertyAttributes
				.Where( p => p.Key.GetSetMethod() != null )
				.ToDictionary(
					p => (Property)p.Value.GetId(),
					p => p.Key.GetSetMethod().CreateDelegate<Action<object>>( _test, DelegateHelper.CreateOptions.Downcasting ) );

			// Verify whether correct amount of read only properties where created.
			Assert.IsTrue( _propertySetters.Count == _propertyDescriptors.Where( d => !d.Value.IsReadOnly ).Count() );
		}

		#endregion  // Common Test Members


		/// <summary>
		///   Getter and setter test for the common language runtime properties.
		/// </summary>
		[TestMethod]
		public void ClrGetterSetterTest()
		{
			// Check whether the values received through the getter are the same as the original values.
			foreach ( var property in EnumHelper<Property>.GetValues() )
			{
				Assert.AreEqual( _initializeValues[ property ], _propertyGetters[ property ]() );
			}

			// Set new values and compare afterwards. Only non-coerced settable properties are tested.
			foreach ( var property in _propertySetters.Keys.Except( CoercedProperties ) )
			{
				_propertySetters[ property ]( SetValues[ property ] );
				Assert.AreEqual( SetValues[ property ], _propertyGetters[ property ]() );
			}
		}

		/// <summary>
		///   Getter and setter test through dependency property identifiers.
		/// </summary>
		[TestMethod]
		public void DependencyPropertyGetterSetterTest()
		{
			// Check whether the values received through the dependency properties are the same as the default values.
			foreach ( var property in _propertyDescriptors )
			{
				Assert.AreEqual( _initializeValues[ property.Key ], property.Value.GetValue( _test ) );
			}

			// Set new values and compare afterwards. Read only dependency properties are skipped.
			var settableProperties = _propertyDescriptors.Where( p => !p.Value.IsReadOnly && !CoercedProperties.Contains( p.Key ) );
			foreach ( var property in settableProperties )
			{
				object setValue = SetValues[ property.Key ];
				property.Value.SetValue( _test, setValue );
				Assert.AreEqual( setValue, property.Value.GetValue( _test ) );
			}
		}

		/// <summary>
		///   Test whether setting the value of a read only property throws an exception.
		/// </summary>
		[TestMethod]
		public void ReadOnlyDependencyPropertyTest()
		{
			foreach ( var property in _propertyDescriptors.Where( d => d.Value.IsReadOnly ) )
			{
				var p = property; // Prevent access to modified closure.
				AssertHelper.ThrowsException<InvalidOperationException>( () => p.Value.SetValue( _test, SetValues[ p.Key ] ) );
			}
		}

		/// <summary>
		///   Test whether the callbacks get called.
		/// </summary>
		[TestMethod]
		public void CallbacksTest()
		{
			Assert.IsFalse( _test.IsCoerceCallbackCalled );
			Assert.IsFalse( _test.IsChangedCallbackCalled );
			Assert.IsTrue( BaseTestControl.IsValidateCallbackCalled ); // Is already called upon initialization.

			_test.ResetCallbackFlags();
			_propertySetters[ Property.Callback ]( SetValues[ Property.Callback ] );

			Assert.IsTrue( _test.AllCallbacksCalled );
		}

		/// <summary>
		///   Test whether coerce handler works.
		///   Maximum should be coerced to minimum the value of the Minimum property.
		/// </summary>
		[TestMethod]
		public void CoerceTest()
		{
			// Verify whether coercion works.
			int minimum = (int)_propertyGetters[ Property.Minimum ]();
			int newValue = minimum - 10;
			_propertySetters[ Property.Maximum ]( newValue );

			Assert.AreEqual( minimum, _propertyGetters[ Property.Maximum ]() );

			// Verify whether re-coercion works when dependent values change.
			_propertySetters[ Property.Minimum ]( newValue );
			Assert.AreEqual( newValue, _propertyGetters[ Property.Maximum ]() );
		}
	}	
}