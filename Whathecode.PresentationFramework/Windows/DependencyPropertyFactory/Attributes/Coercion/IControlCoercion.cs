namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion
{
	/// <summary>
	///   Interface which can coerce a given type within a control.
	/// </summary>
	/// <typeparam name = "TEnum">An enum used to identify the dependency properties.</typeparam>
	/// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
	/// <author>Steven Jeuris</author>
	public interface IControlCoercion<out TEnum, TValue>
	{
		TEnum DependentProperties { get; }
		TValue Coerce( object context, TValue value );
	}
}