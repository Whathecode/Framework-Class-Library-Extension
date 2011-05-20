namespace Whathecode.System.ComponentModel.Validation
{
    /// <summary>
    ///   Abstract class which can validate a given type.
    /// </summary>
    /// <typeparam name = "T">The type of the value to validate.</typeparam>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractValidation<T>
    {
        /// <summary>
        ///   Returns whether a given value is valid.
        /// </summary>
        /// <param name = "value">The value to verify whether it is valid.</param>
        /// <returns>True when the value is valid, false otherwise.</returns>
        public abstract bool IsValid( T value );
    }
}
