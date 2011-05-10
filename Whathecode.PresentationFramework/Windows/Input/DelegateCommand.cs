using System;
using Whathecode.System.Windows.Threading;


namespace Whathecode.System.Windows.Input
{
    /// <summary>
    ///   A command which executes a delegate method when triggered.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public class DelegateCommand : AbstractDelegateCommand
    {
        readonly Action _execute;

        /// <summary>
        ///   Create a new command which executes a delegate when triggered.
        /// </summary>
        /// <param name = "execute">The delegate to invoke when command is executed.</param>
        public DelegateCommand( Action execute )
            : this( execute, null ) {}

        /// <summary>
        ///   Create a new command which executes delegates when triggered.
        /// </summary>
        /// <param name = "execute">The delegate to invoke when command is executed.</param>
        /// <param name = "canExecute">
        ///   The delegate to invoke when a check is done to check whether the command can be executed.
        /// </param>
        public DelegateCommand( Action execute, Func<bool> canExecute )
            : base( canExecute )
        {
            _execute = execute;
        }


        public override void Execute( object parameter )
        {
            DispatcherHelper.SafeDispatch( Dispatcher, _execute );
        }
    }

    /// <summary>
    ///   A command with parameters which executes a delegate method when triggered.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command argument.</typeparam>
    /// <author>Steven Jeuris</author>
    public class DelegateCommand<TParameter> : AbstractDelegateCommand
    {
        readonly Action<TParameter> _execute;

        /// <summary>
        ///   Create a new command which executes a delegate when triggered.
        /// </summary>
        /// <param name = "execute">The delegate to invoke when command is executed.</param>
        public DelegateCommand( Action<TParameter> execute )
            : this( execute, null ) { }

        /// <summary>
        ///   Create a new command which executes delegates when triggered.
        /// </summary>
        /// <param name = "execute">The delegate to invoke when command is executed.</param>
        /// <param name = "canExecute">
        ///   The delegate to invoke when a check is done to check whether the command can be executed.
        /// </param>
        public DelegateCommand( Action<TParameter> execute, Func<bool> canExecute )
            : base( canExecute )
        {
            _execute = execute;
        }


        public override void Execute( object parameter )
        {
            if ( !(parameter is TParameter) )
            {
                throw new InvalidOperationException( "Invalid parameter for this command. Expecting a \"" + typeof( TParameter ) + "\"." );
            }

            DispatcherHelper.SafeDispatch( Dispatcher, _execute, (TParameter)parameter );
        }
    }
}