using System;


namespace Whathecode.System.Windows.Input.CommandFactory.Attributes
{
    /// <summary>
    ///   When applied to a function, specifies a delegate determining whether a command may be run.
    ///   Used by the CommandFactory.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [AttributeUsage( AttributeTargets.Method, AllowMultiple = true )]
    public class CommandCanExecuteAttribute : IdAttribute
    {
        /// <summary>
        ///   Create a new can execute command attribute for a specified command.
        /// </summary>
        /// <param name = "id">The ID of the command.</param>
        public CommandCanExecuteAttribute( object id )
            : base( id ) {}
    }
}