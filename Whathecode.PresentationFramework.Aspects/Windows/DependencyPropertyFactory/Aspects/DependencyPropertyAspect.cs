using System;
using System.Reflection;
using System.Windows;
using PostSharp.Aspects;
using PostSharp.Reflection;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Aspect which is applied to properties by <see cref="WpfControlAspect" />.
    ///   It creates the correct calls to the dependency property factory.
    /// </summary>
    [Serializable]
    public class DependencyPropertyAspect : ILocationInterceptionAspect
    {
        const string FactoryGetValueMethod = "GetValue";
        const string FactorySetValueMethod = "SetValue";

        readonly object _property;

        [NonSerialized]
        private Func<DependencyObject, object, object> _getter;
        [NonSerialized]
        Action<DependencyObject, object, object> _setter;

        [NonSerialized]
        object _factory;
        public object Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public DependencyPropertyAspect( object property )
        {
            _property = property;
        }


        public void RuntimeInitialize( LocationInfo locationInfo )
        {
            // TODO: Nothing to do?
        }

        public void OnGetValue( LocationInterceptionArgs args )
        {
            if ( _getter == null )
            {
                MethodInfo getter = Factory.GetType().GetMethod( FactoryGetValueMethod );
                _getter = getter.CreateDelegate<Func<DependencyObject, object, object>>( Factory, DelegateHelper.CreateOptions.Upcasting );
            }           
           
            args.Value = _getter( args.Instance as DependencyObject, _property );
        }

        public void OnSetValue( LocationInterceptionArgs args )
        {
            if ( _setter == null )
            {
                MethodInfo setter = Factory.GetType().GetMethod( FactorySetValueMethod );
                _setter = setter.CreateDelegate<Action<DependencyObject, object, object>>( Factory, DelegateHelper.CreateOptions.Upcasting );
            }

            _setter( args.Instance as DependencyObject, _property, args.Value );
        }
    }


    /// <summary>
    ///   Aspect which is applied to properties by <see cref="WpfControlAspect" />.
    ///   It creates the correct calls to the dependency property factory.
    /// </summary>
    [Serializable]
    public class DependencyPropertyAspect<T> : ILocationInterceptionAspect
    {
        readonly T _property;

        [NonSerialized]
        DependencyPropertyFactory<T> _factory;
        public DependencyPropertyFactory<T> Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public DependencyPropertyAspect( T property )
        {
            _property = property;
        }


        public void RuntimeInitialize( LocationInfo locationInfo )
        {
            // TODO: Nothing to do?
        }

        public void OnGetValue( LocationInterceptionArgs args )
        {
            args.Value = Factory.GetValue( args.Instance as DependencyObject, _property );
        }

        public void OnSetValue( LocationInterceptionArgs args )
        {
            Factory.SetValue( args.Instance as DependencyObject, _property, args.Value );
        }
    }
}
