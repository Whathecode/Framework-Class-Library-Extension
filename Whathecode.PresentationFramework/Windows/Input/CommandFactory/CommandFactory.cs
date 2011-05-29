using System;
using System.Collections.Generic;
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
                MethodInfo method = attribute.Key as MethodInfo;

                if ( method == null )
                {
                    throw new InvalidOperationException( "A MethodInfo object was expected." );
                }

                foreach ( var id in attribute.Value )
                {
                    // Is a CanExecute method defined?
                    Func<bool> canExecute = null;
                    Dictionary<MemberInfo, IdAttribute[]> matches = GetAttributedMembers<CommandCanExecuteAttribute>( (T)id.GetId() );
                    if ( matches.Count > 0 )
                    {
                        if ( matches.Count > 1 )
                        {
                            throw new InvalidOperationException(
                                "Only one method for one ID can be attributed with a " +
                                    typeof( CommandCanExecuteAttribute ) + "." );
                        }

                        // Get the can execute method.
                        Dictionary<MemberInfo, IdAttribute[]>.Enumerator matchesEnumerator = matches.GetEnumerator();
                        matchesEnumerator.MoveNext();
                        MethodInfo canExecuteMethod = (MethodInfo)matchesEnumerator.Current.Key;
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
                            Action<object> compatibleExecute 
                                = method.CreateDelegate<Action<object>>( _owner, DelegateHelper.CreateOptions.Upcasting );
                            command = new DelegateCommand<object>( compatibleExecute, canExecute );
                            break;
                        }
                        default:
                        {
                            throw new NotSupportedException(
                                "The factory can only create commands without or with one parameter."
                                );
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
    }
}