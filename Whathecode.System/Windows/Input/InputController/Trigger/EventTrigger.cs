using System;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
    /// <summary>
    ///   A class which triggers events when certain input conditions are met.
    /// </summary>
    /// <author>Steven Jeuris <email>mailto:steven@aimproductions.be</email></author>
    public class EventTrigger : AbstractTrigger
    {
        /// <summary>
        ///   The action which gets triggered once the input conditions are met.
        /// </summary>
        public event EventHandler TriggerEvent;

        /// <summary>
        ///   The action which gets triggered once the input conditions no longer validate to true.
        /// </summary>
        public event EventHandler ConditionLostEvent;

        /// <summary>
        ///   The event arguments which will be passed along once events are triggered.
        /// </summary>
        public EventArgs EventArguments { get; set; }


        public EventTrigger( AbstractCondition inputCondition )
            : base( inputCondition ) {}


        #region AbstractTrigger Members

        protected override void TriggerAction()
        {
            if ( TriggerEvent != null )
            {
                TriggerEvent( this, EventArguments );
            }
        }

        protected override void ConditionLost()
        {
            if ( ConditionLostEvent != null )
            {
                ConditionLostEvent( this, EventArguments );
            }
        }

        #endregion  // AbstractInputTrigger Members
    }
}