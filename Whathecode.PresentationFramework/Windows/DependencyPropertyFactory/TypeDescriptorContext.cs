using System;
using System.ComponentModel;
using System.Xaml;


namespace Whathecode.System.Windows.DependencyPropertyFactory
{
	class TypeDescriptorContext : ITypeDescriptorContext, IDestinationTypeProvider
	{
		readonly Type _destinationType;


		public TypeDescriptorContext( Type destinationType )
		{
			_destinationType = destinationType;
		}


		#region ITypeDescriptorContext

		public IContainer Container { get; private set; }

		public object Instance { get; private set; }

		public PropertyDescriptor PropertyDescriptor { get; private set; }

		public object GetService( Type serviceType )
		{
			if ( serviceType == typeof( IDestinationTypeProvider ) )
			{
				return this;
			}

			return null;
		}

		public bool OnComponentChanging()
		{
			throw new NotSupportedException();
		}

		public void OnComponentChanged()
		{
			throw new NotSupportedException();
		}

		#endregion // ITypeDescriptorContext


		#region IDestinationTypeProvider

		public Type GetDestinationType()
		{
			return _destinationType;
		}

		#endregion // IDestinationTypeProvider
	}
}
