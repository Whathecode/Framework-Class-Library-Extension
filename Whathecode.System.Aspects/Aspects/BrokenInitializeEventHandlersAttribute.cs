using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;


namespace Whathecode.System.Aspects
{
	[AttributeUsage( AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Event, AllowMultiple = false )]
	[MulticastAttributeUsage( MulticastTargets.Event )]
	[Serializable]
	public class BrokenInitializeEventHandlersAttribute : EventLevelAspect, IInstanceScopedAspect
	{
		[NonSerialized]
		object _instance;

		EventInfo _event;

		public override void CompileTimeInitialize( EventInfo targetEvent, AspectInfo aspectInfo )
		{
			_event = targetEvent;
		}

		public object CreateInstance( AdviceArgs adviceArgs )
		{
			_instance = adviceArgs.Instance;
			return MemberwiseClone();		
		}

		public void RuntimeInitializeInstance()
		{
			Action emptyAction = () => { };
			_event.AddEventHandler( _instance, emptyAction );
		}
	}
}
