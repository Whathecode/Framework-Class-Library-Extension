using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using Whathecode.System.ComponentModel.Coercion;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion
{
	/// <summary>
	///   Abstract class which can coerce a given type within a given control.
	/// </summary>
	/// <typeparam name = "TEnum">An enum used to identify the dependency properties.</typeparam>
	/// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
	/// <author>Steven Jeuris</author>
	[ContractClass( typeof( AbstractControlCoercionContract<,> ) )]
	public abstract class AbstractControlCoercion<TEnum, TValue> : AbstractCoercion<object, TValue>, IControlCoercion<TEnum, TValue>
	{
		public TEnum DependentProperties { get; private set; }

		protected static DependencyPropertyFactory<TEnum> Factory;


		protected AbstractControlCoercion( TEnum dependentProperties )
		{
			DependentProperties = dependentProperties;
		}


		protected abstract TValue Coerce( Dictionary<TEnum, object> dependentValues, TValue value );

		public override TValue Coerce( object context, TValue value )
		{
			// Initialize property getters the first time.
			if ( Factory == null )
			{
				// Get the dependency property factory.
				Type controlType = context.GetType();
				MemberInfo propertyFactory = controlType.GetMembers( typeof( DependencyPropertyFactory<TEnum> ) ).FirstOrDefault();

				if ( propertyFactory == null )
				{
					throw new InvalidImplementationException( "No dependency property factory found in type: \"" + controlType + "\"" );					
				}

				Factory = (DependencyPropertyFactory<TEnum>)context.GetValue( propertyFactory );
			}

			// Pass the value of each dependent property.
			var dependentValues = EnumHelper<TEnum>.GetFlaggedValues( DependentProperties )
				.ToDictionary( p => p, p => Factory.GetValue( context as DependencyObject, p ) );

			return Coerce( dependentValues, value );
		}
	}


	[ContractClassFor( typeof( AbstractControlCoercion<,> ) )]
	abstract class AbstractControlCoercionContract<TEnum, TValue> : AbstractControlCoercion<TEnum, TValue>
	{
		protected AbstractControlCoercionContract( TEnum dependentProperties )
			: base( dependentProperties ) {}


		protected override TValue Coerce( Dictionary<TEnum, object> dependentValues, TValue value )
		{
			Contract.Requires( dependentValues != null );

			return default(TValue);
		}
	}
}