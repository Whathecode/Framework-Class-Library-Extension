using System;
using System.Collections.Generic;


namespace Whathecode.System.Windows
{
	/// <summary>
	///   Exception which is thrown when an operation is executed on windows which are unresponsive.
	/// </summary>
	public class UnresponsiveWindowsException : Exception
	{
		/// <summary>
		///   The unresponsive windows which are causing the operation to hang.
		/// </summary>
		public IReadOnlyCollection<WindowInfo> UnresponsiveWindows { get; private set; }


		public UnresponsiveWindowsException( List<WindowInfo> unresponsiveWindows )
		{
			UnresponsiveWindows = unresponsiveWindows;
		}
	}
}
