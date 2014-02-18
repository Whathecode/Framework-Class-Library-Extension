using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Markup;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Input.CommandFactory
{
	/// <summary>
	///   A custom markup extension used to specify a command binding.
	/// </summary>
	[MarkupExtensionReturnType( typeof( ICommand ) )]
	public class CommandBindingExtension : AbstractDataContextBindingExtension
	{
		/// <summary>
		///   An enum value specifying the command to which to bind.
		/// </summary>
		[ConstructorArgument( "commandId" )]
		public object Command { get; set; }

		/// <summary>
		///   Gets or sets the path to where the command is located.
		/// </summary>
		public string Path { get; set; }


		/// <summary>
		///   Create a new binding to a command with a given ID.
		/// </summary>
		/// <param name = "commandId">The ID for the command to bind to.</param>
		public CommandBindingExtension( object commandId )
		{
			if ( commandId == null )
			{
				throw new ArgumentException( "Command ID can't be null." );
			}

			Command = commandId;
		}

		protected override object ProvideValue( object dataContext )
		{
			if ( dataContext == null )
			{
				// No data context set yet.
				return null;
			}

			dataContext = dataContext.GetValue( Path );

			// Check whether the data context contains a CommandFactory<TCommands>.
			Type commandFactory = typeof( CommandFactory<> ).MakeGenericType( Command.GetType() );
			return commandFactory.InvokeMember(
				"GetCommand",
				BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, null,
				new [] { dataContext, Command } );
		}
	}
}