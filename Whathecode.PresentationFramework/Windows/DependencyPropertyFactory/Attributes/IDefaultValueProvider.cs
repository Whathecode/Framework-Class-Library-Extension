using System;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	public interface IDefaultValueProvider<in TProperty>
	{
		object GetDefaultValue( TProperty property, Type propertyType );
	}
}
