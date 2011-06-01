using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes;
using Whathecode.System.Reflection;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.ComponentModel.NotifyPropertyFactory
{
    /// <summary>
    ///   A factory to simplify creating and managing properties that notify when they are changed.
    /// </summary>
    /// <typeparam name = "TEnum">An enum used to identify the properties.</typeparam>
    /// <author>Steven Jeuris</author>
    public class NotifyPropertyFactory<TEnum> : AbstractEnumSpecifiedFactory<TEnum>
    {
        /// <summary>
        ///   The owner of the properties.
        /// </summary>
        readonly object _owner;

        /// <summary>
        ///   Event handler to trigger when a property changes.
        /// </summary>
        readonly Func<PropertyChangedEventHandler> _propertyChanged;

        /// <summary>
        ///   Holds the names of all the properties.
        /// </summary>
        readonly Dictionary<TEnum, string> _names = new Dictionary<TEnum, string>();

        /// <summary>
        ///   The collection of properties created by this factory.
        /// </summary>
        readonly Dictionary<TEnum, object> _properties = new Dictionary<TEnum, object>();


        /// <summary>
        ///   Create a new factory which can create properties that notify when they are changed.
        /// </summary>
        /// <param name = "owner">The owner of the properties.</param>
        /// <param name = "propertyChanged">A function which returns the event handler to trigger when a property changes.</param>
        public NotifyPropertyFactory( object owner, Func<PropertyChangedEventHandler> propertyChanged )
            : base( owner.GetType(), true )
        {
            _owner = owner;
            _propertyChanged = propertyChanged;

            // Create properties.
            foreach ( var attribute in MatchingAttributes )
            {
                PropertyInfo property = (PropertyInfo)attribute.Key;
                TEnum id = (TEnum)attribute.Value[ 0 ].GetId();

                // Initialize property with default value.
                _properties.Add( id, property.PropertyType.CreateDefault() );
                _names.Add( id, property.Name );
            }
        }


        #region AbstractEnumSpecifiedFactory

        protected override Type[] GetAttributeTypes()
        {
            return new[] { typeof( NotifyPropertyAttribute ) };
        }

        #endregion // AbstractEnumSpecifiedFactory


        /// <summary>
        ///   Get the value for a specified property.
        /// </summary>
        /// <param name = "property">The property to get the value from.</param>
        /// <returns>The value of the property.</returns>
        public object GetValue( TEnum property )
        {
            return _properties[ property ];
        }

        /// <summary>
        ///   Set the value of a specified property.
        /// </summary>
        /// <param name = "property">The property to set the value from.</param>
        /// <param name = "value">The value for the property.</param>
        public void SetValue( TEnum property, object value )
        {
            if ( !_properties[ property ].Equals( value ) )
            {
                _properties[ property ] = value;

                PropertyChangedEventHandler handler = _propertyChanged();
                if ( handler != null )
                {
                    handler( _owner, new PropertyChangedEventArgs( _names[ property ] ) );
                }
            }
        }
    }
}