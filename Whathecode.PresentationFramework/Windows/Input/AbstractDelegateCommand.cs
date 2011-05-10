using System;
using System.Windows;
using System.Windows.Threading;
using Whathecode.System.Windows.Threading;


namespace Whathecode.System.Windows.Input
{
    /// <summary>
    ///   Abstract class for a command which can execute a delegate method when triggered.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractDelegateCommand : AbstractCommand
    {
        readonly Func<bool> _canExecute;

        public Dispatcher Dispatcher { get; private set; }


        /// <summary>
        ///   Create a new command which executes delegates when triggered.
        /// </summary>
        /// <param name = "canExecute">
        ///   The delegate to invoke when a check is done to check whether the command can be executed.
        /// </param>
        protected AbstractDelegateCommand( Func<bool> canExecute )
        {
            _canExecute = canExecute;

            // Using current dispatcher is useful for unit tests where there is no application running.
            Dispatcher = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        }


        public override bool CanExecute( object parameter )
        {
            bool canExecute = true;

            if ( _canExecute != null )
            {
                canExecute = DispatcherHelper.SafeDispatch( Dispatcher, _canExecute );
            }

            return canExecute;
        }
    }
}