using System;
using System.Collections.Generic;
using PostSharp.Aspects;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Aspect which when applied to a DependencyObject, allows using the <see cref="DependencyPropertyFactory" /> without having to
    ///   add it to the class, or delegate calls to it in the property getters and setters.
    /// </summary>
    [Serializable]
    [AttributeUsage( AttributeTargets.Class )]
    public class WpfControlAttribute : Attribute, IAspectProvider
    {
        readonly Type _propertiesEnumType;


        public WpfControlAttribute( Type propertiesEnumType )
        {
            _propertiesEnumType = propertiesEnumType;
        }


        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            Type targetType = (Type)targetElement;
            Type genericAspect = typeof( WpfControlAspect<> ).MakeGenericType( _propertiesEnumType );

            yield return new AspectInstance( targetType, Activator.CreateInstance( genericAspect ) as IAspect );
        }
    }
}
