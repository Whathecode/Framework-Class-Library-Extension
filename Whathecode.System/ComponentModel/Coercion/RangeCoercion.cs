using System;
using Whathecode.System.Arithmetic.Range;


namespace Whathecode.System.ComponentModel.Coercion
{
    /// <summary>
    ///   Abstract class which can coerce a given value into a given range within a given context.
    /// </summary>
    /// <typeparam name = "TContext">The context in which to coerce the value.</typeparam>
    /// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
    /// <author>Steven Jeuris</author>
    public class RangeCoercion<TContext, TValue> : AbstractCoercion<TContext, TValue>
    {
        readonly Func<TContext, TValue> _getRangeStart;
        readonly Func<TContext, TValue> _getRangeEnd;


        /// <summary>
        ///   Create a new <see cref="RangeCoercion{T, U}" /> which coerces values into a given range within a given context.
        /// </summary>
        /// <param name="getRangeStart">A function which is called upon coercing, returning the start of the range.</param>
        /// <param name="getRangeEnd">A function which is called upon coercing, returning the end of the range.</param>
        public RangeCoercion( Func<TContext, TValue> getRangeStart, Func<TContext, TValue> getRangeEnd )
        {
            _getRangeStart = getRangeStart;
            _getRangeEnd = getRangeEnd;
        }


        /// <summary>
        ///   Coerce a given value.
        /// </summary>
        /// <param name = "context">The context in which to coerce the value.</param>
        /// <param name = "value">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        public override TValue Coerce( TContext context, TValue value )
        {
            // TODO: Cache interval, or calculator?
            return new Interval<TValue>( _getRangeStart( context ), true, _getRangeEnd( context ), true ).Clamp( value );
        }
    }
}