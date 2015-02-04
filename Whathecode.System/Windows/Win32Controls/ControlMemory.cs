using System;
using System.Runtime.InteropServices;
using Whathecode.Interop;
using Whathecode.System.Runtime.InteropServices;
using AccessFlags = Whathecode.Interop.Kernel32.ProcessAccessFlags;


namespace Whathecode.System.Windows.Win32Controls
{
	/// <summary>
	///   Accessible memory by a Win32 control.
	/// </summary>
	class ControlMemory : AbstractDisposable
	{
		readonly SafeProcessHandle _processHandle;

		/// <summary>
		///   The address where the memory is allocated.
		/// </summary>
		public readonly IntPtr Address;


		/// <summary>
		///   Allocates memory which is accessible by the control.
		/// </summary>
		/// <param name="control">The control for which to allocated memory.</param>
		/// <param name="size">The size of the region of memory to allocate, in bytes.</param>
		public ControlMemory( WindowInfo control, int size )
		{
			// Get handle to the process.
			int processId = control.GetProcess().Id;
			const AccessFlags accessFlags = AccessFlags.VirtualMemoryOperation | AccessFlags.VirtualMemoryRead | AccessFlags.VirtualMemoryWrite;
			_processHandle = Kernel32.OpenProcess( accessFlags, false, processId );
			if ( _processHandle == null )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			// Allocate memory.
			Address = Kernel32.VirtualAllocEx(
				_processHandle,
				IntPtr.Zero,
				new UIntPtr( (uint)size ),
				Kernel32.AllocationType.Reserve | Kernel32.AllocationType.Commit,
				Kernel32.MemoryProtection.ReadWrite );
			if ( Address == IntPtr.Zero )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}
		}


		/// <summary>
		///   Read a specified structure from the allocated memory.
		/// </summary>
		/// <typeparam name = "T">The structure type to read from memory.</typeparam>
		public T Read<T>()
			where T : struct
		{
			uint bytesToRead = (uint)Marshal.SizeOf( typeof( T ) );
			T[] buffer = new T[ 1 ];
			GCHandle pinnedBuffer = GCHandle.Alloc( buffer, GCHandleType.Pinned );
			UIntPtr bytesRead;
			bool success = Kernel32.ReadProcessMemory(
				_processHandle,
				Address,
				Marshal.UnsafeAddrOfPinnedArrayElement( buffer, 0 ),
				new UIntPtr( bytesToRead ),
				out bytesRead );
			pinnedBuffer.Free();
			if ( !success )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return buffer[ 0 ];
		}

		public void Write<T>( T point )
			where T : struct
		{
			uint bytesToWrite = (uint)Marshal.SizeOf( typeof( T ) );
			T[] buffer = { point };
			GCHandle pinnedBuffer = GCHandle.Alloc( buffer, GCHandleType.Pinned );
			UIntPtr bytesRead;
			bool success = Kernel32.WriteProcessMemory(
				_processHandle,
				Address,
				Marshal.UnsafeAddrOfPinnedArrayElement( buffer, 0 ),
				new UIntPtr( bytesToWrite ), 
				out bytesRead );
			pinnedBuffer.Free();
			if ( !success )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}
		}

		protected override void FreeManagedResources()
		{
			// Nothing to do.
		}

		protected override void FreeUnmanagedResources()
		{
			// Free previously allocated memory.
			bool success = Kernel32.VirtualFreeEx( _processHandle, Address, new UIntPtr( 0 ), Kernel32.FreeOperationType.Release );
			if ( !success )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}
		}
	}
}
