using System;
using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains Kernel32.dll definitions and operations.
	/// </summary>
	public static partial class Kernel32
	{
		#region Types

		/// <summary>
		///   The Microsoft Windows security model enables you to control access to process objects.
		///   For more information about security, see Access-Control Model.
		/// </summary>
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			/// <summary>
			///   Required to create a process.
			/// </summary>
			CreateProcess = 0x0080,
			/// <summary>
			///   Required to create a thread.
			/// </summary>
			CreateThread = 0x0002,
			/// <summary>
			///   Required to duplicate a handle using DuplicateHandle.
			/// </summary>
			DuplicateHandle = 0x0040,
			/// <summary>
			///   Required to retrieve certain information about a process, such as its token, exit code, and priority class (see <see cref="AdvApi32.OpenProcessToken" />).
			/// </summary>
			QueryInformation = 0x0400,
			/// <summary>
			///   Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
			///   A handle that has the <see cref="QueryInformation" /> access right is automatically granted <see cref="QueryLimitedInformation" />.
			///   Windows Server 2003 and Windows XP: This access right is not supported.
			/// </summary>
			QueryLimitedInformation = 0x1000,
			/// <summary>
			///   Required to set certain information about a process, such as its priority class (see SetPriorityClass).
			/// </summary>
			SetInformation = 0x0200,
			/// <summary>
			///   Required to set memory limits using SetProcessWorkingSetSize.
			/// </summary>
			SetQuota = 0x0100,
			/// <summary>
			///   Required to suspend or resume a process.
			/// </summary>
			SuspendResume = 0x0800,
			/// <summary>
			///   Required to terminate a process using TerminateProcess.
			/// </summary>
			Terminate = 0x0001,
			/// <summary>
			///   Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
			/// </summary>
			VirtualMemoryOperation = 0x0008,
			/// <summary>
			///   Required to read memory in a process using ReadProcessMemory.
			/// </summary>
			VirtualMemoryRead = 0x0010,
			/// <summary>
			///   Required to write to memory in a process using WriteProcessMemory.
			/// </summary>
			VirtualMemoryWrite = 0x0020,
			/// <summary>
			///   Required to wait for the process to terminate using the wait functions.
			/// </summary>
			Synchronize = Constants.StandardAccessRights.Synchronize
		}

		#endregion // Types


		#region Functions

		/// <summary>
		///   Opens an existing local process object.
		/// </summary>
		/// <param name="desiredAccess">
		///   The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the process access rights.
		///   If the caller has enabled the SeDebugPrivilege privilege, the requested access is granted regardless of the contents of the security descriptor.
		/// </param>
		/// <param name="inheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
		/// <param name="processId">
		///   The identifier of the local process to be opened.
		/// 
		///   If the specified process is the System Process (0x00000000), the function fails and the last error code is <see cref="ErrorCode.InvalidParameter" />.
		///   If the specified process is the Idle process or one of the CSRSS processes,
		///   this function fails and the last error code is <see cref="ErrorCode.AccessDenied" /> because their access restrictions prevent user-level code from opening them.
		/// 
		///   If you are using GetCurrentProcessId as an argument to this function, consider using GetCurrentProcess instead of <see cref="OpenProcess" />, for improved performance.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is an open handle to the specified process.
		///   If the function fails, the return value is NULL. To get extended error information, call GetLastWin32Error.
		/// </returns>
		/// <remarks>
		///   To open a handle to another local process and obtain full access rights, you must enable the SeDebugPrivilege privilege.
		///   For more information, see Changing Privileges in a Token.
		///   The handle returned by the <see cref="OpenProcess" /> function can be used in any function that requires a handle to a process,
		///   such as the wait functions, provided the appropriate access rights were requested.
		///   When you are finished with the handle, be sure to close it using the <see cref="CloseHandle" /> function.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern SafeProcessHandle OpenProcess( ProcessAccessFlags desiredAccess, bool inheritHandle, int processId );

		/// <summary>
		///   Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
		/// </summary>
		/// <param name="process">A handle to the process with memory that is being read. The handle must have <see cref="ProcessAccessFlags.VirtualMemoryRead" /> access to the process.</param>
		/// <param name="baseAddress">
		///   A pointer to the base address in the specified process from which to read. Before any data transfer occurs,
		///   the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
		/// </param>
		/// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
		/// <param name="size">The number of bytes to be read from the specified process.</param>
		/// <param name="numberOfBytesRead">
		///   A pointer to a variable that receives the number of bytes transferred into the specified buffer.
		///   If <paramref name="numberOfBytesRead" /> is NULL, the parameter is ignored.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is TRUE.
		///   If the function fails, the return value is FALSE. To get extended error information, call GetLastWin32Error.
		///   The function fails if the requested read operation crosses into an area of the process that is inaccessible.
		/// </returns>
		/// <remarks>
		///   ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process.
		///   Any process that has a handle with <see cref="ProcessAccessFlags.VirtualMemoryRead" /> access can call the function.
		///   The entire area to be read must be accessible, and if it is not accessible, the function fails.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool ReadProcessMemory( SafeProcessHandle process, IntPtr baseAddress, IntPtr buffer, UIntPtr size, out UIntPtr numberOfBytesRead );

		/// <summary>
		///   Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
		/// </summary>
		/// <param name="process">
		///   A handle to the process memory to be modified.
		///   The handle must have <see cref="ProcessAccessFlags.VirtualMemoryWrite" /> and <see cref="ProcessAccessFlags.VirtualMemoryOperation" /> access to the process.
		/// </param>
		/// <param name="baseAddress">
		///   A pointer to the base address in the specified process to which data is written. Before data transfer occurs,
		///   the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
		/// </param>
		/// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
		/// <param name="size">The number of bytes to be written to the specified process.</param>
		/// <param name="numberOfBytesWritten">
		///   A pointer to a variable that receives the number of bytes transferred into the specified process.
		///   This parameter is optional. If <paramref name="numberOfBytesWritten"/> is NULL, the parameter is ignored.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is TRUE.
		///   If the function fails, the return value is FALSE. To get extended error information, call GetLastWin32Error.
		///   The function fails if the requested write operation crosses into an area of the process that is inaccessible.
		/// </returns>
		/// <remarks>
		///   WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process.
		///   Any process that has a handle with <see cref="ProcessAccessFlags.VirtualMemoryWrite" /> and <see cref="ProcessAccessFlags.VirtualMemoryOperation" /> access to the process to be written to can call the function.
		///   Typically but not always, the process with address space that is being written to is being debugged.
		///   The entire area to be written to must be accessible, and if it is not accessible, the function fails.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool WriteProcessMemory( SafeProcessHandle process, IntPtr baseAddress, IntPtr buffer, UIntPtr size, out UIntPtr numberOfBytesWritten );

		#endregion // Functions
	}
}
