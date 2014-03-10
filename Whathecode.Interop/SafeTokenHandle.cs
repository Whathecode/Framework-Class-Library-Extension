using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;


namespace Whathecode.Interop
{
	/// <summary>
	///   A wrapper class for a token handle.
	/// </summary>
	public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public SafeTokenHandle()
			: base( true ) {}

		public SafeTokenHandle( IntPtr handle )
			: base( true )
		{
			base.SetHandle( handle );
		}


		[DllImport( "kernel32.dll" )]
		[ReliabilityContract( Consistency.WillNotCorruptState, Cer.Success )]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs( UnmanagedType.Bool )]
		private static extern bool CloseHandle( IntPtr handle );

		protected override bool ReleaseHandle()
		{
			return CloseHandle( handle );
		}
	}
}
