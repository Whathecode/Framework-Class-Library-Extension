using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Aspect which when applied to a DependencyObject, allows using the <see cref="DependencyPropertyFactory" /> without having to
    ///   add it to the class, or delegate calls to it in the property getters and setters.
    /// </summary>
    /// <typeparam name = "T">Enum type specifying all the dependency properties.</typeparam>
    [Serializable]  
    public class WpfControlAspect<T> : ITypeLevelAspect, IAspectProvider, ISerializable
    {
        public class ConcreteDependencyPropertyFactory : DependencyPropertyFactory<T>
        {
            /// <remarks>
            ///   HACK: This extended factory calls its protected constructor, disabling the need for the factory member to be static.
            ///         Since the static DependencyProperty fields can't be added yet, naming conventions are disabled as well.
            /// </remarks>
            /// <param name="ownerType"></param>
            public ConcreteDependencyPropertyFactory( Type ownerType )
                : base( ownerType, false, false )
            {            
            }
        }


        static readonly Type AttributeType = typeof( DependencyPropertyAttribute );        

        static ConcreteDependencyPropertyFactory _propertyFactory;
        const string AspectsMemberName = "_propertyAspects";
        static List<DependencyPropertyAspect<T>> _propertyAspects = new List<DependencyPropertyAspect<T>>();


        [IntroduceMember( Visibility = Visibility.Private )]
        public ConcreteDependencyPropertyFactory PropertyFactory
        {
            get { return _propertyFactory; }
            set { _propertyFactory = value; }
        }


        /// <summary>
        ///   Create a new aspect which allows generation of dependency properties when applied to a <see cref="DependencyObject" />.
        /// </summary>
        public WpfControlAspect()
        {
        }

        public WpfControlAspect( SerializationInfo info, StreamingContext context )
        {
            _propertyAspects 
                = (List<DependencyPropertyAspect<T>>)info.GetValue( AspectsMemberName, typeof( List<DependencyPropertyAspect<T>> ) );
        }


        public void RuntimeInitialize( Type type )
        {
            PropertyFactory = new ConcreteDependencyPropertyFactory( type );            

            foreach ( var propertyAspect in _propertyAspects )
            {
                propertyAspect.Factory = PropertyFactory;
            }
        }

        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            Type targetType = (Type)targetElement;

            Dictionary<MemberInfo, DependencyPropertyAttribute[]> attributedMembers
                = targetType.GetAttributedMembers<DependencyPropertyAttribute>( MemberTypes.Property );

            foreach ( var member in attributedMembers )
            {
                var attribute = member.Value[ 0 ];
                var propertyAspect = new DependencyPropertyAspect<T>( (T)attribute.GetId() );
                _propertyAspects.Add( propertyAspect );

                yield return new AspectInstance( member.Key, propertyAspect );
            }
        }

        public void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( AspectsMemberName, _propertyAspects );
        }
    }
}