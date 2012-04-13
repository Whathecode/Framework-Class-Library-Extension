using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Xaml.Behaviors
{
	/// <summary>
	///   Properties which can be attached to an <see cref = "UIElement" /> to specify behavior applied to its properties.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class PropertyBehavior
	{
		[Flags]
		enum Properties
		{
			UpdateSourceOnEnter
		}


		static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

		/// <summary>
		///   Updates the source of the binding when enter is pressed on the <see cref = "UIElement" />.
		/// </summary>
		public static readonly DependencyProperty UpdateSourceOnEnter = DependencyProperties[ Properties.UpdateSourceOnEnter ];

		[DependencyProperty( Properties.UpdateSourceOnEnter )]
		public static DependencyProperty GetUpdateSourceOnEnter( UIElement target )
		{
			return (DependencyProperty)DependencyProperties.GetValue( target, Properties.UpdateSourceOnEnter );
		}

		[DependencyProperty( Properties.UpdateSourceOnEnter )]
		public static void SetUpdateSourceOnEnter( UIElement target, DependencyProperty value )
		{
			DependencyProperties.SetValue( target, Properties.UpdateSourceOnEnter, value );
		}

		[DependencyPropertyChanged( Properties.UpdateSourceOnEnter )]
		static void OnUpdateSourceOnEnterChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			UIElement element = (UIElement)d;
			if ( e.OldValue != null )
			{
				element.PreviewKeyDown -= HandlePreviewKeyDown;
			}
			if ( e.NewValue != null )
			{
				element.PreviewKeyDown += HandlePreviewKeyDown;
			}
		}

		static void HandlePreviewKeyDown( object sender, KeyEventArgs e )
		{
			UIElement element = (UIElement)e.Source;

			if ( e.Key == Key.Enter )
			{
				DependencyProperty property = GetUpdateSourceOnEnter( element );
				BindingExpression binding = BindingOperations.GetBindingExpression( element, property );
				if ( binding != null )
				{
					binding.UpdateSource();
				}
			}
		}
	}
}
