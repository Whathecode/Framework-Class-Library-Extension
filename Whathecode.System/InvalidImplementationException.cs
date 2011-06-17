using System;


namespace Whathecode.System
{
    /// <summary>
    ///   The exception that is thrown when a certain implementation contains errors.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public class InvalidImplementationException : Exception
    {
        /// <summary>
        ///   Create a new exception which can be thrown when an implementation contains errors.
        /// </summary>
        /// <param name="message">The message which describes the implementation error.</param>
        public InvalidImplementationException( string message )
            : base( message )
        {
        }
    }
}
