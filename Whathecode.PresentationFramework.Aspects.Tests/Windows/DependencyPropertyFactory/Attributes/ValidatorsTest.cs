using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory.Aspects;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Attributes
{
	[TestClass]
	public class ValidatorsTest
	{
		#region Common test members

		ValidationControl _control;


		[WpfControl( typeof( Property ) )]
		public class ValidationControl : DependencyObject
		{
			[Flags]
			public enum Property
			{
				RegexValidation = 1
			}


			[DependencyProperty( Property.RegexValidation, DefaultValue = "test" )]
			[RegexValidation( "test" )]
			public string RegexValidation { get; set; }
		}


		[TestInitialize]
		public void TestInitialize()
		{
			_control = new ValidationControl();
		}

		#endregion  // Common test members


		[TestMethod]
		public void RegexValidationTest()
		{
			_control.RegexValidation = "test";
			AssertHelper.ThrowsException<ArgumentException>( () => _control.RegexValidation = "invalid" );
		}
	}
}