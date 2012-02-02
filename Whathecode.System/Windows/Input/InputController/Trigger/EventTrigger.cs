using System;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
	/// <summary>
	///   A class which triggers events when a certain input condition is met or lost.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class EventTrigger
	{
		/// <summary>
		///   Condition which need to validate to true in order to trigger.
		/// </summary>
		readonly AbstractCondition _inputCondition;

		bool _isTriggeredLastUpdate;
		DateTime _lastTriggerTime = new DateTime( 0 );

		/// <summary>
		///   Enable/disable this trigger. Default is true.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		///   The minimum interval in milliseconds in between action triggers. Action isn't triggered faster than this.
		/// </summary>
		public TimeSpan MinimumTriggerInterval { get; set; }

		/// <summary>
		///   The action which gets triggered once the input conditions are met.
		/// </summary>
		public event Action ConditionsMet;

		/// <summary>
		///   The action which gets triggered once the input conditions no longer validate to true.
		/// </summary>
		public event Action ConditionLostEvent;


		/// <summary>
		///   Initialise an input trigger with the given condition.
		/// </summary>
		/// <param name = "condition"></param>
		public EventTrigger( AbstractCondition condition )
		{
			_inputCondition = condition;
			Enabled = true;
		}


		internal void Update()
		{
			_inputCondition.Update();

			if ( Enabled )
			{
				if ( _inputCondition.Validates() )
				{
					_isTriggeredLastUpdate = true;

					DateTime now = DateTime.Now;
					if ( (now - _lastTriggerTime).TotalMilliseconds >= MinimumTriggerInterval.TotalMilliseconds )
					{
						if ( ConditionsMet != null )
						{
							ConditionsMet();
						}
						_lastTriggerTime = now;
					}
				}
				else if ( _isTriggeredLastUpdate )
				{
					_isTriggeredLastUpdate = false;
					_lastTriggerTime = DateTime.MinValue;
					if ( ConditionLostEvent != null )
					{
						ConditionLostEvent();
					}
				}
			}
		}
	}
}