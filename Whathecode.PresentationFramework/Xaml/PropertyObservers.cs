using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Xaml
{
    /// <summary>
    ///   Describes an attached property which can be used to bind observers to properties of the object to which it is attached.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    ///   Example usage in XAML:
    /// 
    ///     <SomeControl>
    ///         <Xaml:PropertyObservers.Observers>
    ///             <Xaml:PropertyObserver PropertyName="ActualWidth"
    ///                                    Observer="{Binding Width, Mode=OneWayToSource}" />
    ///         </Xaml:PropertyObservers.Observers>
    ///     </SomeControl>
    /// ]]>
    /// </remarks>
    /// <author>Steven Jeuris</author>
    public static class PropertyObservers
    {
        enum Properties
        {
            Observers
        }


        static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

        public static DependencyProperty ObserversProperty = DependencyProperties[ Properties.Observers ];


        [DependencyProperty( Properties.Observers )]
        public static List<PropertyObserver> GetObservers( DependencyObject target )
        {
            return DependencyProperties.GetValue( target, Properties.Observers ) as List<PropertyObserver>;
        }

        [DependencyProperty( Properties.Observers )]
        public static void SetObservers( DependencyObject target, List<PropertyObserver> value )
        {
            DependencyProperties.SetValue( target, Properties.Observers, value );            
        }

        [DependencyPropertyChanged( Properties.Observers )]
        public static void OnObserversChanged( object sender, DependencyPropertyChangedEventArgs args )
        {
            // Check when the data context for the element of this attached property changed.
            // When this happens, the list is initialized.
            FrameworkElement element = sender as FrameworkElement;
            if ( element != null )
            {
                element.DataContextChanged += ElementDataContextChanged;
            }
        }

        static void ElementDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            DependencyObject element = sender as DependencyObject;
            if ( element != null )
            {
                // Initialize observers.
                List<PropertyObserver> observers = GetObservers( element );
                foreach ( var o in observers )
                {
                    // Find property to observe.
                    if ( !(o.PropertyName is string) )
                    {
                        throw new ArgumentException( "PropertyName should be a string representing the name of a property to observe." );
                    }
                    string propertyName = (string)o.PropertyName;
                    Type senderType = sender.GetType();
                    DependencyPropertyDescriptor property = DependencyPropertyDescriptor.FromName( propertyName, senderType, senderType );
                    if ( property == null )
                    {
                        throw new ArgumentException( "Property \"" + propertyName + "\" not defined in type \"" + senderType + "\"." );
                    }

                    // Listen to changes of the specified dependency property and forward to observer.
                    PropertyObserver observer = o;
                    property.AddValueChanged(sender, delegate
                    {
                        observer.Observer = property.GetValue( sender );
                    });

                    // Set data context so binding works.
                    o.DataContext = e.NewValue;
                }
            }
        }
    }
}