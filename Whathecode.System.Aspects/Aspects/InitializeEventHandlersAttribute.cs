using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;


namespace Whathecode.System.Aspects
{
	/// <summary>
	///   Aspect which when applied on an assembly or class, initializes all the event handlers (<see cref="MulticastDelegate" />) members
	///   in the class(es) with empty delegates to prevent <see cref="NullReferenceException" />'s.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Event )]
	[MulticastAttributeUsage( MulticastTargets.Event, AllowMultiple = false )]
	[AspectTypeDependency( AspectDependencyAction.Commute, typeof( InitializeEventHandlersAttribute ) )]
	[Serializable]
	public class InitializeEventHandlersAttribute : EventLevelAspect
	{
		[NonSerialized]
		Action<object> _addEmptyEventHandler;


		[OnMethodEntryAdvice, MethodPointcut( "SelectConstructors" )]
		public void OnConstructorEntry( MethodExecutionArgs args )
		{
			_addEmptyEventHandler( args.Instance );
		}

		// ReSharper disable UnusedMember.Local
		IEnumerable<ConstructorInfo> SelectConstructors( EventInfo target )
		{
			return target.DeclaringType.GetConstructors( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
		}
		// ReSharper restore UnusedMember.Local

		public override void RuntimeInitialize( EventInfo eventInfo )
		{
			base.RuntimeInitialize( eventInfo );

			// Construct a suitable empty event handler.
			MethodInfo delegateInfo = DelegateHelper.MethodInfoFromDelegateType( eventInfo.EventHandlerType );
			ParameterExpression[] parameters = delegateInfo.GetParameters().Select( p => Expression.Parameter( p.ParameterType ) ).ToArray();
			Delegate emptyDelegate
				= Expression.Lambda( eventInfo.EventHandlerType, Expression.Empty(), "EmptyDelegate", true, parameters ).Compile();

			// Create a delegate which adds the empty handler to an instance.
			MethodInfo addMethod = eventInfo.GetAddMethod( true );
			if ( addMethod.IsPublic )
			{
				_addEmptyEventHandler = instance => eventInfo.AddEventHandler( instance, emptyDelegate );
			}
			else
			{
				_addEmptyEventHandler = instance => addMethod.Invoke( instance, new[] { emptyDelegate } );
			}
		}
	}
}
