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
		///   The type of memory allocation.
		/// </summary>
		[Flags]
		public enum AllocationType
		{
			/// <summary>
			///   Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.
			///   The function also guarantees that when the caller later initially accesses the memory, the contents will be zero.
			///   Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.
			/// 
			///   To reserve and commit pages in one step, call <see cref="VirtualAllocEx" /> with <see cref="Commit" /> | <see cref="Reserve" />.
			/// 
			///   Attempting to commit a specific address range by specifying <see cref="Commit" /> without <see cref="Reserve" /> and a non-NULL address fails unless the entire range has already been reserved.
			///   The resulting error code is <see cref="ErrorCode.InvalidAddress" />.
			/// 
			///   An attempt to commit a page that is already committed does not cause the function to fail.
			///   This means that you can commit pages without first determining the current commitment state of each page.
			/// </summary>
			Commit = 0x1000,
			/// <summary>
			///   Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.
			/// 
			///   You commit reserved pages by calling <see cref="VirtualAllocEx" /> again with <see cref="Commit" />.
			///   To reserve and commit pages in one step, call <see cref="VirtualAllocEx" /> with <see cref="Commit" /> | <see cref="Reserve" />.
			/// 
			///   Other memory allocation functions, such as malloc and LocalAlloc, cannot use reserved memory until it has been released.
			/// </summary>
			Reserve = 0x2000,
			/// <summary>
			///   Indicates that data in the memory range specified by address and size is no longer of interest. The pages should not be read from or written to the paging file.
			///   However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other value.
			/// 
			///   Using this value does not guarantee that the range operated on with <see cref="Reset" /> will contain zeros.
			///   If you want the range to contain zeros, decommit the memory and then recommit it.
			/// 
			///   When you use <see cref="Reset" />, the <see cref="VirtualAllocEx" /> function ignores the value of memoryProtection.
			///   However, you must still set memoryProtection to a valid protection value, such as <see cref="MemoryProtection.NoAccess" />.
			/// 
			///   <see cref="VirtualAllocEx" /> returns an error if you use <see cref="Reset" /> and the range of memory is mapped to a file.
			///   A shared view is only acceptable if it is mapped to a paging file.
			/// </summary>
			Reset = 0x80000,
			/// <summary>
			///   <see cref="ResetUndo" /> should only be called on an address range to which <see cref="Reset" /> was successfully applied earlier.
			///   It indicates that the data in the specified memory range specified by address and size is of interest to the caller and attempts to reverse the effects of <see cref="Reset" />.
			///   If the function succeeds, that means all data in the specified address range is intact.
			///   If the function fails, at least some of the data in the address range has been replaced with zeroes.
			/// 
			///   This value cannot be used with any other value. If <see cref="ResetUndo" /> is called on an address range which was not MEM_RESET earlier, the behavior is undefined.
			///   When you specify <see cref="Reset" />, the <see cref="VirtualAllocEx" /> function ignores the value of memoryProtection.
			///   However, you must still set memoryProtection to a valid protection value, such as <see cref="MemoryProtection.NoAccess" />.
			/// </summary>
			ResetUndo = 0x1000000,
			/// <summary>
			///   Allocates memory using large page support.
			///   The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the GetLargePageMinimum function.
			/// </summary>
			LargePages = 0x20000000,
			/// <summary>
			///   Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.
			///   This value must be used with <see cref="Reserve" /> and no other values.
			/// </summary>
			Physical = 0x00400000,
			/// <summary>
			///   Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.
			/// </summary>
			TopDown = 0x00100000
		}

		/// <summary>
		///   The memory protection for the region of pages to be allocated.
		///   You must specify one of the following values when allocating or protecting a page in memory.
		///   Protection attributes cannot be assigned to a portion of a page; they can only be assigned to a whole page.
		/// </summary>
		[Flags]
		public enum MemoryProtection
		{
			/// <summary>
			///   Enables execute access to the committed region of pages. An attempt to write to the committed region results in an access violation.
			///   This flag is not supported by the CreateFileMapping function.
			/// </summary>
			Execute = 0x10,
			/// <summary>
			///   Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
			///   Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows XP with SP2 and Windows Server 2003 with SP1.
			/// </summary>
			ExecuteRead = 0x20,
			/// <summary>
			///   Enables execute, read-only, or read/write access to the committed region of pages.
			///   Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows XP with SP2 and Windows Server 2003 with SP1.
			/// </summary>
			ExecuteReadWrite = 0x40,
			/// <summary>
			///   Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.
			///   An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
			///   The private page is marked as <see cref="ExecuteReadWrite" />, and the change is written to the new page.
			///   This flag is not supported by the VirtualAlloc or <see cref="VirtualAllocEx" /> functions.
			///   Windows Vista, Windows Server 2003, and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows Vista with SP1 and Windows Server 2008.
			/// </summary>
			ExecuteWriteCopy = 0x80,
			/// <summary>
			///   Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
			///   This flag is not supported by the CreateFileMapping function.
			/// </summary>
			NoAccess = 0x01,
			/// <summary>
			///   Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
			///   If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
			/// </summary>
			ReadOnly = 0x02,
			/// <summary>
			///   Enables read-only or read/write access to the committed region of pages.
			///   If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
			/// </summary>
			ReadWrite = 0x04,
			/// <summary>
			///   Enables read-only or copy-on-write access to a mapped view of a file mapping object.
			///   An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
			///   The private page is marked as <see cref="ReadWrite" />, and the change is written to the new page.
			///   If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
			///   This flag is not supported by the VirtualAlloc or <see cref="VirtualAllocEx" /> functions.
			/// </summary>
			WriteCopy = 0x08,

			/// <summary>
			///   Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status.
			///   Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
			/// 
			///   When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.
			/// 
			///   If a guard page exception occurs during a system service, the service typically returns a failure status indicator.
			/// 
			///   This value cannot be used with <see cref="NoAccess"/>.
			/// 
			///   This flag is not supported by the CreateFileMapping function.
			/// </summary>
			GuardModifierflag = 0x100,
			/// <summary>
			///   Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device.
			///   Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
			/// 
			///   The <see cref="NoCacheModifierflag" /> flag cannot be used with the <see cref="GuardModifierflag" />, <see cref="NoAccess"/>, or <see cref="WriteCombineModifierflag" /> flags.
			/// 
			///   The <see cref="NoCacheModifierflag" /> flag can be used only when allocating private memory with the VirtualAlloc, <see cref="VirtualAllocEx" />, or VirtualAllocExNuma functions.
			///   To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the CreateFileMapping function.
			/// </summary>
			NoCacheModifierflag = 0x200,
			/// <summary>
			///   Sets all pages to be write-combined.
			/// 
			///   Applications should not use this attribute except when explicitly required for a device.
			///   Using the interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
			/// 
			///   The <see cref="WriteCombineModifierflag" /> flag cannot be specified with the <see cref="NoAccess" />, <see cref="GuardModifierflag" />, and <see cref="NoCacheModifierflag" /> flags.
			/// 
			///   The <see cref="WriteCombineModifierflag" /> flag can be used only when allocating private memory with the VirtualAlloc, <see cref="VirtualAllocEx" />, or VirtualAllocExNuma functions.
			///   To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the CreateFileMapping function.
			/// 
			///   Windows Server 2003 and Windows XP: This flag is not supported until Windows Server 2003 with SP1.
			/// </summary>
			WriteCombineModifierflag = 0x400
		}

		/// <summary>
		///   Defines the type of free operation for <see cref="Kernel32.VirtualFreeEx" />.
		/// </summary>
		public enum FreeOperationType
		{
			/// <summary>
			///   Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.
			///   The function does not fail if you attempt to decommit an uncommitted page.
			///   This means that you can decommit a range of pages without first determining their current commitment state.
			/// </summary>
			Decommit = 0x4000,
			/// <summary>
			///   Releases the specified region of pages. After the operation, the pages are in the free state.
			///   If you specify this value, size must be 0 (zero), and address must point to the base address returned by the <see cref="VirtualAllocEx" /> function when the region is reserved.
			///   The function fails if either of these conditions is not met.
			///   If any pages in the region are committed currently, the function first decommits, and then releases them.
			///   The function does not fail if you attempt to release pages that are in different states, some reserved and some committed.
			///   This means that you can release a range of pages without first determining the current commitment state.
			/// </summary>
			Release = 0x8000
		}

		#endregion // Types


		#region Functions

		/// <summary>
		///   Reserves or commits a region of memory within the virtual address space of a specified process.
		///   The function initializes the memory it allocates to zero, unless <see cref="AllocationType.Reset" /> is used.
		///   To specify the NUMA node for the physical memory, see VirtualAllocExNuma.
		/// </summary>
		/// <param name="processHandle">
		///   The handle to a process. The function allocates memory within the virtual address space of this process.
		///   The handle must have the <see cref="ProcessAccessFlags.VirtualMemoryOperation" /> access right.
		///   For more information, see Process Security and Access Rights.
		/// </param>
		/// <param name="address">
		///   The pointer that specifies a desired starting address for the region of pages that you want to allocate.
		///   If you are reserving memory, the function rounds this address down to the nearest multiple of the allocation granularity.
		///   If you are committing memory that is already reserved, the function rounds this address down to the nearest page boundary.
		///   To determine the size of a page and the allocation granularity on the host computer, use the GetSystemInfo function.
		///   If <paramref name="address" /> is NULL, the function determines where to allocate the region.
		/// </param>
		/// <param name="size">
		///   The size of the region of memory to allocate, in bytes.
		///   If <paramref name="address" /> is NULL, the function rounds <paramref name="size" /> up to the next page boundary.
		///   If <paramref name="address" /> is not NULL, the function allocates all pages that contain one or more bytes in the range from
		///   <paramref name="address" /> to <paramref name="address" /> + <paramref name="size" />.
		///   This means, for example, that a 2-byte range that straddles a page boundary causes the function to allocate both pages.
		/// </param>
		/// <param name="allocationType">The type of memory allocation.</param>
		/// <param name="memoryProtection">
		///   The memory protection for the region of pages to be allocated.
		///   If the pages are being committed, you can specify any one of <see cref="MemoryProtection" />.</param>
		/// <returns>
		///   If the function succeeds, the return value is the base address of the allocated region of pages.
		///   If the function fails, the return value is <see cref="IntPtr.Zero" />. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern IntPtr VirtualAllocEx( SafeProcessHandle processHandle, IntPtr address, UIntPtr size, AllocationType allocationType, MemoryProtection memoryProtection );

		/// <summary>
		///   Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.
		/// </summary>
		/// <param name="processHandle">
		///   A handle to a process. The function frees memory within the virtual address space of the process.
		///   The handle must have the <see cref="ProcessAccessFlags.VirtualMemoryOperation" /> access right.
		///   For more information, see Process Security and Access Rights.
		/// </param>
		/// <param name="address">
		///   A pointer to the starting address of the region of memory to be freed.
		///   If the <paramref name="freeOperationType" /> parameter is <see cref="FreeOperationType.Release" />, <paramref name="address" /> must be the base address
		///   returned by the <see cref="VirtualAllocEx" /> function when the region is reserved.
		/// </param>
		/// <param name="size">
		///   The size of the region of memory to free, in bytes.
		/// 
		///   If the <paramref name="freeOperationType" /> parameter is <see cref="FreeOperationType.Release" />, <paramref name="size" /> must be 0 (zero).
		///   The function frees the entire region that is reserved in the initial allocation call to <see cref="VirtualAllocEx" />.
		/// 
		///   If <paramref name="freeOperationType" /> is <see cref="FreeOperationType.Decommit" />, the function decommits all memory pages that contain one or more bytes in the range
		///   from the <paramref name="address" /> parameter to (<paramref name="address" />+<paramref name="size" />). 
		///   This means, for example, that a 2-byte region of memory that straddles a page boundary causes both pages to be decommitted.
		///   If <paramref name="address" /> is the base address returned by <see cref="VirtualAllocEx" /> and <paramref name="size" /> is 0 (zero),
		///   the function decommits the entire region that is allocated by <see cref="VirtualAllocEx" />.
		///   After that, the entire region is in the reserved state.
		/// </param>
		/// <param name="freeOperationType">The type of free operation.</param>
		/// <returns>
		///   If the function succeeds, the return value is TRUE.
		///   If the function fails, the return value is FALSE. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool VirtualFreeEx( SafeProcessHandle processHandle, IntPtr address, UIntPtr size, FreeOperationType freeOperationType );

		#endregion // Functions
	}
}
