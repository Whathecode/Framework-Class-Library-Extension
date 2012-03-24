using System.Diagnostics.Contracts;
using System.Reflection;


namespace Whathecode.System.Experimental.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   EXPERIMENTAL! Info: http://stackoverflow.com/q/9847424/590790
		///   Returns the associated <see cref = "FieldInfo" /> for an event.
		/// </summary>
		/// <param name = "eventInfo">The event to return the FieldInfo for.</param>
		/// <returns>The associated <see cref = "FieldInfo" /> for the event.</returns>
		public static FieldInfo GetFieldInfo( this EventInfo eventInfo )
		{
			Contract.Requires( eventInfo != null );

			return eventInfo.DeclaringType.GetField(
				eventInfo.Name,
				BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
		}
	}
}
