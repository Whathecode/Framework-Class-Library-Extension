using System.Reflection;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns whether the event is static or not.
		/// </summary>
		/// <param name = "source">The event to verify whether it is static or not.</param>
		/// <returns>True when the event is static; false otherwise.</returns>
		public static bool IsStatic( this EventInfo source )
		{
			// When an event is static, both the add and remove method are static.
			return source.AddMethod.IsStatic;
		}
	}
}
