using System;
using System.Runtime.InteropServices;
using System.Security.Principal;


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

		/// <summary>
		///   Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.
		/// </summary>
		/// <param name="folderId">A GUID that identifies the known folder.</param>
		/// <param name="flags">Either <see cref="KnownFolderRetrievalFlags.None" />; otherwise, one or more of <see cref="KnownFolderRetrievalFlags" />.</param>
		/// <param name="accessToken">
		///   An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder for the current user.
		/// 
		///   Request a specific user's folder by passing the hToken of that user.
		///   This is typically done in the context of a service that has sufficient privileges to retrieve the token of a given user.
		///   That token must be opened with TOKEN_QUERY and <see cref = "TokenAccessLevels.Impersonate" /> rights. In some cases, you also need to include <see cref = "TokenAccessLevels.Duplicate" />.
		///   In addition to passing the user's hToken, the registry hive of that specific user must be mounted. See Access Control for further discussion of access control issues.
		/// 
		///   Assigning the hToken parameter a value of -1 indicates the Default User.
		///   This allows clients of <see cref="SHGetKnownFolderPath" /> to find folder locations (such as the Desktop folder) for the Default User.
		///   The Default User user profile is duplicated when any new user account is created, and includes special folders such as Documents and Desktop.
		///   Any items added to the Default User folder also appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </param>
		/// <param name="path">
		///   When this method returns, contains the address of a pointer to a null-terminated Unicode string that specifies the path of the known folder.
		///   The calling process is responsible for freeing this resource once it is no longer needed by calling CoTaskMemFree.
		///   The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".</param>
		/// <returns>
		///   Returns <see cref="HResult.Ok" /> if successful, or an error value otherwise, including the following:
		///   <list type="table">
		///     <item>
		///       <term><see cref="HResult.Fail" /></term>
		///       <description>
		///         Among other things, this value can indicate that the rfid parameter references a KNOWNFOLDERID which does not have a path (such as a folder marked as KF_CATEGORY_VIRTUAL).
		///       </description>
		///     </item>
		///     <item>
		///       <term><see cref="HResult.InvalidArgument" /></term>
		///       <description>
		///         Among other things, this value can indicate that the rfid parameter references a KNOWNFOLDERID that is not present on the system. Not all KNOWNFOLDERID values are present on all systems.
		///         Use IKnownFolderManager::GetFolderIds to retrieve the set of KNOWNFOLDERID values for the current system.
		///       </description>
		///     </item>
		///   </list>
		/// </returns>
		[DllImport( Dll )]
		public static extern int SHGetKnownFolderPath( ref Guid folderId, KnownFolderRetrievalFlags flags, SafeTokenHandle accessToken, out IntPtr path );

		/// <summary>
		///   Redirects a known folder to a new location.
		/// </summary>
		/// <param name="folderId">A GUID that identifies the known folder.</param>
		/// <param name="flags">Either <see cref="KnownFolderRetrievalFlags.None" /> or <see cref="KnownFolderRetrievalFlags.DontUnexpand" />.</param>
		/// <param name="accessToken">
		///   An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to access the current user's instance of the folder.
		///   However, you may need to assign a value to <paramref name="accessToken" /> for those folders that can have multiple users but are treated as belonging to a single user.
		///   The most commonly used folder of this type is Documents.
		/// 
		///   The calling application is responsible for correct impersonation when <paramref name="accessToken" /> is non-null.
		///   It must have appropriate security privileges for the particular user, including <see cref = "TokenAccessLevels.Query" /> and <see cref = "TokenAccessLevels.Impersonate" />,
		///   and the user's registry hive must be currently mounted.
		///   See Access Control for further discussion of access control issues.
		/// 
		///   Assigning the <paramref name="accessToken" /> parameter a value of -1 indicates the Default User.
		///   This allows clients of <see cref="SHSetKnownFolderPath" /> to set folder locations (such as the Desktop folder) for the Default User.
		///   The Default User user profile is duplicated when any new user account is created, and includes special folders such as Documents and Desktop.
		///   Any items added to the Default User folder also appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </param>
		/// <param name="path">The folder's new path, with maximum length <see cref="Constants.MaximumPathLength" />. This path cannot be of zero length.</param>
		/// <returns>
		///   Returns <see cref="HResult.Ok" /> if successful, or an error value otherwise, including the following:
		///   <list type="table">
		///     <item>
		///       <term><see cref="HResult.InvalidArgument" /></term>
		///       <description>
		///         Among other things, this value can indicate that the rfid parameter references a KNOWNFOLDERID that is not present on the system.
		///         Not all KNOWNFOLDERID values are present on all systems. Use IKnownFolderManager::GetFolderIds to retrieve the set of KNOWNFOLDERID values for the current system.
		///       </description>
		///     </item>
		///   </list>
		/// </returns>
		/// <remarks>
		///   The caller of this function must have Administrator privileges. To call this function on public known folders, the caller must have Administrator privileges.
		///   For per-user known folders the caller only requires User privileges.
		/// 
		///   Some of the known folders, for example, the Documents folder, are per-user. Every user has a different path for their Documents folder.
		///   If <paramref name="accessToken" /> is NULL, the API tries to access the calling application's instance of the folder, which is that of the current user.
		///   If <paramref name="accessToken" /> is a valid user token, the API tries to impersonate the user using this token and tries to access that user's instance.
		/// 
		///   This function cannot be called on folders of type KF_CATEGORY_FIXED and KF_CATEGORY_VIRTUAL.
		/// 
		///   To call this function on a folder of type KF_CATEGORY_COMMON, the calling application must be running with elevated privileges.
		/// </remarks>
		[DllImport( Dll, CharSet = CharSet.Unicode )]
		public static extern int SHSetKnownFolderPath( ref Guid folderId, KnownFolderRetrievalFlags flags, SafeTokenHandle accessToken, string path );

		/// <summary>
		///   Notifies the system of an event that an application has performed. An application should use this function if it performs an action that may affect the Shell.
		/// </summary>
		/// <param name="eventId">
		///   Describes the event that has occurred. Typically, only one event is specified at a time.
		///   If more than one event is specified, the values contained in the <paramref name="item1" /> and <paramref name="item2" /> parameters must be the same, respectively, for all specified events.
		///   This value determines what to pass for <paramref name="item1" /> and <paramref name="item2" />, as specified in the documentation:
		///   https://msdn.microsoft.com/en-us/library/windows/desktop/bb762118%28v=vs.85%29.aspx
		/// </param>
		/// <param name="flags">Flags that, when combined bitwise with SHCNF_TYPE, indicate the meaning of the dwItem1<paramref name="item1" /> and <paramref name="item2" /> parameters.</param>
		/// <param name="item1">First event-dependent value.</param>
	    /// <param name="item2">Second event-dependent value.</param>
	    /// <returns>This function does not return a value.</returns>
	    /// <remarks>
	    ///   Applications that register new handlers of any type must call <see cref="SHChangeNotify" /> with the <see cref="EventId.FileTypeAssociationChanged" /> flag
	    ///   to instruct the Shell to invalidate the icon and thumbnail cache. This will also load new icon and thumbnail handlers that have been registered.
	    ///   Note, however, that icon overlay handlers are not reloaded.
	    /// 
		///   The strings pointed to by <paramref name="item1" /> and <paramref name="item2" /> are set to Unicode when using <see cref="ChangeNotifyFlags.Path" /> and <see cref="ChangeNotifyFlags.Printer" />.
	    /// </remarks>
		[DllImport( Dll )]
		public static extern int SHChangeNotify( EventId eventId, ChangeNotifyFlags flags, IntPtr item1, IntPtr item2 );
	}
}
