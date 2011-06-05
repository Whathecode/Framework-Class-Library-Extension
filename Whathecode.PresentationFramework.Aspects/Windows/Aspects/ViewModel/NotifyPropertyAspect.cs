using System;
using PostSharp.Aspects;
using PostSharp.Reflection;
using Whathecode.System.ComponentModel.NotifyPropertyFactory;


namespace Whathecode.System.Windows.Aspects.ViewModel
{
    /// <summary>
    ///   Aspect which is applied to properties by <see cref="ViewModelAspect{T,U}" />.
    ///   It creates the correct calls to the notify property factory.
    /// </summary>
    [Serializable]
    public class NotifyPropertyAspect<T> : ILocationInterceptionAspect
    {
        readonly T _property;

        [NonSerialized]
        NotifyPropertyFactory<T> _factory;
        public NotifyPropertyFactory<T> Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public NotifyPropertyAspect( T property )
        {
            _property = property;
        }


        public void RuntimeInitialize( LocationInfo locationInfo )
        {
            // TODO: Nothing to do?
        }

        public void OnGetValue( LocationInterceptionArgs args )
        {
            args.Value = Factory.GetValue( _property );
        }

        public void OnSetValue( LocationInterceptionArgs args )
        {
            Factory.SetValue( _property, args.Value );
        }
    }
}
