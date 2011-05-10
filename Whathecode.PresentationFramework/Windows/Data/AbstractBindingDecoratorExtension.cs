using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Whathecode.System.Windows.Markup;


namespace Whathecode.System.Windows.Data
{
    /// <summary>
    ///   A Decorator for <see cref = "Binding">System.Windows.Data.Binding</see>
    ///   which allows to add additional logic.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [MarkupExtensionReturnType( typeof( object ) )]
    public abstract class AbstractBindingDecoratorExtension : AbstractDependencyPropertyBindingExtension
    {
        readonly string _path;
        bool _dataContextHooked;

        /// <summary>
        ///   The inner binding which is decorated.
        /// </summary>
        Binding _binding;


        #region Decorator

        /// <summary>
        ///   Infrastructure. Gets or sets opaque data passed to the asynchronous data dispatcher.
        /// </summary>
        public object AsyncState
        {
            get { return _binding.AsyncState; }
            set { _binding.AsyncState = value; }
        }

        /// <summary>
        ///   Gets or sets the name of the BindingGroup to which this binding belongs.
        ///   (Inherited from BindingBase.)
        /// </summary>
        public string BindingGroupName
        {
            get { return _binding.BindingGroupName; }
            set { _binding.BindingGroupName = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to evaluate the Path relative to the data item or the DataSourceProvider object.
        /// </summary>
        public bool BindsDirectlyToSource
        {
            get { return _binding.BindsDirectlyToSource; }
            set { _binding.BindsDirectlyToSource = value; }
        }

        /// <summary>
        ///   Gets or sets the converter to use.
        /// </summary>
        public IValueConverter Converter
        {
            get { return _binding.Converter; }
            set { _binding.Converter = value; }
        }

        /// <summary>
        ///   Gets or sets the culture in which to evaluate the converter.
        /// </summary>
        public CultureInfo ConverterCulture
        {
            get { return _binding.ConverterCulture; }
            set { _binding.ConverterCulture = value; }
        }

        /// <summary>
        ///   Gets or sets the parameter to pass to the Converter.
        /// </summary>
        public object ConverterParameter
        {
            get { return _binding.ConverterParameter; }
            set { _binding.ConverterParameter = value; }
        }

        /// <summary>
        ///   Gets or sets the name of the element to use as the binding source object.
        /// </summary>
        public string ElementName
        {
            get { return _binding.ElementName; }
            set { _binding.ElementName = value; }
        }

        /// <summary>
        ///   Gets or sets the value to use when the binding is unable to return a value.
        ///   (Inherited from BindingBase.)
        /// </summary>
        public object FallbackValue
        {
            get { return _binding.FallbackValue; }
            set { _binding.FallbackValue = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether the Binding should get and set values asynchronously.
        /// </summary>
        public bool IsAsync
        {
            get { return _binding.IsAsync; }
            set { _binding.IsAsync = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether the Binding should get and set values asynchronously.
        /// </summary>
        public BindingMode Mode
        {
            get { return _binding.Mode; }
            set { _binding.Mode = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to raise the SourceUpdated event
        ///   when a value is transferred from the binding target to the binding source.
        /// </summary>
        public bool NotifyOnSourceUpdated
        {
            get { return _binding.NotifyOnSourceUpdated; }
            set { _binding.NotifyOnSourceUpdated = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to raise the TargetUpdated event
        ///   when a value is transferred from the binding source to the binding target.
        /// </summary>
        public bool NotifyOnTargetUpdated
        {
            get { return _binding.NotifyOnTargetUpdated; }
            set { _binding.NotifyOnTargetUpdated = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to raise the Error attached event on the bound object.
        /// </summary>
        public bool NotifyOnValidationError
        {
            get { return _binding.NotifyOnValidationError; }
            set { _binding.NotifyOnValidationError = value; }
        }

        /// <summary>
        ///   Gets or sets the path to the binding source property.
        /// </summary>
        public PropertyPath Path
        {
            get { return _binding.Path; }
            set { _binding.Path = value; }
        }

        /// <summary>
        ///   Gets or sets the binding source by specifying its location relative to the position of the binding target.
        /// </summary>
        public RelativeSource RelativeSource
        {
            get { return _binding.RelativeSource; }
            set { _binding.RelativeSource = value; }
        }

        /// <summary>
        ///   Gets or sets the object to use as the binding source.
        /// </summary>
        public object Source
        {
            get { return _binding.Source; }
            set { _binding.Source = value; }
        }

        /// <summary>
        ///   Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.
        ///   (Inherited from BindingBase.)
        /// </summary>
        public string StringFormat
        {
            get { return _binding.StringFormat; }
            set { _binding.StringFormat = value; }
        }

        /// <summary>
        ///   Gets or sets the value that is used in the target when the value of the source is null. (Inherited from BindingBase.)
        /// </summary>
        public object TargetNullValue
        {
            get { return _binding.TargetNullValue; }
            set { _binding.TargetNullValue = value; }
        }

        /// <summary>
        ///   Gets or sets a handler you can use to provide custom logic for handling exceptions that the binding engine encounters
        ///   during the update of the binding source value.
        ///   This is only applicable if you have associated an ExceptionValidationRule with your binding.
        /// </summary>
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
        {
            get { return _binding.UpdateSourceExceptionFilter; }
            set { _binding.UpdateSourceExceptionFilter = value; }
        }

        /// <summary>
        ///   Gets or sets a value that determines the timing of binding source updates.
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return _binding.UpdateSourceTrigger; }
            set { _binding.UpdateSourceTrigger = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to include the DataErrorValidationRule.
        /// </summary>
        public bool ValidatesOnDataErrors
        {
            get { return _binding.ValidatesOnDataErrors; }
            set { _binding.ValidatesOnDataErrors = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether to include the ExceptionValidationRule.
        /// </summary>
        public bool ValidatesOnExceptions
        {
            get { return _binding.ValidatesOnExceptions; }
            set { _binding.ValidatesOnExceptions = value; }
        }

        /// <summary>
        ///   Gets a collection of rules that check the validity of the user input.
        /// </summary>
        public Collection<ValidationRule> ValidationRules
        {
            get { return _binding.ValidationRules; }
        }

        /// <summary>
        ///   Gets or sets an XPath query that returns the value on the XML binding source to use.
        /// </summary>
        public string XPath
        {
            get { return _binding.XPath; }
            set { _binding.XPath = value; }
        }

        #endregion // Decorator


        protected AbstractBindingDecoratorExtension()
        {
            _binding = new Binding();
        }

        protected AbstractBindingDecoratorExtension( string path )
        {
            _path = path;
            _binding = new Binding( _path );
        }


        /// <summary>
        ///   Called whenever a value is being provided for a framework element.
        /// </summary>
        /// <param name = "frameworkElement">The framework element a value is being provided for.</param>
        protected abstract void ProvidingValue( FrameworkElement frameworkElement );

        protected override object ProvideValue( DependencyObject dependencyObject, DependencyProperty dependencyProperty )
        {
            FrameworkElement element = dependencyObject as FrameworkElement;
            if ( element == null )
            {
                throw new InvalidOperationException( "The BindingDecoratorExtension may only be used on framework elements." );
            }

            if ( !_dataContextHooked )
            {
                element.DataContextChanged += DataContextChanged;
                _dataContextHooked = true;
            }

            ProvidingValue( element );

            return _binding.ProvideValue( ServiceProvider );
        }

        void DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            object newContext = e.NewValue;
            _binding = _path != null ? new Binding( _path ) : new Binding();
            _binding.Source = newContext;
        }
    }
}