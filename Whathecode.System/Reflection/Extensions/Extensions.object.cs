using System;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Get the value of a property of an object.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "property">The property to get.</param>
        /// <returns>The value of the property of the object.</returns>
        public static object GetPropertyValue( this object source, string property )
        {
            PropertyInfo info = source.GetType().GetProperty( property );

            if ( info == null )
            {
                throw new ArgumentException( "The property with name \"" + property + "\" wasn't found.", "property" );
            }

            return info.GetValue( source, null );
        }

        /// <summary>
        ///   Returns the value of a given member of an object.
        /// </summary>
        /// <param name = "source">The source of this extension method.</param>
        /// <param name = "member">The member of the object to get the value from.</param>
        /// <returns>The value of the member of the object.</returns>
        public static object GetValue( this object source, MemberInfo member )
        {
            object value;
            if ( member is FieldInfo )
            {
                FieldInfo field = (FieldInfo)member;
                value = field.GetValue( source );
            }
            else if ( member is PropertyInfo )
            {
                PropertyInfo property = (PropertyInfo)member;
                value = property.GetValue( source, null );
            }
            else
            {
                throw new NotImplementedException( "Can't return value of the given member." );
            }

            return value;
        }
    }
}
