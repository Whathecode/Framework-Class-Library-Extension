namespace Whathecode.Interop
{
	/// <summary>
	///   Common error codes used throughout win32.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public enum ErrorCode
	{
		/// <summary>
		///   The operation completed successfully.
		/// </summary>
		Success = 0x0000,
		/// <summary>
		///   The system cannot find the file specified.
		/// </summary>
		FileNotFound = 0x0002,
		/// <summary>
		///   The system cannot find the path specified.
		/// </summary>
		PathNotFound = 0x0003,
		/// <summary>
		///   Access is denied.
		/// </summary>
		AccessDenied = 0x0005,
		/// <summary>
		///   Not enough storage is available to process this command.
		/// </summary>
		OutOfMemory = 0x0008,
		/// <summary>
		///   The process cannot access the file because it is being used by another process.
		/// </summary>
		SharingViolation = 0x0020,
		/// <summary>
		///   The parameter is incorrect.
		/// </summary>
		InvalidParameter = 0x0057,
		/// <summary>
		///   This function is not supported on this system.
		/// </summary>
		CallNotImplemented = 0x0078,
		/// <summary>
		///   More data is available.
		/// </summary>
		MoreData = 0x00EA,
		/// <summary>
		///   No application is associated with the specified file for this operation.
		/// </summary>
		NoAssociation = 0x0483,
		/// <summary>
		///   An error occurred in sending the command to the application.
		/// </summary>
		DdeFail = 0x0484,
		/// <summary>
		///   One of the library files needed to run this application cannot be found.
		/// </summary>
		DllNotFound = 0x0485,
		/// <summary>
		///   The operation was canceled by the user.
		/// </summary>
		Cancelled = 0x04C7,
		/// <summary>
		///   This operation returned because the time-out period expired.
		/// </summary>
		Timeout = 0x05B4
	}
}