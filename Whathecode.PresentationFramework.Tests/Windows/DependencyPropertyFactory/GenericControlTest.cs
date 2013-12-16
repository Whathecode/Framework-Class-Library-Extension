using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	[TestClass]
	public class GenericControlTest
	{
		[Flags]
		public enum GenericProperties
		{
			Generic
		}


		class GenericControl<T> : DependencyObject
		{
			static readonly DependencyPropertyFactory<GenericProperties> PropertyFactory = new DependencyPropertyFactory<GenericProperties>( typeof( GenericControl<T> ) );

#pragma warning disable 169
			// Requirement of the naming conventions. http://whathecode.wordpress.com/2010/10/25/dependency-property-factory-part-2/
			public static readonly DependencyProperty GenericProperty = PropertyFactory[ GenericProperties.Generic ];
#pragma warning restore 169


			[DependencyProperty( GenericProperties.Generic )]
			public T Generic
			{
				get { return (T)PropertyFactory.GetValue( this, GenericProperties.Generic ); }
				set { PropertyFactory.SetValue( this, GenericProperties.Generic, value ); }
			}
		}

		/// <summary>
		///   Getter and setter test for the common language runtime properties.
		/// </summary>
		[TestMethod]
		public void ClrGetterSetterTest()
		{
			var control = new GenericControl<DateTime>();
			var now = DateTime.Now;
			control.Generic = now;

			Assert.AreEqual( now, control.Generic );
		}

		/// <summary>
		///   Getter and setter test through dependency property identifiers.
		/// </summary>
		[TestMethod]
		public void DependencyPropertyGetterSetterTest()
		{
			var control = new GenericControl<DateTime>();
			var now = DateTime.Now;
			control.SetValue( GenericControl<DateTime>.GenericProperty, now );

			Assert.AreEqual( now, control.GetValue( GenericControl<DateTime>.GenericProperty ) );
		}
	}
}
