using System.Collections.Generic;
using Whathecode.System.Windows.Input.InputController.Trigger;


namespace Whathecode.System.Windows.Input.InputController
{
    /// <summary>
    ///   A class which manages InputTrigger's.
    ///   Add wanted InputTrigger's to this class and call Update() as much as possible.
    ///   The InputTrigger's will be triggered when needed.
    /// </summary>
    /// <author>Steven Jeuris <email>mailto:steven@aimproductions.be</email></author>
    public class InputController
    {
        readonly List<AbstractTrigger> _toRemove = new List<AbstractTrigger>();

        /// <summary>
        ///   List of InputTrigger's which the InputController needs to manage.
        /// </summary>
        protected List<AbstractTrigger> Triggers { get; set; }

        /// <summary>
        ///   Enable/disable input triggers.
        /// </summary>
        public bool Enabled { get; set; }


        public InputController()
        {
            Enabled = true;
            Triggers = new List<AbstractTrigger>();
        }


        public void AddTrigger( AbstractTrigger trigger )
        {
            Triggers.Add( trigger );
        }

        public void RemoveTrigger( AbstractTrigger trigger )
        {
            _toRemove.Add( trigger );
        }

        public void Update()
        {
            if ( Enabled )
            {
                foreach ( var trigger in _toRemove )
                {
                    Triggers.Remove( trigger );
                }
                _toRemove.Clear();

                // TODO: Why is there sometimes a InvalidOperationException here? (Triggers collection modified.)
                foreach ( var trigger in Triggers )
                {
                    trigger.Update();
                }
            }
        }
    }
}