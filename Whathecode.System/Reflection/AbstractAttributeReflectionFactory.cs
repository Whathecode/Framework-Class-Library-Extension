using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Whathecode.System.ComponentModel.Property;


namespace Whathecode.System.Reflection
{
	/// <summary>
	///   An abstract factory which allows to create instances for a specific type
	///   by using reflection to get information on what to create through attributes applied to its class members.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractAttributeReflectionFactory
	{
		readonly Property<Type> _ownerType = new Property<Type>();

		/// <summary>
		///   The type of the class for which to create instances.
		/// </summary>
		protected Type OwnerType
		{
			get { return _ownerType.GetValue(); }
			set { _ownerType.SetValue( value, OnNewOwnerType ); }
		}

		/// <summary>
		///   The list of members with relevant attibute(s) added to it.
		/// </summary>
		protected Dictionary<MemberInfo, Attribute[]> AttributedMembers { get; private set; }


		/// <summary>
		///   Create a new attribute reflection factory for a given type.
		/// </summary>
		/// <param name = "ownerType">The type on which to do reflection and generate instances.</param>
		protected AbstractAttributeReflectionFactory( Type ownerType )
		{
			OwnerType = ownerType;
		}


		#region Abstract definitions

		/// <summary>
		///   Get a list of all the attribute types used by the factory.
		/// </summary>
		/// <returns>A list of all the attribute types used by the factory.</returns>
		protected abstract Type[] GetAttributeTypes();

		#endregion


		void OnNewOwnerType( Type oldType, Type newType )
		{
			// Initialize the list of attributed members for the new owner type.
			if ( newType == null )
			{
				AttributedMembers = new Dictionary<MemberInfo, Attribute[]>();
			}
			else
			{
				// Get all wanted attribute types for all members.
				AttributedMembers = (from member in newType.GetMembers( ReflectionHelper.AllClassMembers )
					from type in GetAttributeTypes()
					from attribute in (Attribute[])member.GetCustomAttributes( type, false )
					group attribute by member).ToDictionary( g => g.Key, g => g.ToArray() );
			}
		}
	}
}