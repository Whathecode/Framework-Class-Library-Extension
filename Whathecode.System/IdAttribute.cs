using System;


namespace Whathecode.System
{
    /// <summary>
    ///   An attribute which contains an ID which can be used to link members together,
    ///   or reference them by using e.g. an enum value.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class IdAttribute : Attribute
    {
        readonly object _id;


        protected IdAttribute( object id )
        {
            _id = id;
        }


        /// <summary>
        ///   Get the ID to which the attribute applies.
        /// </summary>
        /// <returns>The ID to which the attribute applies.</returns>
        public object GetId()
        {
            return _id;
        }
    }
}