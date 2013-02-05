using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Experimental
{
	public class Property<T, TProperty>
	{
		public readonly Func<T, TProperty> Get;
		public readonly Func<T, TProperty, T> Set;


		public Property( Func<T, TProperty> get, Func<T, TProperty, T> set )
		{
			Get = get;
			Set = set;
		}
	}

	public class PropertyList<T, TProperty>
	{
		readonly List<Property<T, TProperty>> _properties;


		public PropertyList( IEnumerable<Property<T, TProperty>> properties )
		{
			_properties = properties.ToList();
		}


		public IEnumerable<TProperty> Get( T instance )
		{
			return _properties.Select( p => p.Get( instance ) );
		}

		public T Set( T instance, IEnumerable<TProperty> values )
		{
			using ( var sourceIterator = _properties.GetEnumerator() )
			using ( var valuesIterator = values.GetEnumerator() )
			{
				while ( sourceIterator.MoveNext() && valuesIterator.MoveNext() )
				{
					instance = sourceIterator.Current.Set( instance, valuesIterator.Current );
				}
			}

			return instance;
		}
	}

	public static class PropertyListFactory<T>
	{
		public static PropertyList<T, TProperty> Create<TProperty>( params Expression<Func<T, TProperty>>[] properties )
		{
			var accessors = properties.Select( e =>
			{
				var pointsAt = ((LambdaExpression)e).Body as MemberExpression;
				if ( pointsAt == null )
				{
					throw new InvalidImplementationException(
						"PropertyList should be initialized using delegates which point to fields or properties of the type." );
				}
				var member = typeof( T ).GetMember( pointsAt.Member.Name )[ 0 ];

				Func<T, TProperty> getter = null;
				Func<T, TProperty, T> setter = null;
				if ( member is FieldInfo )
				{
					var field = (FieldInfo)member;
					getter = field.CreateOpenInstanceGetterDelegate<T, TProperty>();
					setter = field.CreateOpenInstanceReturningSetterDelegate<T, TProperty>();
				}
				else if ( member is PropertyInfo )
				{
					var property = (PropertyInfo)member;
					getter = property.CreateOpenInstanceGetterDelegate<T, TProperty>();
					setter = property.CreateOpenInstanceReturningSetterDelegate<T, TProperty>();
				}
				else
				{
					throw new InvalidImplementationException( "The delegates should only point to fields or properties." );
				}

				return new Property<T, TProperty>( getter, setter );
			} );

			return new PropertyList<T, TProperty>( accessors );
		}
	}

	public class Swizzle<T, TProperty> : IEnumerable<TProperty>
	{
		readonly T _instance;
		PropertyList<T, TProperty> _properties;


		public Swizzle( T instance, PropertyList<T, TProperty> properties )
		{
			_instance = instance;
			_properties = properties;
		}


		public IEnumerable<TProperty> Get()
		{
			return _properties.Get( _instance );
		}

		public T Set( IEnumerable<TProperty> values )
		{
			return _properties.Set( _instance, values );
		}

		public IEnumerator<TProperty> GetEnumerator()
		{
			return Get().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
