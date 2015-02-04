using System;
using Microsoft.Win32.SafeHandles;


namespace Whathecode.Interop
{
	/// <summary>
	///   A wrapper class for a process handle.
	/// </summary>
	public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public SafeProcessHandle()
			: base( true ) {}

		public SafeProcessHandle( IntPtr handle )
			: base( true )
		{
			SetHandle( handle );
		}


		protected override bool ReleaseHandle()
		{
			return Kernel32.CloseHandle( handle );
		}
	}
}
