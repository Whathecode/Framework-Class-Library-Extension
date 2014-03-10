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
		///   Access is denied.
		/// </summary>
		AccessDenied = 0x0005,
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
		///   This operation returned because the time-out period expired.
		/// </summary>
		Timeout = 0x05B4
	}
}