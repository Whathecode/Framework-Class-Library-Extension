using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using ReflectionHelper = Whathecode.System.Reflection.ReflectionHelper;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Aspect which when applied to a <see cref="DependencyObject" />, allows using the <see cref="DependencyPropertyFactory" /> without having to
    ///   add it to the class, or delegate calls to it in the property getters and setters.
    /// </summary>
    /// <remarks>
    ///   This non-generic aspect was created due to a bug in PostSharp. See <see cref="WpfControlAttribute" /> for more details.
    ///   Once this is fixed, the generic class in this same source file can be used instead.
    /// </remarks>
    [Serializable]
    public class WpfControlAspect : ITypeLevelAspect, IAspectProvider, ISerializable
    {
        static readonly Type AttributeType = typeof( DependencyPropertyAttribute );        

        static readonly Dictionary<Type, object> PropertyFactories = new Dictionary<Type, object>();
        const string AspectsMemberName = "_propertyAspects";
        readonly List<DependencyPropertyAspect> _propertyAspects = new List<DependencyPropertyAspect>();
        const string EnumTypeMemberName = "_enumType";
        readonly Type _enumType;
        Type _runtimeType;


        [IntroduceMember( Visibility = Visibility.Private )]
        public object PropertyFactory
        {
            get { return PropertyFactories[ _runtimeType ]; }
            set { PropertyFactories[ _runtimeType ] = value; }
        }


        /// <summary>
        ///   Create a new aspect which allows generation of dependency properties when applied to a <see cref="DependencyObject" />.
        /// </summary>
        /// <param name = "enumType">Enum type specifying all the dependency properties.</param>
        public WpfControlAspect( Type enumType )
        {
            _enumType = enumType;
        }

        public WpfControlAspect( SerializationInfo info, StreamingContext context )
        {
            _enumType = (Type)info.GetValue( EnumTypeMemberName, typeof( Type ) );
            _propertyAspects 
                = (List<DependencyPropertyAspect>)info.GetValue( AspectsMemberName, typeof( List<DependencyPropertyAspect> ) );
        }


        public void RuntimeInitialize( Type type )
        {
            _runtimeType = type;

            // Create dependency property factory.
            // HACK: A protected constructor is called, disabling the need for the factory member to be static.
            //       Since the static DependencyProperty fields can't be added yet, naming conventions are disabled as well.
            Type factoryType = typeof( DependencyPropertyFactory<> ).MakeGenericType( _enumType );
            object factory = Activator.CreateInstance(
                factoryType,
                BindingFlags.NonPublic | BindingFlags.Instance,
                Type.DefaultBinder,
                new object[] { type, false, false },
                null );
            PropertyFactories.Add( _runtimeType, factory );

            foreach ( var propertyAspect in _propertyAspects )
            {
                propertyAspect.Factory = PropertyFactory;
            }
        }

        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            Type targetType = (Type)targetElement;

            var attributedMembers = (from member in targetType.GetMembers( ReflectionHelper.AllClassMembers )
                                     from attribute in (Attribute[])member.GetCustomAttributes( AttributeType, false )
                                     where member is PropertyInfo
                                     group attribute by member).ToDictionary( g => g.Key, g => g.ToArray() );

            foreach ( var member in attributedMembers )
            {
                var attribute = (DependencyPropertyAttribute)member.Value[ 0 ];
                var propertyAspect = new DependencyPropertyAspect( attribute.GetId() );
                _propertyAspects.Add( propertyAspect );

                yield return new AspectInstance( member.Key, propertyAspect );
            }
        }

        public void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( EnumTypeMemberName, _enumType );
            info.AddValue( AspectsMemberName, _propertyAspects );
        }
    }


    /// <summary>
    ///   Aspect which when applied to a DependencyObject, allows using the <see cref="DependencyPropertyFactory" /> without having to
    ///   add it to the class, or delegate calls to it in the property getters and setters.
    /// </summary>
    /// <remarks>
    ///   Due to a bug in PostSharp, this class causes some bugs. See <see cref="WpfControlAttribute" /> for more details.
    ///   Once this is fixed, this class should work.
    /// </remarks>
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

            var attributedMembers = (from member in targetType.GetMembers( ReflectionHelper.AllClassMembers )
                                     from attribute in (Attribute[])member.GetCustomAttributes( AttributeType, false )
                                     where member is PropertyInfo
                                     group attribute by member).ToDictionary( g => g.Key, g => g.ToArray() );

            foreach ( var member in attributedMembers )
            {
                var attribute = (DependencyPropertyAttribute)member.Value[ 0 ];
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