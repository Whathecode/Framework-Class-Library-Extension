using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns the type of a member when it's a property, field, event or inner class.
		/// </summary>
		/// <param name = "member">The member to get the type from.</param>
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
			else if ( member is Type )
			{
				memberType = (Type)member;
			}
			else if ( member is MethodInfo || member is ConstructorInfo )
			{
				throw new InvalidOperationException( "Can't return the type for methods and constructors." );
			}

			if ( memberType == null )
			{
				throw new NotSupportedException( "Can't return type of the given member of type \"" + member.GetType() + "\"" );
			}

			return memberType;
		}

		/// <summary>
		///   Get the attributes of the specified type.
		/// </summary>
		/// <typeparam name = "T">The type of the attributes to find.</typeparam>
		/// <param name = "member">The member on which to look for attributes.</param>
		/// <param name = "inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
		/// <returns>The found attributes.</returns>
		public static T[] GetAttributes<T>( this MemberInfo member, bool inherit = false )
		{
			Contract.Requires( typeof( T ).IsSubclassOf( typeof( Attribute ) ) );

			return member.GetCustomAttributes( typeof( T ), inherit ).Cast<T>().ToArray();
		}
	}
}