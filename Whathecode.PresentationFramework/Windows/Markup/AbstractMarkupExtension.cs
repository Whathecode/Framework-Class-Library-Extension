using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;


namespace Whathecode.System.Windows.Markup
{
	/// <summary>
	///   An abstract implementation for MarkupExtension which already exposes required properties.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractMarkupExtension : MarkupExtension
	{
		IXamlTypeResolver _typeResolver;

		/// <summary>
		///   The service provider used to retrieve service object.
		///   This is passed whenever ProvideValue is called.
		/// </summary>
		protected IServiceProvider ServiceProvider { get; private set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			_typeResolver = (IXamlTypeResolver)serviceProvider.GetService( typeof( IXamlTypeResolver ) );
			var targetResolver = (IProvideValueTarget)serviceProvider.GetService( typeof( IProvideValueTarget ) );

			object targetObject = targetResolver.TargetObject;
			object targetProperty = targetResolver.TargetProperty;

			if ( ServiceProvider == null )
			{
				// HACK: The service provider provided by SharedDP objects doesn't seem to work?
				ServiceProvider = serviceProvider;
			}

			if ( targetObject is DependencyObject || targetObject is Binding || targetObject is MultiBinding )
			{
				return ProvideValue( targetObject, targetProperty );
			}			

			// Required to resolve SharedDP when Binding is used in templates.
			return this;
		}

		/// <summary>
		///   Provides a value for the target.
		/// </summary>
		/// <returns>A vlue for the target.</returns>
		protected abstract object ProvideValue( object targetObject, object targetProperty );

		/// <summary>
		///   Resolves a XAML element to the corresponding CLR type.
		/// </summary>
		/// <param name = "qualifiedTypeName">
		///   The XAML type name to resolve.
		///   The type name is optionally qualified by the prefix for a XML namespace.
		///   Otherwise the current default XML namespace is assumed.
		/// </param>
		/// <returns>The corresponding CLR type.</returns>
		public Type ResolveXamlType( string qualifiedTypeName )
		{
			return _typeResolver.Resolve( qualifiedTypeName );
		}
	}
}