using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;


namespace Whathecode.System.Aspects
{
	/// <summary>
	///   Aspect which when applied on an assembly or class, initializes all the event handlers (<see cref="MulticastDelegate" />) members
	///   in the class(es) with empty delegates to prevent <see cref="NullReferenceException" />'s.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false )]
	[MulticastAttributeUsage( MulticastTargets.Class, AllowMultiple = false )]
	[Serializable]
	public class InitializeEventHandlersAttribute : MulticastAttribute, IAspectProvider
	{	
		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			Type targetType = targetElement as Type;

			if ( targetType == null )
			{
				return new AspectInstance[] { };
			}

			return
				from @event in targetType.GetEvents(
					BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance )
				let eventAspect = new InitializeEventHandlerAspect()
				select new AspectInstance( @event, eventAspect );
		}
	}
}
