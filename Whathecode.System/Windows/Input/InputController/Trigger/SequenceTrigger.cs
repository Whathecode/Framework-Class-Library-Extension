using System.Collections.Generic;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
	/// <summary>
	///   A trigger which switches between a sequence of triggers, each time one is triggered.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SequenceTrigger : EventTrigger
	{
		class SequenceCondition : AbstractCondition
		{
			readonly LoopList<EventTrigger> _triggers;
			EventTrigger _currentTrigger;


			public SequenceCondition( IEnumerable<EventTrigger> triggers )
			{
				_triggers = new LoopList<EventTrigger>( triggers );
				_currentTrigger = _triggers.Next();
				_currentTrigger.ConditionsMet += OnConditionsMet;
			}


			void OnConditionsMet()
			{
				_currentTrigger.ConditionsMet -= OnConditionsMet;
				_currentTrigger = _triggers.Next();
				_currentTrigger.ConditionsMet += OnConditionsMet;
			}

			public override void Update()
			{
				_currentTrigger.Update();
			}

			protected override bool InputValidates()
			{
				// Nothing to do. The child triggers cause the desired side effects.
				return false;
			}
		}


		public SequenceTrigger( params EventTrigger[] triggers )
			: base( new SequenceCondition( triggers ) ) {}
	}
}