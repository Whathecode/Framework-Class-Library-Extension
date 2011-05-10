using System;


namespace Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes
{
    /// <summary>
    ///   When applied to a property,
    ///   specifies that the property should be a property which notifies when it is changed.
    ///   Used by the NotifyPropertyFactory.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class NotifyPropertyAttribute : IdAttribute
    {
        /// <summary>
        ///   Create a new notify property attribute for a certain property.
        /// </summary>
        /// <param name = "id">The enum ID of the property.</param>
        public NotifyPropertyAttribute( object id )
            : base( id ) {}
    }
}