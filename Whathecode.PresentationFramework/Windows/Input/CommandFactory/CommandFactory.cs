﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Whathecode.System.Reflection;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.Input.CommandFactory.Attributes;


namespace Whathecode.System.Windows.Input.CommandFactory
{
	/// <summary>
	///   A command factory to simplify creating and managing commands for a certain type.
	/// </summary>
	/// <typeparam name = "T">An enum used to identify the commands.</typeparam>
	/// <author>Steven Jeuris</author>
	public class CommandFactory<T> : AbstractEnumSpecifiedFactory<T>
	{
		/// <summary>
		///   The name of the property holding the dictionary with all the commands in the CommandFactory.
		/// </summary>
		public const string CommandsProperty = "Commands";

		readonly object _owner;

		/// <summary>
		///   The collection of commands created by this factory.
		///   TODO: Make this a readonly collection.
		/// </summary>
		public Dictionary<T, ICommand> Commands { get; private set; }

		/// <summary>
		///   Create a new command factory for an object.
		/// </summary>
		/// <param name = "owner">The object for which to create a command factory. (Usually just pass, 'this'.)</param>
		public CommandFactory( object owner )
			: base( owner.GetType(), true )
		{
			_owner = owner;
			Commands = new Dictionary<T, ICommand>();

			foreach ( var attribute in MatchingAttributes )
			{
				var method = (MethodInfo)attribute.Key;

				foreach ( var id in attribute.Value )
				{
					// Is a CanExecute method defined?
					Func<bool> canExecute = null;
					Dictionary<MemberInfo, CommandCanExecuteAttribute[]> matches = GetAttributedMembers<CommandCanExecuteAttribute>( (T)id.GetId() );
					if ( matches.Count > 0 )
					{
						if ( matches.Count > 1 )
						{
							throw new InvalidImplementationException(
								"Only one method for one ID can be attributed with a "
									+ typeof( CommandCanExecuteAttribute ) + "." );
						}

						// Get the can execute method.
						Dictionary<MemberInfo, CommandCanExecuteAttribute[]>.Enumerator matchesEnumerator = matches.GetEnumerator();
						matchesEnumerator.MoveNext();
						var canExecuteMethod = (MethodInfo)matchesEnumerator.Current.Key;
						canExecute = (Func<bool>)Delegate.CreateDelegate( typeof( Func<bool> ), _owner, canExecuteMethod );
					}

					// Based on parameters, create a suitable Command.
					ParameterInfo[] parameters = method.GetParameters();
					ICommand command;
					switch ( parameters.Length )
					{
						case 0:
						{
							Action execute = method.CreateDelegate<Action>( _owner );
							command = new DelegateCommand( execute, canExecute );
							break;
						}
						case 1:
						{
							// TODO: Support conversion to primitive types. (object -> int doesn't seem to work)
							Action<object> compatibleExecute
								= method.CreateDelegate<Action<object>>( _owner, DelegateHelper.CreateOptions.Downcasting );
							command = new DelegateCommand<object>( compatibleExecute, canExecute );
							break;
						}
						default:
						{
							throw new NotSupportedException( "The factory can only create commands without or with one parameter." );
						}
					}
					Commands.Add( (T)id.GetId(), command );
				}
			}
		}

		protected override Type[] GetAttributeTypes()
		{
			return new[] { typeof( CommandExecuteAttribute ) };
		}

		/// <summary>
		///   Retrieve a selected command from the command factory in the specified data context.
		/// </summary>
		/// <param name = "dataContext">The data context in which to look for the command factory.</param>
		/// <param name = "desiredCommand">The command to return from the command factory.</param>
		/// <returns>The requested command, or null when no data context is set.</returns>
		public static ICommand GetCommand( object dataContext, T desiredCommand )
		{
			if ( dataContext == null )
			{
				// No data context set.
				return null;
			}

			ICommand command = null;
			Type dataContextType = dataContext.GetType();
			MemberInfo[] commandFactories = dataContextType.GetMembers( typeof( CommandFactory<T> ) ).ToArray();

			foreach ( object factory in commandFactories.Select( dataContext.GetValue ) )
			{
				// Get dictionary containing commands from command factory.
				const string commandsProperty = CommandFactory<object>.CommandsProperty;
				var dictionary = factory.GetPropertyValue( commandsProperty ) as IDictionary;
				if ( dictionary == null )
				{
					throw new InvalidCastException( "Expected that \"" + commandsProperty + "\" property is IDictionary." );
				}
				if ( !dictionary.Contains( desiredCommand ) )
				{
					throw new ArgumentException( "No command found for command ID \"" + desiredCommand + "\"" );
				}

				command = (ICommand)dictionary[ desiredCommand ];
			}

			if ( command == null )
			{
				// No useful factory available.
				throw new InvalidImplementationException(
					"No CommandFactory for ID type \"" + desiredCommand.GetType() + "\" in type \"" + dataContextType + "\" found." );
			}

			return command;
		}
	}
}