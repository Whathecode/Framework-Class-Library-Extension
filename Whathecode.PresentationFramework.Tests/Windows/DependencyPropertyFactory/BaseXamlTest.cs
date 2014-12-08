using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	public abstract class BaseXamlTest<TControl>
		where TControl : new()
	{
		TControl _control;


		[TestInitialize]
		public void InitializeTest()
		{
			_control = new TControl();
		}


		[TestMethod]
		public void XamlInitializationTest()
		{
			// Nothing to do.
		}
	}
}
