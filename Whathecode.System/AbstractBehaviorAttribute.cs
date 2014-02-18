using System;
using System.Diagnostics.Contracts;


namespace Whathecode.System
{
	/// <summary>
	///   Allows creating an attribute which contains a behavior, implemented in another class of any possible type.
	/// </summary>
	/// <remarks>
	///   Attributes don't allow generic template arguments and only constant expressions.
	///   By passing a type argument and arguments for its constructor, it is possible to instantiate any desired type,
	///   overcoming some of these limitations.
	/// </remarks>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractBehaviorAttribute : Attribute
	{
		/// <summary>
		///   The dynamically created instance of the type passed to the constructor.
		/// </summary>
		protected object DynamicInstance { get; private set; }


		/// <summary>
		///   Create a new attribute and initialize a certain type.
		/// </summary>
		/// <param name = "dynamicType">The type to initialize.</param>
		/// <param name = "constructorArguments">The arguments to pass to the constructor of the type.</param>
		protected AbstractBehaviorAttribute( Type dynamicType, params object[] constructorArguments )
		{
			Contract.Ensures( DynamicInstance != null );

			DynamicInstance = Activator.CreateInstance( dynamicType, constructorArguments );
		}
	}
}