using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Reflection.Extensions;


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
		CachedDictionary<Type, Action<object>> _addEmptyEventHandlers;


		[OnMethodEntryAdvice, MethodPointcut( "SelectConstructors" )]
		public void OnConstructorEntry( MethodExecutionArgs args )
		{
			_addEmptyEventHandlers[ args.Instance.GetType() ]( args.Instance );
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

			_addEmptyEventHandlers = new CachedDictionary<Type, Action<object>>( type =>
			{
				EventInfo runtimeEvent = type.GetEvents().Where( e => e.Name == eventInfo.Name ).First();

				MethodInfo delegateInfo = DelegateHelper.MethodInfoFromDelegateType( runtimeEvent.EventHandlerType );
				ParameterExpression[] parameters = delegateInfo.GetParameters().Select( p => Expression.Parameter( p.ParameterType ) ).ToArray();
				Delegate emptyDelegate
					= Expression.Lambda( runtimeEvent.EventHandlerType, Expression.Empty(), "EmptyDelegate", true, parameters ).Compile();

				// Adds the empty handler to the instance.
				MethodInfo addMethod = runtimeEvent.GetAddMethod( true );
				if ( addMethod.IsPublic )
				{
					return instance => runtimeEvent.AddEventHandler( instance, emptyDelegate );
				}

				return instance => addMethod.Invoke( instance, new object[] { emptyDelegate } );
			} );
		}
	}
}
