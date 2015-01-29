namespace Whathecode.Interop
{
	public enum HResult
	{
		/// <summary>
		///   Operation successful.
		/// </summary>
		Ok = 0x00000000,
		/// <summary>
		///   Operation aborted.
		/// </summary>
		Abort = unchecked( (int)0x80004004 ),
		/// <summary>
		///   General access denied error.
		/// </summary>
		AccessDenied = unchecked( (int)0x80070005 ),
		/// <summary>
		///   Unspecified failure.
		/// </summary>
		Fail = unchecked( (int)0x80004005 ),
		/// <summary>
		///   Handle that is not valid.
		/// </summary>
		Handle = unchecked( (int)0x80070006 ),
		/// <summary>
		///   One or more arguments are not valid.
		/// </summary>
		InvalidArgument = unchecked( (int)0x80070057 ),
		/// <summary>
		///   No such interface supported.
		/// </summary>
		NoInterface = unchecked( (int)0x80004002 ),
		/// <summary>
		///   Not implemented.
		/// </summary>
		NotImplemented = unchecked( (int)0x80004001 ),
		/// <summary>
		///   Failed to allocate necessary memory.
		/// </summary>
		OutOfMemory = unchecked( (int)0x8007000E ),
		/// <summary>
		///   Pointer that is not valid.
		/// </summary>
		Pointer = unchecked( (int)0x80004003 ),
		/// <summary>
		///   Unexpected failure.
		/// </summary>
		Unexpected = unchecked( (int)0x8000FFFF ),
	}
}
