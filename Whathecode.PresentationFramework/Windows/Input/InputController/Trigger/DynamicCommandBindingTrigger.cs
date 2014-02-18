using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Whathecode.System.Windows.Input.CommandFactory;
using Whathecode.System.Windows.Input.InputController.Condition;


namespace Whathecode.System.Windows.Input.InputController.Trigger
{
	/// <summary>
	///   A class which triggers a <see cref="CommandFactory" /> command at a source provided through a callback.
	///   This can be used to bind to data context commands from code-behind.
	/// </summary>
	/// <typeparam name = "TCommand">An enum used to identify the commands.</typeparam>
	/// <author>Steven Jeuris</author>
	public class DynamicCommandBindingTrigger<TCommand> : EventTrigger
	{
		readonly Func<object> _getDataContext;
		readonly TCommand _desiredCommand;
		readonly object _parameter;

		object _cachedDataContext;
		ICommand _command;


		public DynamicCommandBindingTrigger( AbstractCondition condition, Func<object> getDataContext, TCommand command, object parameter = null )
			: base( condition )
		{
			Contract.Requires( condition != null );

			_getDataContext = getDataContext;
			_desiredCommand = command;
			_parameter = parameter;

			ConditionsMet += TriggerAction;
		}


		void TriggerAction()
		{
			if ( _cachedDataContext != _getDataContext() )
			{
				_cachedDataContext = _getDataContext();
				_command = CommandFactory<TCommand>.GetCommand( _cachedDataContext, _desiredCommand );
			}

			if ( _command != null && _command.CanExecute( _parameter ) )
			{
				_command.Execute( _parameter );
			}
		}
	}
}
