using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.Input.InputController.Condition;
using WTC = Whathecode.System.Windows.Input.InputController;


namespace Whathecode.Tests.System.Windows.Input.InputController
{
	/// <summary>
	///   Unit tests for <see cref = "InputController" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class InputControllerTest
	{
		protected WTC.InputController InputController { get; private set; }

		protected AbstractCondition AlwaysTrue { get; private set; }


		[TestInitialize]
		public void InitializeTest()
		{			
			InputController = new WTC.InputController();
			AlwaysTrue = new DelegateCondition( () => true );
		}
	}
}
