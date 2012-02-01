using System;
using System.Collections.Generic;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Can find and manage application windows.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class WindowManager
	{
		/// <summary>
		///   Enumerates all top-level windows on the screen.
		/// </summary>
		public static List<WindowInfo> GetWindows()
		{			
			var windows = new List<WindowInfo>();
			bool success = User32.EnumWindows(
				( handle, lparam ) =>
				{
					windows.Add( new WindowInfo( handle ) );
					return true;
				},
				IntPtr.Zero );

			if ( !success )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return windows;
		}
	}
}
