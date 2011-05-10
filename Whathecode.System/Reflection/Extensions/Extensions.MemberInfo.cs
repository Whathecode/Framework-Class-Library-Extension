using System;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///   Returns the type of a member when it's a property, field or event.
        /// </summary>
        /// <param name = "member">The member to get the type from.</param>
        /// <returns></returns>
        public static Type GetMemberType( this MemberInfo member )
        {
            Type memberType = null;
            if ( member is FieldInfo )
            {
                FieldInfo field = (FieldInfo)member;
                memberType = field.FieldType;
            }
            else if ( member is PropertyInfo )
            {
                PropertyInfo property = (PropertyInfo)member;
                memberType = property.PropertyType;
            }
            else if ( member is EventInfo )
            {
                EventInfo e = (EventInfo)member;
                memberType = e.EventHandlerType;
            }

            if ( memberType == null )
            {
                throw new NotImplementedException( "Can't return type of the given member." );
            }

            return memberType;
        }
    }
}
