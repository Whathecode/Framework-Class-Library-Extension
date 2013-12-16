using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory.Aspects;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Aspects
{
	[TestClass]
	public class GenericControlTest
	{
		[Flags]
		public enum GenericProperties
		{
			Generic
		}


		[WpfControl( typeof( GenericProperties ) )]
		class GenericControl<T> : DependencyObject
		{
			[DependencyProperty( GenericProperties.Generic )]
			public T Generic { get; set; }
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
	}
}
