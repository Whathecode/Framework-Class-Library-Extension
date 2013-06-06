namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Indicates the state a window is in.
	/// </summary>
	public enum WindowState
	{
		/// <summary>
		///   The window is open, but not maximized.
		/// </summary>
		Open,
		/// <summary>
		///   The window is minimized and not visible.
		/// </summary>
		Minimized,
		/// <summary>
		///   The window is open and maximized.
		/// </summary>
		Maximized,
		/// <summary>
		///   The window is hidden.
		/// </summary>
		Hidden
	}
}
