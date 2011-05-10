using System.Windows;
using System.Windows.Data;


namespace Whathecode.System.Windows.Data
{
    /// <summary>
    ///   A binding markup extension with added functionality.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public class AdvancedBindingExtension : AbstractBindingDecoratorExtension
    {
        FrameworkElement _frameworkElement;
        bool _prevNotifyOnTargetUpdated;

        /// <summary>
        ///   When set to true, forces to call the changed callback the first time the property is set,
        ///   even when the value equals the default value.
        /// </summary>
        public bool ForceChangeCallbackFirstTime { get; set; }


        public AdvancedBindingExtension( string path )
            : base( path ) {}


        protected override void ProvidingValue( FrameworkElement element )
        {
            if ( ForceChangeCallbackFirstTime )
            {
                // Save data to work with when target is updated.
                _frameworkElement = element;
                _prevNotifyOnTargetUpdated = NotifyOnTargetUpdated;

                // Prepare to get target updates. (Including the first one.)
                NotifyOnTargetUpdated = true;
                element.TargetUpdated += TargetUpdated;
            }
        }

        void TargetUpdated( object sender, DataTransferEventArgs e )
        {
            // Trigger a property changed callback as long as the new value equals the default value.
            object current = DependencyObject.GetValue( DependencyProperty );
            PropertyMetadata metaData = DependencyProperty.GetMetadata( DependencyObject );
            bool equals = current == null ? current == metaData.DefaultValue : current.Equals( metaData.DefaultValue );
            if ( equals )
            {
                if ( metaData.PropertyChangedCallback != null )
                {
                    metaData.PropertyChangedCallback.Invoke(
                        DependencyObject,
                        new DependencyPropertyChangedEventArgs(
                            DependencyProperty,
                            current,
                            current ) );
                }
            }
            else
            {
                // Once it is no longer the default value, we know it has been changed.
                _frameworkElement.TargetUpdated -= TargetUpdated;
                NotifyOnTargetUpdated = _prevNotifyOnTargetUpdated;
            }
        }
    }
}