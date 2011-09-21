using System.Windows;


namespace Whathecode.System.Windows.Markup
{
	/// <summary>
	///   A MarkupExtension which allows binding from the DataContext of a FrameworkElement
	///   to a dependency property.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractDataContextBindingExtension : AbstractDependencyPropertyBindingExtension
	{
		FrameworkElement _frameworkElement;
		bool _dataContextChangedHooked;


		~AbstractDataContextBindingExtension()
		{
			_frameworkElement.DataContextChanged -= DataContextChanged;
		}


		/// <summary>
		///   Provide a value from a given data context.
		/// </summary>
		/// <param name = "dataContext">The data context.</param>
		/// <returns>A value from the data context.</returns>
		protected abstract object ProvideValue( object dataContext );

		protected override object ProvideValue(
			DependencyObject dependencyObject,
			DependencyProperty dependencyProperty )
		{
			_frameworkElement = dependencyObject as FrameworkElement;
			if ( _frameworkElement == null )
			{
				throw new InvalidImplementationException( "The DataContextBinding may only be used on framework elements." );
			}

			if ( !_dataContextChangedHooked )
			{
				_frameworkElement.DataContextChanged += DataContextChanged;
				_dataContextChangedHooked = true;
			}

			return ProvideValue( _frameworkElement.DataContext );
		}

		void DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
		{
			// When the data context changes, get the new value.
			UpdateProperty( ProvideValue( _frameworkElement.DataContext ) );
		}
	}
}