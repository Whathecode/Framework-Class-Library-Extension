using System.Windows;
using System.Xaml;


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
			if ( _dataContextChangedHooked )
			{
				_frameworkElement.DataContextChanged -= DataContextChanged;
			}
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
			// Attempt to find a FrameworkElement from which the DataContext can be accessed.
			if ( dependencyObject is FrameworkElement )
			{
				_frameworkElement = dependencyObject as FrameworkElement;
			}
			else
			{
				// Use the root object in case the DependencyObject isn't a FrameworkElement. (e.g. Freezable)
				// TODO: This might be an overly simplistic implementation. What about custom DataContext's lower in the tree?				
				var rootProvider = (IRootObjectProvider)ServiceProvider.GetService( typeof( IRootObjectProvider ) );
				_frameworkElement = rootProvider.RootObject as FrameworkElement;
			}			
			if ( _frameworkElement == null )
			{
				throw new InvalidImplementationException(
					"The DataContextBinding may only be used in a context where DataContext can be obtained." );
			}

			// Listen to DataContext changes.
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