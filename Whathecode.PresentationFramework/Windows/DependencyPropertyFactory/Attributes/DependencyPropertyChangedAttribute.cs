using System;
using System.Windows;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to a PropertyChangedCallback method,
	///   specifies the changed callback for a dependency property.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
	public class DependencyPropertyChangedAttribute : AbstractDependencyPropertyCallbackAttribute
	{
		/// <summary>
		///   Create a new attribute to assign a function as changed callback to a given dependency property ID.
		/// </summary>
		/// <param name = "id">The ID of the dependency property.</param>
		public DependencyPropertyChangedAttribute( object id )
			: base( id ) {}

		/// <summary>
		///   The delegate type for this callback.
		/// </summary>
		public override Type CallbackType
		{
			get { return typeof( PropertyChangedCallback ); }
		}
	}
}