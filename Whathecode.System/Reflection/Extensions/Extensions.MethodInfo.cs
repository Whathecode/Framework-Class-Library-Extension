using System.Diagnostics.Contracts;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Creates a delegate of the specified type to represent this method.
        /// </summary>
        /// <typeparam name = "TDelegate">The type of the delegate to create.</typeparam>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "instance">When an instance method, the instance to call this method on.</param>
        /// <param name = "options">Options which specify what type of delegate should be created.</param>         
        /// <returns>The delegate representing this method.</returns>
        public static TDelegate CreateDelegate<TDelegate>(
            this MethodInfo source,
            object instance = null,
            DelegateHelper.CreateOptions options = DelegateHelper.CreateOptions.None )
            where TDelegate : class
        {
            return DelegateHelper.CreateDelegate<TDelegate>( source, instance, options );
        }

        /// <summary>
        ///   Creates a delegate of a specified type that represents a method which can be executed on an instance passed as parameter.
        /// </summary>
        /// <typeparam name="TDelegate">
        ///   The type for the delegate. This delegate needs at least one (first) type parameter denoting the type of the instance
        ///   which will be passed.
        ///   E.g. Action&lt;ExampleObject, object&gt;,
        ///        where ExampleObject denotes the instance type and object denotes the desired type of the first parameter of the method.
        /// </typeparam>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "options">Options which specify what type of delegate should be created.</param> 
        /// <returns>The delegate representing this method, with as first argument the instance to call this method on.</returns>
        public static TDelegate CreateDynamicInstanceDelegate<TDelegate>(
            this MethodInfo source,
            DelegateHelper.CreateOptions options = DelegateHelper.CreateOptions.None )
            where TDelegate : class
        {
            Contract.Requires( !source.IsStatic );

            return DelegateHelper.CreateDynamicInstanceDelegate<TDelegate>( source, options );
        }
    }
}
