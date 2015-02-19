using System;
using System.Windows;
using PostSharp.Aspects;
using PostSharp.Reflection;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Aspects
{
	/// <summary>
	///   Aspect which is applied to properties by <see cref = "WpfControlAspect{T}" />.
	///   It creates the correct calls to the dependency property factory.
	/// </summary>
	[Serializable]
	public class DependencyPropertyAspect<T> : IInstanceScopedAspect, ILocationInterceptionAspect
	{
		readonly T _property;

		[NonSerialized]
		object _instance;


		public DependencyPropertyAspect( T property )
		{
			_property = property;
		}


		public object CreateInstance( AdviceArgs adviceArgs )
		{
			var newAspect = (DependencyPropertyAspect<T>)MemberwiseClone();
			newAspect._instance = adviceArgs.Instance;

			return newAspect;
		}

		public void RuntimeInitializeInstance()
		{
			// Nothing to do.
		}

		public void RuntimeInitialize( LocationInfo locationInfo ) {}

		public void OnGetValue( LocationInterceptionArgs args )
		{
			DependencyPropertyFactory<T> factory = WpfControlAspect<T>.PropertyFactories[ _instance.GetType() ];
			args.Value = factory.GetValue( args.Instance as DependencyObject, _property );
		}

		public void OnSetValue( LocationInterceptionArgs args )
		{
			DependencyPropertyFactory<T> factory = WpfControlAspect<T>.PropertyFactories[ _instance.GetType() ];
			factory.SetValue( args.Instance as DependencyObject, _property, args.Value );
		}
	}
}