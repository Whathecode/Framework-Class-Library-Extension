using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.Input.InputController.Trigger;


namespace Whathecode.Tests.System.Windows.Input.InputController.Trigger
{
	/// <summary>
	///   Unit tests for <see cref = "SequenceTrigger" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class SequenceTriggerTest : InputControllerTest
	{
		[TestMethod]
		public void OneTriggerTest()
		{
			EventTrigger trigger = new EventTrigger( AlwaysTrue );
			SequenceTrigger sequenceTrigger = new SequenceTrigger( trigger );
			int triggerCount = 0;
			trigger.ConditionsMet += () => ++triggerCount;
			InputController.AddTrigger( sequenceTrigger );
			Assert.AreEqual( 0, triggerCount );

			InputController.Update();
			Assert.AreEqual( 1, triggerCount );

			InputController.Update();
			Assert.AreEqual( 2, triggerCount );
		}

		[TestMethod]
		public void MultipleTriggerTest()
		{
			EventTrigger trigger1 = new EventTrigger( AlwaysTrue );
			EventTrigger trigger2 = new EventTrigger( AlwaysTrue );
			EventTrigger trigger3 = new EventTrigger( AlwaysTrue );
			SequenceTrigger sequenceTrigger = new SequenceTrigger( trigger1, trigger2, trigger3 );
			int trigger1Count = 0;
			int trigger2Count = 0;
			int trigger3Count = 0;
			trigger1.ConditionsMet += () => ++trigger1Count;
			trigger2.ConditionsMet += () => ++trigger2Count;
			trigger3.ConditionsMet += () => ++trigger3Count;
			InputController.AddTrigger( sequenceTrigger );
			Assert.AreEqual( 0, trigger1Count );
			Assert.AreEqual( 0, trigger2Count );
			Assert.AreEqual( 0, trigger3Count );

			InputController.Update();
			Assert.AreEqual( 1, trigger1Count );
			Assert.AreEqual( 0, trigger2Count );
			Assert.AreEqual( 0, trigger3Count );

			InputController.Update();
			Assert.AreEqual( 1, trigger1Count );
			Assert.AreEqual( 1, trigger2Count );
			Assert.AreEqual( 0, trigger3Count );

			InputController.Update();
			Assert.AreEqual( 1, trigger1Count );
			Assert.AreEqual( 1, trigger2Count );
			Assert.AreEqual( 1, trigger3Count );

			InputController.Update();
			Assert.AreEqual( 2, trigger1Count );
			Assert.AreEqual( 1, trigger2Count );
			Assert.AreEqual( 1, trigger3Count );
		}
	}
}
