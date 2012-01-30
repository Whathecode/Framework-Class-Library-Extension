using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.Input.CommandFactory;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
	/// <summary>
	///   A class which triggers a <see cref="CommandFactory" /> command for the DataContext of a <see cref="FrameworkElement" />.
	///   This can be used to bind to data context commands from code-behind.
	/// </summary>
	/// <typeparam name = "TCommand">An enum used to identify the commands.</typeparam>
	/// <author>Steven Jeuris</author>
	public class CommandBindingTrigger<TCommand> : AbstractTrigger
	{
		readonly TCommand _desiredCommand;
		ICommand _command;


		public CommandBindingTrigger( AbstractCondition condition, FrameworkElement element, TCommand command )
			: base( condition )
		{
			Contract.Requires( condition != null && element != null );

			_desiredCommand = command;

			element.DataContextChanged += OnDataContextChanged;
		}


		void OnDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
		{
			// TODO: Remove duplication of CommandBindingExtension.

			object dataContext = e.NewValue;
			if ( dataContext == null )
			{
				// No data context set.
				_command = null;
				return;
			}

			// Check whether the data context contains a CommandFactory<TCommands>.
			Type dataContextType = dataContext.GetType();
			MemberInfo[] commandFactories = dataContextType.GetMembers( typeof( CommandFactory<TCommand> ) ).ToArray();

			foreach ( object factory in commandFactories.Select( commandFactory => dataContext.GetValue( commandFactory ) ) )
			{
				// Get dictionary containing commands from command factory.
				const string commandsProperty = CommandFactory<object>.CommandsProperty;
				IDictionary dictionary = factory.GetPropertyValue( commandsProperty ) as IDictionary;
				if ( dictionary == null )
				{
					throw new InvalidCastException( "Expected that \"" + commandsProperty + "\" property is IDictionary." );
				}
				if ( !dictionary.Contains( _desiredCommand ) )
				{
					throw new ArgumentException( "No command found for command ID \"" + _desiredCommand + "\"" );
				}

				_command = (ICommand)dictionary[ _desiredCommand ];
			}

			if ( _command == null )
			{
				// No useful factory available.
				throw new InvalidImplementationException(
					"No CommandFactory for ID type \"" + _desiredCommand.GetType() + "\" in type \"" + dataContextType + "\" found." );
			}
		}


		protected override void TriggerAction()
		{
			if ( _command != null )
			{
				_command.Execute( null );
			}
		}

		protected override void ConditionLost()
		{
			// Nothing to do.
		}
	}
}
