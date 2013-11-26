using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;


namespace Whathecode.System.Runtime.InteropServices
{
	/// <summary>
	///   A wrapper class for an unmanaged handle created with Marshal.AllocHGlobal which will cleaned up when its finalizer is run.
	///   Use this for handles which require no further cleaning up than <see cref="Marshal.FreeHGlobal"/>.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[SecurityPermission( SecurityAction.Demand, UnmanagedCode = true )]  // Demand unmanaged code permission to use this class.
	sealed class SafeUnmanagedMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public SafeUnmanagedMemoryHandle()
			: base( true ) {}

		public SafeUnmanagedMemoryHandle( IntPtr existingHandle )
			: base( true )
		{
			SetHandle( existingHandle );
		}


		/// <summary>
		///   Allocated unmanaged memory of the requested size and returns it as a SafeUnmanagedMemoryHandle.
		/// </summary>
		/// <param name = "size">The required number of bytes in memory.</param>
		/// <returns>A safe memory handle arround the newly allocated amount of bytes.</returns>
		public static SafeUnmanagedMemoryHandle FromNewlyAllocatedMemory( int size )
		{
			return new SafeUnmanagedMemoryHandle( Marshal.AllocHGlobal( size ) );
		}


		override protected bool ReleaseHandle()
		{
			// Check whether there is a handle to clean up.
			if ( handle == IntPtr.Zero )
			{
				return false;
			}

			// Free the handle.
			Marshal.FreeHGlobal( handle );
			handle = IntPtr.Zero;

			return true;
		}
	}
}
