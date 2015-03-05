using System;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Windows.Controls.Internal
{
	/// <summary>
	///   Creates an interval using the default values for the generic type which is used to define the interval.
	/// </summary>
	class EmptyIntervalProvider : IDefaultValueProvider<AxesPanelBinding>
	{
		public object GetDefaultValue( AxesPanelBinding property, Type propertyType )
		{
			Type intervalValueType = propertyType.GenericTypeArguments[ 0 ];
			object defaultValue = intervalValueType.CreateDefault();

			return Activator.CreateInstance( propertyType, defaultValue, defaultValue );
		}
	}
}
