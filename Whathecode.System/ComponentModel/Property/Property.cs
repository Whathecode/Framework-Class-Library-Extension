using System;
using System.Diagnostics.Contracts;


namespace Whathecode.System.ComponentModel.Property
{
    /// <summary>
    ///   Class which can be used as a backing field for a property.
    ///   It supports common desired behavior when getting or setting a value.
    /// </summary>
    /// <typeparam name = "T">The type of the backing field for the property.</typeparam>
    /// <author>Steven Jeuris</author>
    public class Property<T>
    {
        public delegate void PropertyChanged( T oldValue, T newValue );


        /// <summary>
        ///   The backing field value for the property.
        /// </summary>
        T _value;


        /// <summary>
        ///   Create a new backing field for a property. Default value is the same as an ordinary field.
        /// </summary>
        public Property() {}

        /// <summary>
        ///   Create a new backing field for a property with a specified default value.
        /// </summary>
        /// <param name = "defaultValue">The default value to be used.</param>
        public Property( T defaultValue )
        {
            _value = defaultValue;
        }


        /// <summary>
        ///   Get the current value of the backing field.
        /// </summary>
        /// <returns>The current value of the backing field.</returns>
        public T GetValue()
        {
            return _value;
        }


        bool _firstSetterCall = true;

        /// <summary>
        ///   Set a new value for the backing field. The callback options can specify when a callback should be called.
        /// </summary>
        /// <param name = "newValue">The new value for the backing field.</param>
        /// <param name = "option">Option which specify when the callback should be called.</param>
        /// <param name = "changedCallback">The callback which is called, based on the callback options.</param>
        public void SetValue(
            T newValue,            
            PropertyChanged changedCallback = null,
            SetterCallbackOption option = SetterCallbackOption.OnNewValue )
        {
            Contract.Requires( Enum.IsDefined( typeof( SetterCallbackOption ), option ) );

            // Set new value.
            T oldValue = _value;
            _value = newValue;
            bool isNewValue = !Equals( oldValue, newValue );

            // Trigger callback when necessary.
            if ( changedCallback != null )
            {
                bool shouldTriggerCallback = false;
                switch ( option )
                {
                    case SetterCallbackOption.Always:
                        shouldTriggerCallback = true;
                        break;
                    case SetterCallbackOption.OnNewValue:
                        shouldTriggerCallback = isNewValue;
                        break;
                    case SetterCallbackOption.OnNewValueAndFirst:
                        shouldTriggerCallback = isNewValue || _firstSetterCall;
                        break;
                    case SetterCallbackOption.OnSameValue:
                        shouldTriggerCallback = Equals( oldValue, newValue );
                        break;
                }

                if ( shouldTriggerCallback )
                {
                    changedCallback( oldValue, newValue );
                }
            }

            // No longer first setter call.
            _firstSetterCall = false;
        }
    }
}