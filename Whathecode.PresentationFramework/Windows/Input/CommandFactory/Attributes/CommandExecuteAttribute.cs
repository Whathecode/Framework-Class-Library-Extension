using System;


namespace Whathecode.System.Windows.Input.CommandFactory.Attributes
{
    /// <summary>
    ///   When applied to a member, specifies the delegate to execute when a command is triggered.
    ///   Used by the CommandFactory.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
    public class CommandExecuteAttribute : IdAttribute
    {
        /// <summary>
        ///   Create a new execute command attribute for a specified command.
        /// </summary>
        /// <param name = "id">The ID of the command.</param>
        public CommandExecuteAttribute( object id )
            : base( id ) {}
    }
}