using System;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
    /// <summary>
    ///   A class which triggers an action when a certain input condition is met.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractTrigger
    {
        /// <summary>
        ///   Condition which need to validate to true in order to trigger.
        /// </summary>
        readonly AbstractCondition _inputCondition;

        bool _enabled = true;

        bool _isTriggeredLastUpdate;
        DateTime _lastTriggerTime = new DateTime( 0 );

        /// <summary>
        ///   Enable/disable this trigger.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        ///   The minimum interval in milliseconds in between action triggers. Action isn't triggered faster than this.
        /// </summary>
        public TimeSpan MinimumTriggerInterval { get; set; }


        /// <summary>
        ///   Initialise an input trigger with the given condition.
        /// </summary>
        /// <param name = "condition"></param>
        protected AbstractTrigger( AbstractCondition condition )
        {
            _inputCondition = condition;
        }


        internal void Update()
        {
            _inputCondition.Update();

            if ( _enabled )
            {
                if ( _inputCondition.Validates() )
                {
                    _isTriggeredLastUpdate = true;

                    DateTime now = DateTime.Now;
                    if ( (now - _lastTriggerTime).TotalMilliseconds > MinimumTriggerInterval.TotalMilliseconds )
                    {
                        TriggerAction();
                        _lastTriggerTime = now;
                    }
                }
                else if ( _isTriggeredLastUpdate )
                {
                    _isTriggeredLastUpdate = false;
                    _lastTriggerTime = DateTime.MinValue;
                    ConditionLost();
                }
            }
        }


        /// <summary>
        ///   This function gets called when input condition is met.
        /// </summary>
        protected abstract void TriggerAction();

        /// <summary>
        ///   This function gets called when input condition is lost.
        /// </summary>
        protected abstract void ConditionLost();
    }
}