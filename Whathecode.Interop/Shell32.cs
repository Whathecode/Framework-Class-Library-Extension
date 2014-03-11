using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Class through which Shell32.dll calls can be accessed.
	///   TODO: Clean up remaining original documentation, converting it to the wrapper's equivalents.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static partial class Shell32
	{
		const string Dll = "Shell32.dll";


		/// <summary>
		///   Performs an operation on a specified file.
		/// </summary>
		/// <param name="executeInfo">A <see cref="ShellExecuteInfo" /> structure that contains and receives information about the application being executed.</param>
		/// <returns>Returns TRUE if successful; otherwise, FALSE. Call <see cref="Marshal.GetLastWin32Error" /> for extended error information.</returns>
		/// <remarks>
		///   Because <see cref="ShellExecuteEx "/> can delegate execution to Shell extensions (data sources, context menu handlers, verb implementations) that are activated using Component Object Model (COM),
		///   COM should be initialized before <see cref="ShellExecuteEx "/> is called.
		///   Some Shell extensions require the COM single-threaded apartment (STA) type. In that case, COM should be initialized as shown here:
		///   <code>CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE)</code>
		///   There are instances where <see cref="ShellExecuteEx "/> does not use one of these types of Shell extension and those instances would not require COM to be initialized at all.
		///   Nonetheless, it is good practice to always initalize COM before using this function.
		/// 
		///   When DLLs are loaded into your process, you acquire a lock known as a loader lock.
		///   The DllMain function always executes under the loader lock.
		///   It is important that you do not call ShellExecuteEx while you hold a loader lock.
		///   Because <see cref="ShellExecuteEx "/> is extensible, you could load code that does not function properly in the presence of a loader lock, risking a deadlock and therefore an unresponsive thread.
		/// 
		///   With multiple monitors, if you specify an HWND and set the <see cref="ShellExecuteInfo.Verb" /> member of <see cref="executeInfo" /> to <see cref="CommonShellExecuteVerbs.Properties" />,
		///   any windows created by <see cref="ShellExecuteEx "/> might not appear in the correct position.
		/// 
		///   If the function succeeds, it sets the <see cref="ShellExecuteInfo.SitePointer" /> member of <see cref="executeInfo" /> structure to a value greater than 32.
		///   If the function fails, <see cref="ShellExecuteInfo.SitePointer" /> is set to the <see cref="ShellExecuteError" /> error value that best indicates the cause of the failure.
		///   Although <see cref="ShellExecuteInfo.SitePointer" /> is declared as an HINSTANCE for compatibility with 16-bit Windows applications, it is not a true HINSTANCE.
		///   It can be cast only to an int and can be compared only to either the value 32 or the <see cref="ShellExecuteError" /> error codes.
		/// 
		///   The <see cref="ShellExecuteError" /> error values are provided for compatibility with ShellExecute.
		///   To retrieve more accurate error information, use <see cref="Marshal.GetLastWin32Error" />. It may return one of the following values:
		/// 
		///   <list type="table">
		///     <item>
		///       <term><see cref="ErrorCode.FileNotFound" /></term>
		///       <description>The specified file was not found.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.PathNotFound" /></term>
		///       <description>The specified path was not found.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.DdeFail" /></term>
		///       <description>The Dynamic Data Exchange (DDE) transaction failed.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.DdeFail" /></term>
		///       <description>The Dynamic Data Exchange (DDE) transaction failed.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.NoAssociation" /></term>
		///       <description>There is no application associated with the specified file name extension.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.AccessDenied" /></term>
		///       <description>Access to the specified file is denied.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.DllNotFound" /></term>
		///       <description>One of the library files necessary to run the application can't be found.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.Cancelled" /></term>
		///       <description>The function prompted the user for additional information, but the user canceled the request.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.OutOfMemory" /></term>
		///       <description>There is not enough memory to perform the specified action.</description>
		///     </item>
		///     <item>
		///       <term><see cref="ErrorCode.SharingViolation" /></term>
		///       <description>A sharing violation occurred.</description>
		///     </item>
		///   </list>
		/// 
		///   Opening items from a URL You can register your application to activate when passed URLs. You can also specify which protocols your application supports. See Application Registration for more info.
		///   Site chain support As of Windows 8, you can provide a site chain pointer to the <see cref="ShellExecuteInfo" /> function to support item activation with services from that site.
		///   See Launching Applications (ShellExecute, <see cref="ShellExecuteEx" />, <see cref="ShellExecuteInfo" />) for more information.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool ShellExecuteEx( ref ShellExecuteInfo executeInfo );
	}
}
