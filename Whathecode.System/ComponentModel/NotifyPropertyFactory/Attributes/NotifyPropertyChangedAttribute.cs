using System;


namespace Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to an <see cref="Action{T, T}" /> method, specifies the changed callback for a notify property.
	///   The first parameter indicates the old value, the second parameter indicates the new value.
	///   It should be possible to cast the notify property to the types of the parameters.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
	public class NotifyPropertyChangedAttribute : IdAttribute
	{
		/// <summary>
		///   Create a new attribute to assign a function as changed callback to a given notify property ID.
		/// </summary>
		/// <param name = "id">The ID of the notify property.</param>
		public NotifyPropertyChangedAttribute( object id )
			: base( id ) {}
	}
}
