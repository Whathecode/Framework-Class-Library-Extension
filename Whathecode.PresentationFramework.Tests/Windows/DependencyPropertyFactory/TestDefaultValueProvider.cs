using System;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	public class TestDefaultValueProvider : IDefaultValueProvider<Property>
	{
		public object GetDefaultValue( Property property, Type propertyType )
		{
			return 10;
		}
	}
}
