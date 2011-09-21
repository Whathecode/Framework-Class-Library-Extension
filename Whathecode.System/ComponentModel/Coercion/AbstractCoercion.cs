namespace Whathecode.System.ComponentModel.Coercion
{
	/// <summary>
	///   Abstract class which can coerce a given type within a given context.
	/// </summary>
	/// <typeparam name = "TContext">The context in which to coerce the value.</typeparam>
	/// <typeparam name = "TValue">The type of the value to coerce.</typeparam>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractCoercion<TContext, TValue>
	{
		/// <summary>
		///   Coerce a given value.
		/// </summary>
		/// <param name = "context">The context in which to coerce the value.</param>
		/// <param name = "value">The value to coerce.</param>
		/// <returns>The coerced value.</returns>
		public abstract TValue Coerce( TContext context, TValue value );
	}
}