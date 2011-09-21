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


		public void RuntimeInitialize( LocationInfo locationInfo ) {}

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