using System;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	/// <summary>
	///   Enum specifying all the dependency properties to be managed by the factory.
	/// </summary>
	[Flags]
	public enum Property
	{
		Standard,
		/// <summary>
		///   This should become a read only property.
		/// </summary>
		ReadOnly,
		/// <summary>
		///   This property should have callbacks assigned to it.
		/// </summary>
		Callback,
		Minimum,
		Maximum
	}
}