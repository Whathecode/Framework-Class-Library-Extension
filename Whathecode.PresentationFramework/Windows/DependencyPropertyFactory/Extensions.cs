using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory
{
	public static class Extensions
	{
		static readonly Dictionary<object, Dictionary<object, DependencyProperty>> DependencyProperties
			= new Dictionary<object, Dictionary<object, DependencyProperty>>();

		public static DependencyProperty GetDependencyProperty<TControl, TProperties>( this TControl control, TProperties id )
			where TControl : UserControl
		{
			if ( !DependencyProperties.ContainsKey( control ) )
			{
				DependencyProperties[ control ] = new Dictionary<object, DependencyProperty>();			
			}
			if ( !DependencyProperties[ control ].ContainsKey( id ) )
			{
				Type idType = typeof( TProperties );
				var factories = typeof( TControl ).GetMembers( typeof( DependencyPropertyFactory<> ) );
				var factoryInfo = factories.FirstOrDefault( m => m.GetMemberType().GetGenericArguments()[ 0 ] == idType );
				if ( factoryInfo == null )
				{
					throw new InvalidOperationException(
						"The control does not contain a DependencyPropertyFactory which contains properties of type \"" + idType + "\"." );
				}

				var factory = (DependencyPropertyFactory<TProperties>)control.GetValue( factoryInfo );
				DependencyProperties[ control ][ id ] = factory.Properties[ id ];
			}

			return DependencyProperties[ control ][ id ];
		}
	}
}
