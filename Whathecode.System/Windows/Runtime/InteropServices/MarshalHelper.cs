using System.Runtime.InteropServices;


namespace Whathecode.System.Runtime.InteropServices
{
	/// <summary>
	///   A helper class to do common <see cref="Marshal" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class MarshalHelper
	{
		/// <summary>
		///   Retrieves the last Win32 error code and throws the corresponding exception.
		/// </summary>
		public static void ThrowLastWin32ErrorException()
		{
			Marshal.ThrowExceptionForHR( Marshal.GetLastWin32Error() );
		}
	}
}
