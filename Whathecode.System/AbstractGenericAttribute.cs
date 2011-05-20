using System;
using System.Diagnostics.Contracts;


namespace Whathecode.System
{
    /// <summary>
    ///   Allows creating an attribute which behavior is implemented in another class of any possible type.
    /// </summary>
    /// <remarks>
    ///   Since attributes don't allow generic template arguments and only allow constant arguments
    ///   it is difficult to group behavior in one attribute.
    ///   By passing a type argument and arguments for its constructor, it is possible to instantiate any desired type,
    ///   overcoming some of these limitations.
    /// </remarks>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractGenericAttribute : Attribute
    {
        /// <summary>
        ///   Thy dynamically created instance as specified in the constructor.
        /// </summary>
        protected object DynamicInstance { get; private set; }


        /// <summary>
        ///   Create a new attribute and initialize a certain type.
        /// </summary>
        /// <param name = "dynamicType">The type to initialize.</param>
        /// <param name="constructorArguments">The arguments to pass to the constructor of the type.</param>
        protected AbstractGenericAttribute( Type dynamicType, params object[] constructorArguments )
        {
            Contract.Ensures( DynamicInstance != null );

            DynamicInstance = Activator.CreateInstance( dynamicType, constructorArguments );
        }
    }
}
