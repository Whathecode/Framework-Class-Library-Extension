using System;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to a member,
	///   specifies a callback for a dependency property.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractDependencyPropertyCallbackAttribute : AbstractDependencyPropertyAttribute
	{
		/// <summary>
		///   The delegate type for the callback.
		/// </summary>
		public abstract Type CallbackType { get; }

		/// <summary>
		///   Create a new attribute which specifies how the dependency property should be created.
		/// </summary>
		/// <param name = "id">The ID of the dependency property.</param>
		protected AbstractDependencyPropertyCallbackAttribute( object id )
			: base( id ) {}
	}
}