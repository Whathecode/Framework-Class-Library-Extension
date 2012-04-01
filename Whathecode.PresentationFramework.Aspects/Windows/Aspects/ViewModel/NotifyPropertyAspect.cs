using System;
using System.Collections.Generic;
using PostSharp.Aspects;
using PostSharp.Reflection;
using Whathecode.System.ComponentModel.NotifyPropertyFactory;


namespace Whathecode.System.Windows.Aspects.ViewModel
{
	/// <summary>
	///   Aspect which is applied to properties by <see cref = "ViewModelAspect{T,U}" />.
	///   It creates the correct calls to the notify property factory.
	/// </summary>
	[Serializable]
	public class NotifyPropertyAspect<T> : ILocationInterceptionAspect
	{
		readonly T _property;

		readonly Dictionary<object, NotifyPropertyFactory<T>> _factories = new Dictionary<object, NotifyPropertyFactory<T>>();


		public NotifyPropertyAspect( T property )
		{
			_property = property;			
		}


		public void RuntimeInitialize( LocationInfo locationInfo )
		{
			// Nothing to do.
		}

		public void SetPropertyFactory( object instance, NotifyPropertyFactory<T> factory )
		{
			_factories.Add( instance, factory );
		}

		public void OnGetValue( LocationInterceptionArgs args )
		{
			args.Value = _factories[ args.Instance ].GetValue( _property );
		}

		public void OnSetValue( LocationInterceptionArgs args )
		{
			_factories[ args.Instance ].SetValue( _property, args.Value );
		}
	}
}