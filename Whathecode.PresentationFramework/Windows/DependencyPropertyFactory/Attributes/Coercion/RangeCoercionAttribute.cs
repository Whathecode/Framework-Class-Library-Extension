using System;
using Whathecode.System.ComponentModel.Coercion;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion
{
    /// <summary>
    ///   Coerces a value to a certain range.
    /// </summary>
    public class RangeCoercionAttribute : CoercionHandlerAttribute
    {
        /// <summary>
        ///   Helper class to support creating a generic attribute.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum values which link to properties.</typeparam>
        /// <typeparam name="TValue">The type of the value to coerce.</typeparam>
        private class GenericHelper<TEnum, TValue> : RedirectedControlCoercion<TEnum, TValue>
        {
            public GenericHelper( TEnum rangeStart, TEnum rangeEnd )
                : base(
                    new RangeCoercion<RedirectedControlCoercion<TEnum, TValue>, TValue>(
                        r => (TValue)r.Values[ rangeStart ],
                        r => (TValue)r.Values[ rangeEnd ] ),
                    (TEnum)rangeStart.EnumOr( rangeEnd )
                    ) {}
        }


        /// <summary>
        ///   Create a new coercion handler which coerces values to a specified range.
        /// </summary>
        /// <param name="valueType">The type of the value to coerce.</param>
        /// <param name="rangeStart">Enum value specifying the property which indicates the start of the range.</param>
        /// <param name="rangeEnd">Enum value specifying the property which indicates the end of the range.</param>
        public RangeCoercionAttribute( Type valueType, object rangeStart, object rangeEnd )
            : base(
                typeof( GenericHelper<,> ).MakeGenericType( rangeStart.GetType(), valueType ),
                new[] { rangeStart, rangeEnd },
                new[] { rangeStart.EnumOr( rangeEnd ) } ) {}
    }
}