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
			PropertyInfo info = GetPropertyInfo( source, property );

			return info.GetValue( source, null );
		}

		/// <summary>
		///   Set the value of a property of an object.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "property">The property to set.</param>
		/// <param name = "value">The value to set the property to.</param>
		public static void SetPropertyValue( this object source, string property, object value )
		{
			PropertyInfo info = GetPropertyInfo( source, property );

			info.SetValue( source, value );
		}

		static PropertyInfo GetPropertyInfo( object source, string property )
		{
			PropertyInfo info = source.GetType().GetProperty( property );

			if ( info == null )
			{
				throw new ArgumentException( "The property with name \"" + property + "\" wasn't found.", "property" );
			}

			return info;
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

		/// <summary>
		///   Returns the value of the object at the given location inside this object.
		///   TODO: Support more advanced property paths. (e.g. indexers) Create custom PropertyPath class?
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "path">
		///   The path in the object to find the value for.
		///   The dot operator can be used to access composed members as you would ordinarily use it.
		///   E.g. Member.SubMember.SubSubMember
		/// </param>
		/// <exception cref="ArgumentException">Thrown when an invalid path is passed and no value could be found.</exception>
		/// <returns>The object at the given path.</returns>
		public static object GetValue( this object source, string path )
		{
			string[] paths = path.Split( '.' );
			object currentObject = source;

			foreach ( var subPath in paths )
			{
				Type type = currentObject.GetType();
				MemberInfo[] matchingMembers = type.GetMember( subPath );
				if ( matchingMembers.Length != 1 )
				{
					throw new ArgumentException( "Invalid path \"" + path + "\" for object of type \"" + source.GetType() + "\".", "path" );
				}
				currentObject = currentObject.GetValue( matchingMembers[ 0 ] );
			}

			return currentObject;
		}
	}
}