﻿using System;
using System.Windows;
using Whathecode.System.Windows.Threading;


namespace Whathecode.System.Windows.Markup
{
    /// <summary>
    ///   A MarkupExtension which allows binding to a dependency property.
    ///   This is similar to System.Windows.Data.Binding, but more abstract.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractDependencyPropertyBindingExtension : AbstractMarkupExtension
    {
        protected DependencyObject DependencyObject { get; private set; }

        protected DependencyProperty DependencyProperty { get; private set; }

        /// <summary>
        ///   Provides a value for a dependency property of a dependency object.
        /// </summary>
        /// <param name = "dependencyObject">The dependency object to set a dependency property of.</param>
        /// <param name = "dependencyProperty">The dependency property to set.</param>
        /// <returns>A value for the dependency property of the dependency object.</returns>
        protected abstract object ProvideValue(
            DependencyObject dependencyObject,
            DependencyProperty dependencyProperty
            );

        protected override object ProvideValue( object targetObject, object targetProperty )
        {
            DependencyObject = targetObject as DependencyObject;
            DependencyProperty = targetProperty as DependencyProperty;

            if ( DependencyObject == null || DependencyProperty == null )
            {
                throw new InvalidOperationException(
                    "To use DependencyPropertyBindingExtension, the target object should be a DependencyObject, " +
                    "and the target property should be a DependencyProperty."
                    );
            }

            return ProvideValue( DependencyObject, DependencyProperty );
        }

        protected void UpdateProperty( object value )
        {
            Action updateAction = () => DependencyObject.SetValue( DependencyProperty, value );

            DispatcherHelper.SafeDispatch( DependencyObject.Dispatcher, updateAction );
        }
    }
}