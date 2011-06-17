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
        ///   The name of the property holding the dictionary with all the commands
        ///   in the CommandFactory.
        ///   TODO: Can this string literal somehow be circumvented by accessing the generic class directly?
        /// </summary>
        const string CommandsProperty = "Commands";

        /// <summary>
        ///   An enum value specifying the command to which to bind.
        /// </summary>
        public object Command { get; set; }

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

            // Check whether the data context contains a CommandFactory<TCommands>.
            Type dataContextType = dataContext.GetType();
            MemberInfo[] commandFactories = dataContextType.GetMembers( typeof( CommandFactory<> ) ).ToArray();

            foreach ( var commandFactory in commandFactories )
            {
                // CommandFactory found.
                // Check whether type parameter matches the command type passed to the constructor.           
                Type genericType = commandFactory.GetMemberType();
                Type parameter = genericType.GetGenericArguments()[ 0 ];
                if ( parameter != Command.GetType() )
                {
                    throw new ArgumentException( "The passed command ID should be of type " + parameter + "." );
                }

                // Correct type, get factory.
                object factory = dataContext.GetValue( commandFactory );

                // Get dictionary containing commands from command factory.
                IDictionary dictionary = factory.GetPropertyValue( CommandsProperty ) as IDictionary;
                if ( dictionary == null )
                {
                    throw new InvalidCastException( "Expected that \"" + CommandsProperty + "\" property is IDictionary." );
                }
                if ( !dictionary.Contains( Command ) )
                {
                    throw new ArgumentException( "No command found for command ID \"" + Command + "\"" );
                }

                return dictionary[ Command ];
            }

            // No useful factory available.
            throw new InvalidImplementationException(
                "No CommandFactory for ID type \"" + Command.GetType() +
                "\" in type \"" + dataContextType + "\" found."
                );
        }
    }
}