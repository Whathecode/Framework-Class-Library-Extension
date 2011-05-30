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
    public class WpfControlAttribute : Attribute, IAspectProvider, IAspect
    {
        readonly Type _propertiesEnumType;


        public WpfControlAttribute( Type propertiesEnumType )
        {
            _propertiesEnumType = propertiesEnumType;
        }


        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            Type targetType = (Type)targetElement;

            // TODO: Once PostSharp is fixed, the following generic aspect should work properly.
            // Meanwhile the more complex non generic aspect can be used which uses reflection.
            Type genericAspect = typeof( WpfControlAspect<> ).MakeGenericType( _propertiesEnumType );

            yield return new AspectInstance( targetType, Activator.CreateInstance( genericAspect ) as IAspect );

            // HACK: Non generic version required until fixed in PostSharp.
            /*yield return new AspectInstance(
                targetType,
                Activator.CreateInstance( typeof( WpfControlAspect ), _propertiesEnumType ) as IAspect );*/
        }
    }
}
