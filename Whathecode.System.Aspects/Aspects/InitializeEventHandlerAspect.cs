using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PostSharp.Aspects;


namespace Whathecode.System.Aspects
{
	[Serializable]
	public class InitializeEventHandlerAspect : IEventLevelAspect, IInstanceScopedAspect
	{
		[NonSerialized]
		WeakReference _instance;

		[NonSerialized]
		EventInfo _event;


		public void RuntimeInitialize( EventInfo eventInfo )
		{
			_event = eventInfo;
		}

		public object CreateInstance( AdviceArgs adviceArgs )
		{
			_instance = new WeakReference( adviceArgs.Instance );
			return MemberwiseClone();
		}

		public void RuntimeInitializeInstance()
		{
			MethodInfo delegateInfo = DelegateHelper.MethodInfoFromDelegateType( _event.EventHandlerType );
			ParameterExpression[] parameters = delegateInfo.GetParameters().Select( p => Expression.Parameter( p.ParameterType ) ).ToArray();
			Delegate emptyDelegate
				= Expression.Lambda( _event.EventHandlerType, Expression.Empty(), "EmptyDelegate", true, parameters ).Compile();
			_event.AddEventHandler( _instance.Target, emptyDelegate );
		}
	}
}
