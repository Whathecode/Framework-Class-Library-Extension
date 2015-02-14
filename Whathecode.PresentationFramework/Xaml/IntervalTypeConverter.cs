using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xaml;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Reflection;


namespace Whathecode.System.Xaml
{
	/// <summary>
	///   Type converter which can be used to define <see cref="Interval{T,TSize}" />'s as strings within XAML.
	/// </summary>
	public class IntervalTypeConverter : TypeConverter
	{
		readonly Dictionary<Type, Func<string, object>> _convertFromType = new Dictionary<Type, Func<string, object>>();


		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			if ( sourceType == typeof( string ) )
			{
				return true;
			}

			return base.CanConvertFrom( context, sourceType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
		{
			string text = value as string;
			if ( text != null )
			{
				// Get target type.
				var typeProvider = (IDestinationTypeProvider)context.GetService( typeof( IDestinationTypeProvider ) );
				Type targetType = typeProvider.GetDestinationType();

				// Get parse method which can convert from string to the generic target type.
				Func<string, object> parse;
				bool createdBefore = _convertFromType.TryGetValue( targetType, out parse );
				if ( !createdBefore )
				{
					MethodInfo parseMethod = targetType.GetMethod( "Parse", ReflectionHelper.ClassMembers );
					parse = DelegateHelper.CreateDelegate<Func<string, object>>( parseMethod );
					_convertFromType[ targetType ] = parse;
				}

				return parse( text );
			}

			return base.ConvertFrom( context, culture, value );
		}
	}
}
