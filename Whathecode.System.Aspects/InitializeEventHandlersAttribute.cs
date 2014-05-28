using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Reflection;
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
	public class InitializeEventHandlersAttribute : EventLevelAspect, IInstanceScopedAspect
	{
		[NonSerialized]
		CachedDictionary<Type, Action<object>> _addEmptyEventHandlers;

		[NonSerialized]
		Type _declaringGenericType;

		[NonSerialized]
		bool _isInitialized;

		[OnMethodEntryAdvice, MethodPointcut( "SelectConstructors" )]
		public void OnConstructorEntry( MethodExecutionArgs args )
		{
			if ( !_isInitialized )
			{
				_addEmptyEventHandlers[ args.Instance.GetType() ]( args.Instance );
				_isInitialized = true;
			}
		}

		// ReSharper disable UnusedMember.Local
		IEnumerable<MethodBase> SelectConstructors( EventInfo target )
		{
			// An event always has a declaring type.
			Contract.Assume( target.DeclaringType != null );

			return
				// Constructors.
				target.DeclaringType.GetConstructors( ReflectionHelper.InstanceMembers )
				// Deserialization method.
				.Concat<MethodBase>( target.DeclaringType.GetMethods( ReflectionHelper.InstanceMembers ).Where( m => m.GetAttributes<OnDeserializingAttribute>().Any() ) );
		}
		// ReSharper restore UnusedMember.Local

		public override void RuntimeInitialize( EventInfo eventInfo )
		{
			// An event always has a declaring type.
			Contract.Assume( eventInfo.DeclaringType != null );

			base.RuntimeInitialize( eventInfo );

			Type declaringType = eventInfo.DeclaringType;
			_declaringGenericType = declaringType.IsGenericType ? declaringType.GetGenericTypeDefinition() : declaringType;

			_addEmptyEventHandlers = new CachedDictionary<Type, Action<object>>( type =>
			{
				// Find the type in which the constructor is defined.
				// Needed since events can be private, and those wouldn't be returned otherwise, even when searching a flattened hierarchy.
				Type baseType = type.GetMatchingGenericType( _declaringGenericType );

				EventInfo runtimeEvent = baseType.GetEvents( ReflectionHelper.ClassMembers ).Where( e => e.Name == eventInfo.Name ).First();

				MethodInfo delegateInfo = DelegateHelper.MethodInfoFromDelegateType( runtimeEvent.EventHandlerType );
				ParameterExpression[] parameters = delegateInfo.GetParameters().Select( p => Expression.Parameter( p.ParameterType ) ).ToArray();
				Delegate emptyDelegate
					= Expression.Lambda( runtimeEvent.EventHandlerType, Expression.Empty(), "EmptyDelegate", true, parameters ).Compile();

				// Create the delegate which adds the empty handler to an instance.
				MethodInfo addMethod = runtimeEvent.GetAddMethod( true );
				if ( addMethod.IsPublic )
				{
					return instance => runtimeEvent.AddEventHandler( instance, emptyDelegate );
				}

				return instance => addMethod.Invoke( instance, new object[] { emptyDelegate } );
			} );
		}

		public object CreateInstance( AdviceArgs adviceArgs )
		{
			return MemberwiseClone();
		}

		public void RuntimeInitializeInstance()
		{
			_isInitialized = false;
		}
	}
}
