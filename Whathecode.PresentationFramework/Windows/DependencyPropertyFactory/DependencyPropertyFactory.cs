using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using Whathecode.System.Reflection;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators;
using Whathecode.System.Windows.Threading;
using Whathecode.System.Linq;


namespace Whathecode.System.Windows.DependencyPropertyFactory
{
	/// <summary>
	///   A dependency property factory to simplify creating and managing dependency properties for a certain type.
	/// </summary>
	/// <typeparam name = "T">A flags enum used to identify the dependency properties.</typeparam>
	/// <author>Steven Jeuris</author>
	public class DependencyPropertyFactory<T> : AbstractEnumSpecifiedFactory<T>
	{
		class DependencyPropertyInfo
		{
			public bool IsAttached;
			public string Name;
			public Type Type;
			public object DefaultValue;
			public bool ReadOnly;
			public T Id;
		}


		const string ConventionEnabledError = "WPF dependency property conventions are enabled and not followed. ";
		const string IdentifierSuffix = "Property";
		const string GetPrefix = "Get";
		const string SetPrefix = "Set";
		readonly bool _enforceWpfConvention = true;

		readonly Dictionary<T, DependencyPropertyKey> _readOnlyProperties = new Dictionary<T, DependencyPropertyKey>();

		/// <summary>
		///   All attached properties collections. They need to be initialized upon first retrieval.
		/// </summary>
		readonly Dictionary<T, Type> _attachedCollectionsTypes = new Dictionary<T, Type>();

		/// <summary>
		///   A list containing the dependency properties.
		///   TODO: Make this a readonly collection. Will be available in .NET 4.5.
		/// </summary>
		public Dictionary<T, DependencyProperty> Properties { get; private set; }


		/// <summary>
		///   Create a new dependency property factory for a specific set of properties.
		///   When naming conventions aren't followed, an exception is thrown.
		///   (http://msdn.microsoft.com/en-us/library/bb613563.aspx)
		///   The type which encloses the type parameter is used as the owner type.
		/// </summary>
		public DependencyPropertyFactory()
			: this( true ) {}

		/// <summary>
		///   Create a new dependency property factory for a specific set of properties.
		///   The type which encloses the type parameter is used as the owner type.
		/// </summary>
		/// <param name = "enforceNamingConventions">
		///   Whether or not to throw exceptions when the naming conventions aren't followed.
		///   See http://msdn.microsoft.com/en-us/library/bb613563.aspx.
		/// </param>
		public DependencyPropertyFactory( bool enforceNamingConventions )
			: this( null, enforceNamingConventions ) {}

		/// <summary>
		///   Create a new dependency property factory for a specific set of properties.
		///   When naming conventions aren't followed, an exception is thrown.
		///   (http://msdn.microsoft.com/en-us/library/bb613563.aspx)
		/// </summary>
		/// <param name = "ownerType">The owner type of the dependency properties.</param>
		public DependencyPropertyFactory( Type ownerType )
			: this( ownerType, true ) {}

		/// <summary>
		///   Create a new dependency property factory for a specific set of properties.
		/// </summary>
		/// <param name = "ownerType">The owner type of the dependency properties.</param>
		/// <param name = "enforceNamingConventions">
		///   Whether or not to throw exceptions when the naming conventions aren't followed.
		///   See http://msdn.microsoft.com/en-us/library/bb613563.aspx.
		/// </param>
		public DependencyPropertyFactory( Type ownerType, bool enforceNamingConventions )
			: this( ownerType, enforceNamingConventions, true ) {}

		/// <summary>
		///   Create a new dependency property factory for a specific set of properties.
		/// </summary>
		/// <param name = "ownerType">The owner type of the dependency properties.</param>
		/// <param name = "enforceNamingConventions">
		///   Whether or not to throw exceptions when the naming conventions aren't followed.
		///   See http://msdn.microsoft.com/en-us/library/bb613563.aspx.
		/// </param>
		/// <param name = "checkForStatic">Whether or not to check whether the factory is static.</param>
		protected DependencyPropertyFactory( Type ownerType, bool enforceNamingConventions, bool checkForStatic )
			: base( ownerType, true )
		{
			Contract.Requires( typeof( T ).IsFlagsEnum() && (typeof( T ).IsPublic || typeof( T ).IsNestedPublic) );

			Properties = new Dictionary<T, DependencyProperty>();
			_enforceWpfConvention = enforceNamingConventions;

			// Check whether the factory itself is defined as a static field.
			// TODO: Can part of this logic be moved to ReflectionHelper?
			if ( checkForStatic )
			{
				FieldInfo[] fields = OwnerType.GetFields(
					BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );
				bool validFactory
					= fields
						.Where( field => field.FieldType == typeof( DependencyPropertyFactory<T> ) && field.IsStatic )
						.Any();
				if ( !validFactory )
				{
					throw new InvalidImplementationException(
						"Incorrect usage of DependencyPropertyFactory in class '" + OwnerType.Name + "'. " +
							"A DependencyPropertyFactory needs to be created as a static field inside it's owner class." );
				}
			}

			// Check whether callback attributes are applied to non-static functions. They should be static!
			var callbackMethods =
				from method in OwnerType.GetMethods( ReflectionHelper.FlattenedClassMembers )
				from attribute in method.GetAttributes<AbstractDependencyPropertyCallbackAttribute>()
				select method;
			if ( !callbackMethods.All( m => m.IsStatic ) )
			{
				throw new InvalidImplementationException(
					"Not all dependency property callback functions are defined as static in type '" + OwnerType.Name + "'." );
			}

			// Create dependency properties.
			var properties =
				from item in MatchingAttributes
				where item.Key is PropertyInfo
				select item;
			foreach ( var item in properties )
			{
				var attribute = (DependencyPropertyAttribute)item.Value[ 0 ];
				CreateDependencyProperty( GetDependencyPropertyInfo( item.Key as PropertyInfo, attribute ) );
			}

			// Create attached properties.
			var attachedProperties =
				from item in MatchingAttributes
				where item.Key is MethodInfo
				from attribute in item.Value
				group new
				{
					MemberInfo = item.Key,
					Attribute = (DependencyPropertyAttribute)attribute
				} by attribute.GetId();
			foreach ( var item in attachedProperties )
			{
				var attachedProperty = item.ToList();
				bool valid = false;
				if ( attachedProperty.Count <= 2 )
				{
					
					MethodInfo[] methods = attachedProperty.Select( p => (MethodInfo)p.MemberInfo ).ToArray();
					MethodInfo getter = methods.Single( m => m.Name.StartsWith( GetPrefix ) );
					MethodInfo setter = methods.SingleOrDefault( m => m.Name.StartsWith( SetPrefix ) && m.ReturnType == typeof( void ) );

					// Verify whether getter and setter correspond.
					// TODO: Do typechecking of parameters?
					if ( setter != null && getter.Name.Substring( GetPrefix.Length ) == setter.Name.Substring( SetPrefix.Length ) )
					{
						// Extract settings.
						var attributes = attachedProperty.Select( a => a.Attribute );
						var id = (T)attachedProperty.First().Attribute.GetId();						
						// TODO: Allow checking whether or not the default value is set or not.
						var defaultValues =
							attributes.Where( a => a.DefaultValue != null ).Select( a => a.DefaultValue );
						var readOnlySettings =
							attributes.Where( a => a.IsReadOnlySet() ).Select( a => a.IsReadOnlySet() ? a.IsReadOnly() : new bool?() );
						if ( !defaultValues.AllEqual() || !readOnlySettings.AllEqual() )
						{
							throw new InvalidImplementationException(
								"All set options of \"" + typeof( DependencyPropertyAttribute )  +
								"\" should correspond with attributes with the same ID." +
								" Preferably only set options on one of the attributes." );
						}

						DependencyPropertyInfo info = GetAttachedDependencyPropertyInfo(
							getter, setter,
							id, defaultValues.FirstOrDefault(), readOnlySettings.FirstOrDefault() );

						// HACK: For collections, use a dependency property with non-matching name so the getter is called the first time,
						//       so it can be initialized and doesn't need to be initialized through XAML.
						if ( info.Type.ImplementsInterface( typeof( ICollection ) ) )
						{
							info.Name = info.Name + "Internal";
							_attachedCollectionsTypes.Add( info.Id, info.Type );
						}

						CreateDependencyProperty( info );
						valid = true;
					}
				}

				if ( !valid )
				{
					throw new InvalidImplementationException(
						"Invalid usage of the attribute " + typeof( DependencyPropertyAttribute ) + ". " +
							"To create an attached property, apply it to a correctly named get and optionally corresponding set method. " );
				}
			}
		}

		DependencyPropertyInfo GetDependencyPropertyInfo( PropertyInfo property, DependencyPropertyAttribute attribute )
		{
			if ( _enforceWpfConvention )
			{
				// Find public dependency property field identifier.                    
				const BindingFlags identifierModifiers = BindingFlags.Public | BindingFlags.Static;
				string identifierField = property.Name + IdentifierSuffix;
				FieldInfo identifier = OwnerType.GetField( identifierField, identifierModifiers );
				if ( identifier == null || (identifier.FieldType != typeof( DependencyProperty )) )
				{
					throw new InvalidImplementationException(
						ConventionEnabledError +
							"There is no public static dependency property field identifier \"" + identifierField +
								"\" available in the class \"" + OwnerType.Name + "\"." );
				}

				// Verify name when set.                    
				if ( attribute.Name != null && property.Name != attribute.Name )
				{
					throw new InvalidImplementationException(
						ConventionEnabledError + "The CLR property wrapper '" + property.Name +
							"' doesn't match the name of the dependency property." );
				}
			}

			// Set dependency property parameters.
			Type propertyType = property.PropertyType;
			MethodInfo setMethod = property.GetSetMethod();
			var dependencyPropertyInfo = new DependencyPropertyInfo
			{
				IsAttached = false,
				Name = attribute.Name ?? property.Name,
				Type = propertyType,
				// When no default value is set, use the default value.
				DefaultValue = attribute.DefaultValue ?? propertyType.CreateDefault(),
				// By default, readonly when setter is private.
				ReadOnly = attribute.IsReadOnlySet() ? attribute.IsReadOnly() : (setMethod == null || setMethod.IsPrivate),
				Id = (T)attribute.GetId()
			};

			return dependencyPropertyInfo;
		}

		static DependencyPropertyInfo GetAttachedDependencyPropertyInfo(
			MethodInfo getter, MethodInfo setter,
			T id, object defaultValue, bool? isReadOnly )
		{
			// TODO: Enforce naming conventions.

			Type propertyType = getter.ReturnType;
			return new DependencyPropertyInfo
			{
				IsAttached = true,
				Name = getter.Name.Substring( GetPrefix.Length ),
				Type = propertyType,
				// When no default value is set, use the default value.
				DefaultValue = defaultValue ?? propertyType.CreateDefault(),
				// By default, readonly when no setter available.
				ReadOnly = isReadOnly ?? setter == null,
				Id = id
			};
		}

		void CreateDependencyProperty( DependencyPropertyInfo info )
		{
			// Find callbacks.
			var changedCallback = (PropertyChangedCallback)CreateCallbackDelegate<DependencyPropertyChangedAttribute>( info.Id );
			var coerceCallback = (CoerceValueCallback)CreateCallbackDelegate<DependencyPropertyCoerceAttribute>( info.Id );
			var validateValueCallback = (ValidateValueCallback)CreateCallbackDelegate<DependencyPropertyValidateAttribute>( info.Id );

			// Find specified handlers through the handler attributes when no callback found yet.
			// TODO: At the moment no handlers for attached properties are implemented.
			if ( !info.IsAttached )
			{
				PropertyInfo property = OwnerType.GetProperty( info.Name );
				if ( validateValueCallback == null )
				{
					ValidationHandlerAttribute handler = property.GetAttributes<ValidationHandlerAttribute>().FirstOrDefault();
					if ( handler != null )
					{
						validateValueCallback = handler.GenericValidation.IsValid;
					}
				}
				if ( coerceCallback == null )
				{
					CoercionHandlerAttribute handler = property.GetAttributes<CoercionHandlerAttribute>().FirstOrDefault();
					if ( handler != null )
					{
						coerceCallback = handler.GenericCoercion.Coerce;
					}
				}
			}

			// Create a correct changed callback when no custom callback specified.
			if ( changedCallback == null )
			{
				var dependentProperties = (
					from a in MatchingAttributes
					let coercion = a.Key.GetAttributes<CoercionHandlerAttribute>().FirstOrDefault()
					where coercion != null
					where EnumHelper<T>
						.GetFlaggedValues( (T)coercion.GenericCoercion.DependentProperties )
						.Contains( info.Id ) // Id of this property.
					select (T)a.Key.GetAttributes<DependencyPropertyAttribute>().First().GetId()
					).ToArray();
				if ( dependentProperties.Length > 0 )
				{
					changedCallback = ( dependencyObject, changedArgs ) =>
					{
						foreach ( var p in dependentProperties )
						{
							dependencyObject.CoerceValue( Properties[ p ] );
						}
					};
				}
			}

			// Create property.
			if ( info.ReadOnly )
			{
				CreateReadOnlyDependencyProperty(
					info.IsAttached,
					info.Id, info.Name, info.Type, info.DefaultValue,
					changedCallback, coerceCallback, validateValueCallback );
			}
			else
			{
				CreateDependencyProperty(
					info.IsAttached,
					info.Id, info.Name, info.Type, info.DefaultValue,
					changedCallback, coerceCallback, validateValueCallback );
			}
		}

		/// <summary>
		///   Create a callback delegate for a dependency property.
		/// </summary>
		/// <typeparam name = "TCallbackAttribute">The callback attribute type.</typeparam>
		/// <param name = "id">The ID of the dependency property.</param>
		/// <returns>A delegate which can be used as a callback.</returns>
		Delegate CreateCallbackDelegate<TCallbackAttribute>( object id ) where TCallbackAttribute : AbstractDependencyPropertyCallbackAttribute
		{
			const BindingFlags allStaticMethods =
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static;

			return (
				from method in OwnerType.GetMethods( allStaticMethods )
				let methodAttributes = method.GetAttributes<TCallbackAttribute>()
				where methodAttributes != null && methodAttributes.Length == 1
				let changed = methodAttributes[ 0 ]
				where changed.GetId().Equals( id )
				select Delegate.CreateDelegate( changed.CallbackType, method )
				).FirstOrDefault();
		}

		void CreateDependencyProperty(
			bool isAttached,
			T identifier,
			string name,
			Type propertyType,
			object defaultValue,
			PropertyChangedCallback changedCallback,
			CoerceValueCallback coerceCallback,
			ValidateValueCallback validateValueCallback )
		{
			DependencyProperty property;
			if ( isAttached )
			{
				property = DependencyProperty.RegisterAttached(
					name,
					propertyType,
					OwnerType,
					new PropertyMetadata( defaultValue, changedCallback, coerceCallback ),
					validateValueCallback );
			}
			else
			{
				property = DependencyProperty.Register(
					name,
					propertyType,
					OwnerType,
					new PropertyMetadata( defaultValue, changedCallback, coerceCallback ),
					validateValueCallback );
			}

			Properties.Add( identifier, property );
		}

		void CreateReadOnlyDependencyProperty(
			bool isAttached,
			T identifier,
			string name,
			Type propertyType,
			object defaultValue,
			PropertyChangedCallback changedCallback,
			CoerceValueCallback coerceCallback,
			ValidateValueCallback validateValueCallback )
		{
			DependencyPropertyKey propertyKey;
			if ( isAttached )
			{
				propertyKey = DependencyProperty.RegisterAttachedReadOnly(
					name,
					propertyType,
					OwnerType,
					new PropertyMetadata( defaultValue, changedCallback, coerceCallback ),
					validateValueCallback );
			}
			else
			{
				propertyKey = DependencyProperty.RegisterReadOnly(
					name,
					propertyType,
					OwnerType,
					new PropertyMetadata( defaultValue, changedCallback, coerceCallback ),
					validateValueCallback );
			}
			_readOnlyProperties.Add( identifier, propertyKey );

			DependencyProperty property = propertyKey.DependencyProperty;

			Properties.Add( identifier, property );
		}

		/// <summary>
		///   Returns the dependency property for a given ID.
		/// </summary>
		/// <param name = "id">The ID of the dependency propert. (enum type of the class type parameter)</param>
		/// <returns>The dependency property for the given ID.</returns>
		public DependencyProperty this[ T id ]
		{
			get
			{
				try
				{
					return Properties[ id ];
				}
				catch ( KeyNotFoundException )
				{
					throw new KeyNotFoundException(
						"The dependency property with the key \"" + id + "\" doesn't exist. "
							+ "Did you forget to add a DependencyPropertyAttribute?" );
				}
			}
		}

		/// <summary>
		///   Get the value of a property.
		/// </summary>
		/// <param name = "o">The dependency object from which to get the value.</param>
		/// <param name = "property">The property to get the value from.</param>
		/// <returns>The value from the asked property.</returns>
		public object GetValue( DependencyObject o, T property )
		{
			if ( _attachedCollectionsTypes.ContainsKey( property ) )
			{
				// Check whether collection needs to be initialized.
				object value = o.GetValue( Properties[ property ] );
				if ( value == null )
				{
					value = Activator.CreateInstance( _attachedCollectionsTypes[ property ] );
					SetValue( o, property, value );
				}

				return value;
			}

			return o.GetValue( Properties[ property ] );
		}

		/// <summary>
		///   Set the value of a property, whether it is readonly or not.
		/// </summary>
		/// <param name = "o">The dependency object on which to set the value.</param>
		/// <param name = "property">The property to set.</param>
		/// <param name = "value">The new value for the property.</param>
		public void SetValue( DependencyObject o, T property, object value )
		{
			DependencyPropertyKey key;
			if ( _readOnlyProperties.TryGetValue( property, out key ) )
			{
				DispatcherHelper.SafeDispatch( o.Dispatcher, () => o.SetValue( key, value ) );
			}
			else
			{
				DispatcherHelper.SafeDispatch( o.Dispatcher, () => o.SetValue( Properties[ property ], value ) );
			}
		}

		protected override Type[] GetAttributeTypes()
		{
			return new[] { typeof( DependencyPropertyAttribute ) };
		}
	}
}