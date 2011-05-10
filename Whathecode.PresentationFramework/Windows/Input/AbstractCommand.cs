using System;
using System.Windows.Input;


namespace Whathecode.System.Windows.Input
{
    /// <summary>
    ///   An abstract implementation for ICommand.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract void Execute( object parameter );

        public abstract bool CanExecute( object parameter );

        public void RaiseExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}