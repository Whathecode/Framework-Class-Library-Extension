using System;
using System.Windows;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to a PropertyChangedCallback method,
	///   specifies the coerce callback for a dependency property.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
	public class DependencyPropertyCoerceAttribute : AbstractDependencyPropertyCallbackAttribute
	{
		/// <summary>
		///   Create a new attribute to assign a function as coerce callback to a given dependency property ID.
		/// </summary>
		/// <param name = "id">The ID of the dependency property.</param>
		public DependencyPropertyCoerceAttribute( object id )
			: base( id ) {}

		/// <summary>
		///   The delegate type for this callback.
		/// </summary>
		public override Type CallbackType
		{
			get { return typeof( CoerceValueCallback ); }
		}
	}
}