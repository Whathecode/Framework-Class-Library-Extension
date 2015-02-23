using System;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	/// <summary>
	///   Enum specifying all the dependency properties to be managed by the factory.
	/// </summary>
	[Flags]
	public enum Property
	{
		Standard = 1,
		/// <summary>
		///   This should become a read only property.
		/// </summary>
		ReadOnly = 1 << 1,
		/// <summary>
		///   This property should have callbacks assigned to it.
		/// </summary>
		Callback = 1 << 2,
		Minimum = 1 << 3,
		Maximum = 1 << 4,
		DefaultValueProvider = 1 << 5
	}
}