using System;
using System.Collections.Generic;
using System.Linq;
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
	[MulticastAttributeUsage( MulticastTargets.Class )]
	[Serializable]
	public class InitializeEventHandlersAttribute : Attribute, IAspectProvider
	{
		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			Type targetType = (Type)targetElement;

			return
				from @event in targetType.GetEvents()
				let eventAspect = new InitializeEventHandlerAspect()
				select new AspectInstance( @event, eventAspect );
		}
	}
}
