namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to a member,
	///   specifies how the dependency property should be created by the DependencyPropertyFactory.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractDependencyPropertyAttribute : IdAttribute
	{
		/// <summary>
		///   Create a new dependency property attribute for a specified dependency property.
		/// </summary>
		/// <param name = "id">The ID of the dependency property.</param>
		protected AbstractDependencyPropertyAttribute( object id )
			: base( id ) {}
	}
}