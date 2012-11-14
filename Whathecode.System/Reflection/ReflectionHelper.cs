using System.Reflection;


namespace Whathecode.System.Reflection
{
	/// <summary>
	///   A helper class to do common reflection operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class ReflectionHelper
	{
		/// <summary>
		///   BindingFlags to return all instance members of a class, not including members up the hierarchy.
		/// </summary>
		public const BindingFlags InstanceMembers = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		/// <summary>
		///   BindingFlags to return all members of a class (static and instance), not including members up the hierarchy.
		/// </summary>
		public const BindingFlags ClassMembers = InstanceMembers | BindingFlags.Static;

		/// <summary>
		///   BindingFlags to return all instance members of a class, including members up the entire hierarchy.
		/// </summary>
		public const BindingFlags FlattenedInstanceMembers = BindingFlags.FlattenHierarchy | InstanceMembers;

		/// <summary>
		///   BindingFlags to return all members of a class (static and instance), including members up the entire class hierarchy.
		/// </summary>
		public const BindingFlags FlattenedClassMembers = BindingFlags.FlattenHierarchy | ClassMembers;

		/// <summary>
		///   BindingFlags to return all values of an enum type.
		/// </summary>
		public const BindingFlags EnumValues = BindingFlags.Public | BindingFlags.Static;
	}
}