using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Whathecode.System.ComponentModel.Coercion;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory
{
    /// <summary>
    ///   Abstract class which can coerce a given type within a given control.
    /// </summary>
    /// <typeparam name = "TContext">The context in which to coerce the value.</typeparam>
    /// <typeparam name = "TEnum">An enum used to identify the dependency properties.</typeparam>
    /// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractControlCoercion<TContext, TEnum, TValue> : AbstractCoercion<TContext, TValue>
        where TContext : DependencyObject
    {
        public TEnum DependentProperties { get; private set; }

        protected static DependencyPropertyFactory<TEnum> Factory;


        protected AbstractControlCoercion( TEnum dependentProperties )
        {
            DependentProperties = dependentProperties;
        }


        protected abstract TValue Coerce( Dictionary<TEnum, object> dependentValues, TValue value );

        public override TValue Coerce( TContext context, TValue value )
        {
            // Initialize property getters the first time.
            if ( Factory == null )
            {
                // Get the dependency property factory.
                Type controlType = typeof( TContext );
                MemberInfo propertyFactory = controlType.GetMembers( typeof( DependencyPropertyFactory<TEnum> ) ).FirstOrDefault();                

                if ( propertyFactory != null )
                {
                    Factory = (DependencyPropertyFactory<TEnum>)context.GetValue( propertyFactory );
                }
                else
                {
                    // HACK: When using aspects to create the dependency property factory, it is added as type of object.
                    //       This is due to a bug in PostSharp.
                    //       In a generic aspect which introduces generic types into a non-generic class, the added member contains
                    //       invalid type parameters.
                    //       http://www.sharpcrafters.com/forum/Topic6504-19-1.aspx
                    PropertyInfo factoryProperty = controlType.GetProperty( "PropertyFactory", BindingFlags.Instance | BindingFlags.NonPublic );
                    if ( factoryProperty != null )
                    {
                        Factory = (DependencyPropertyFactory<TEnum>)context.GetValue( factoryProperty );
                    }
                    else
                    {
                        throw new InvalidOperationException( "No dependency property factory found in type: \"" + controlType + "\"");
                    }
                }
            }

            // Pass the value of each dependent property.
            var dependentValues = EnumHelper<TEnum>.GetFlaggedValues( DependentProperties )
                .ToDictionary( p => p, p => Factory.GetValue( context, p ) );

            return Coerce( dependentValues, value );
        }
    }
}
