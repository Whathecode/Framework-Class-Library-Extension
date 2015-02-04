using System;
using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains common Kernel32.dll definitions and operations.
	/// </summary>
	public static partial class Kernel32
	{
		const string Dll = "kernel32.dll";


		#region Functions

		/// <summary>
		///   Closes an open object handle.
		/// </summary>
		/// <param name="objectHandle">A valid handle to an open object.</param>
		/// <returns>
		///   If the function succeeds, the return value is nonzero.
		///   If the function fails, the return value is zero. To get extended error information, call GetLastWin32Error.
		///   If the application is running under a debugger, the function will throw an exception if it receives either a handle value that is not valid or a pseudo-handle value.
		///   This can happen if you close a handle twice,
		///   or if you call <see cref="CloseHandle" /> on a handle returned by the FindFirstFile function instead of calling the FindClose function.
		/// </returns>
		/// <remarks>
		///   The CloseHandle function closes handles to the following objects:
		///   <list type="bullet">
		///     <item>Access token</item>
		///     <item>Communications device</item>
		///     <item>Console input</item>
		///     <item>Console screen buffer</item>
		///     <item>Event</item>
		///     <item>File</item>
		///     <item>File mapping</item>
		///     <item>I/O completion port</item>
		///     <item>Job</item>
		///     <item>Mailslot</item>
		///     <item>Memory resource notification</item>
		///     <item>Mutex</item>
		///     <item>Named pipe</item>
		///     <item>Pipe</item>
		///     <item>Process</item>
		///     <item>Semaphore</item>
		///     <item>Thread</item>
		///     <item>Transaction</item>
		///     <item>Waitable timer</item>
		///	  </list>
		///   The documentation for the functions that create these objects indicates that <see cref="CloseHandle" /> should be used when you are finished with the object,
		///   and what happens to pending operations on the object after the handle is closed.
		///   In general, <see cref="CloseHandle" /> invalidates the specified object handle, decrements the object's handle count, and performs object retention checks.
		///   After the last handle to an object is closed, the object is removed from the system.
		///   For a summary of the creator functions for these objects, see Kernel Objects.
		/// 
		///   Generally, an application should call <see cref="CloseHandle" /> once for each handle it opens.
		///   It is usually not necessary to call <see cref="CloseHandle" /> if a function that uses a handle fails with <see cref="ErrorCode.InvalidHandle"/>,
		///   because this error usually indicates that the handle is already invalidated.
		///   However, some functions use <see cref="ErrorCode.InvalidHandle"/> to indicate that the object itself is no longer valid.
		///   For example, a function that attempts to use a handle to a file on a network might fail with <see cref="ErrorCode.InvalidHandle"/> if the network connection is severed,
		///   because the file object is no longer available. In this case, the application should close the handle.
		/// 
		///   If a handle is transacted, all handles bound to a transaction should be closed before the transaction is committed.
		///   If a transacted handle was opened by calling CreateFileTransacted with the FILE_FLAG_DELETE_ON_CLOSE flag,
		///   the file is not deleted until the application closes the handle and calls CommitTransaction.
		///   For more information about transacted objects, see Working With Transactions.
		/// 
		///   Closing a thread handle does not terminate the associated thread or remove the thread object.
		///   Closing a process handle does not terminate the associated process or remove the process object.
		///   To remove a thread object, you must terminate the thread, then close all handles to the thread.
		///   For more information, see Terminating a Thread. To remove a process object, you must terminate the process, then close all handles to the process.
		///   For more information, see Terminating a Process.
		/// 
		///   Closing a handle to a file mapping can succeed even when there are file views that are still open.
		///   For more information, see Closing a File Mapping Object.
		/// 
		///   Do not use the <see cref="CloseHandle" /> function to close a socket.
		///   Instead, use the closesocket function, which releases all resources associated with the socket including the handle to the socket object.
		///   For more information, see Socket Closure.
		/// 
		///   Windows Phone 8: This API is supported.
		///   Windows Phone 8.1: This API is supported.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool CloseHandle( IntPtr objectHandle );

		#endregion // Functions
	}
}
