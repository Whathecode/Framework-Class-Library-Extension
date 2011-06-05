using System;
using System.Collections.Generic;
using PostSharp.Aspects;
using Whathecode.System.ComponentModel.NotifyPropertyFactory;
using Whathecode.System.Windows.Input.CommandFactory;


namespace Whathecode.System.Windows.Aspects.ViewModel
{
    /// <summary>
    ///   Aspect which when applied to a class, allows using the <see cref="NotifyPropertyFactory{TEnum}" />
    ///   and the <see cref="CommandFactory{T}" /> without having to add it to the class,
    ///   or delegate calls to it in the property getters and setters.
    /// </summary>
    [Serializable]
    [AttributeUsage( AttributeTargets.Class )]
    public class ViewModelAttribute : Attribute, IAspectProvider
    {
        readonly Type _propertiesEnumType;
        readonly Type _commandsEnumType;


        public ViewModelAttribute( Type propertiesEnumType, Type commandsEnumType )
        {
            _propertiesEnumType = propertiesEnumType;
            _commandsEnumType = commandsEnumType;
        }


        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            Type targetType = (Type)targetElement;

            Type genericAspect = typeof( ViewModelAspect<,> ).MakeGenericType( _propertiesEnumType, _commandsEnumType );

            yield return new AspectInstance( targetType, Activator.CreateInstance( genericAspect ) as IAspect );
        }
    }
}
