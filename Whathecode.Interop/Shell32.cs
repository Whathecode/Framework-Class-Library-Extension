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


		#region Types

		/// <summary>
		///   Contains information used by <see cref="ShellExecuteEx" />.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct ShellExecuteInfo
		{
			/// <summary>
			///   Required. The size of this structure, in bytes.
			/// </summary>
			public uint StructSize;
			/// <summary>
			///   Flags that indicate the content and validity of the other structure members.
			/// </summary>
			public ShellExecuteInfoType Type;
			/// <summary>
			///   Optional. A handle to the parent window, used to display any message boxes that the system might produce while executing this function. This value can be <see cref="IntPtr.Zero" />.
			/// </summary>
			public IntPtr ParentWindowHandle;
			/// <summary>
			///   A string, referred to as a verb, that specifies the action to be performed. The set of available verbs depends on the particular file or folder.
			///   Generally, the actions available from an object's shortcut menu are available verbs. This parameter can be NULL, in which case the default <see cref="Verb" /> is used if available.
			///   If not, the "open" verb is used. If neither verb is available, the system uses the first verb listed in the registry. Commonly used verbs are listed in <see cref="CommonShellExecuteVerbs" />.
			/// </summary>
			[MarshalAs( UnmanagedType.LPTStr )]
			public string Verb;
			/// <summary>
			///   Specifies the name of the file or object on which <see cref="ShellExecuteEx" /> will perform the action specified by the <see cref="Verb" /> parameter.
			///   The system registry verbs that are supported by the <see cref="ShellExecuteEx" /> function include "open" for executable files and document files and "print" for document files
			///   for which a print handler has been registered. Other applications might have added Shell verbs through the system registry, such as "play" for .avi and .wav files.
			///   To specify a Shell namespace object, pass the fully qualified parse name and set the <see cref="ShellExecuteInfoType.InvokeIdList" /> flag in the <see cref="Type" /> parameter.
			///   Note: If the <see cref="ShellExecuteInfoType.InvokeIdList" /> flag is set, you can use either <see cref="File" /> or <see cref="Pidl" /> to identify the item by its file system path
			///   or its PIDL respectively. One of the two values—<see cref="File" /> or <see cref="Pidl" />—must be set.
			///   Note: If the path is not included with the name, the current directory is assumed.
			/// </summary>
			[MarshalAs( UnmanagedType.LPTStr )]
			public string File;
			/// <summary>
			///   Optional. Application parameters. The parameters must be separated by spaces. If the <see cref="File" /> member specifies a document file, <see cref="Parameters" /> should be NULL.
			/// </summary>
			[MarshalAs( UnmanagedType.LPTStr )]
			public string Parameters;
			/// <summary>
			///   Optional. Specifies the name of the working directory. If this member is NULL, the current directory is used as the working directory.
			/// </summary>
			[MarshalAs( UnmanagedType.LPTStr )]
			public string Directory;
			/// <summary>
			///   Required. Flags that specify how an application is to be shown when it is opened.
			///   If <see cref="File" /> specifies a document file, the flag is simply passed to the associated application. It is up to the application to decide how to handle it.
			/// </summary>
			public User32.WindowState Show;
			/// <summary>
			///   If <see cref="ShellExecuteInfoType.GetProcessHandle" /> is set and the <see cref="ShellExecuteEx" /> call succeeds, it sets this member to a value greater than 32.
			///   If the function fails, it is set to an <see cref="ShellExecuteError" /> error value that indicates the cause of the failure.
			///   Although <see cref="SitePointer" /> is declared as an HINSTANCE for compatibility with 16-bit Windows applications, it is not a true HINSTANCE.
			///   It can be cast only to an int and compared to either 32 or the following <see cref="ShellExecuteError" /> error codes.
			/// </summary>
			public IntPtr SitePointer;
			/// <summary>
			///   The address of an absolute ITEMIDLIST structure (PCIDLIST_ABSOLUTE) to contain an item identifier list that uniquely identifies the file to execute.
			///   This member is ignored if the <see cref="Type" /> member does not include <see cref="ShellExecuteInfoType.Pidl" /> or <see cref="ShellExecuteInfoType.InvokeIdList" />.
			/// </summary>
			public IntPtr Pidl;
			/// <summary>
			///   Specifies the following:
			///   <list type="bullet">
			///     <item>A ProgId. For example, "Paint.Picture".</item>
			///     <item>A URI protocol scheme. For example, "http".</item>
			///     <item>A file extension. For example, ".txt".</item>
			///     <item>
			///       A registry path under HKEY_CLASSES_ROOT that names a subkey that contains one or more Shell verbs.
			///       This key will have a subkey that conforms to the Shell verb registry schema, such as shell\verb name
			///     </item>
			///   </list>
			///   This member is ignored if <see cref="Type"/> does not include <see cref="ShellExecuteInfoType.ClassName" />.
			/// </summary>
			[MarshalAs( UnmanagedType.LPTStr )]
			public string ClassName;
			/// <summary>
			///   A handle to the registry key for the file type. The access rights for this registry key should be set to KEY_READ.
			///   This member is ignored if <see cref="Type" /> does not include <see cref="ShellExecuteInfoType.ClassKey" />.
			/// </summary>
			public IntPtr ClassKey;
			/// <summary>
			///   A keyboard shortcut to associate with the application. The low-order word is the virtual key code, and the high-order word is a modifier flag (HOTKEYF_).
			///   For a list of modifier flags, see the description of the WM_SETHOTKEY message. This member is ignored if <see cref="Type" /> does not include <see cref="ShellExecuteInfoType.HotKey" />.
			/// </summary>
			public uint HotKey;
			/// <summary>
			///   Used to pass either a handle to an icon or a monitor.
			/// 
			///   When an icon, a handle to the icon for the file type. This member is ignored if <see cref="Type" /> does not include <see cref="ShellExecuteInfoType.Icon" />.
			///   This value is used only in Windows XP and earlier. It is ignored as of Windows Vista.
			/// 
			///   When a monitor, a handle to the monitor upon which the document is to be displayed. This member is ignored if <see cref="Type" /> does not include <see cref="ShellExecuteInfoType.Monitor" />.
			/// </summary>
			public IntPtr IconOrMonitor;
			/// <summary>
			///   A handle to the newly started application. This member is set on return and is always NULL unless <see cref="Type"/> is set to <see cref="ShellExecuteInfoType.GetProcessHandle" />.
			///   Even if <see cref="Type"/> is set to <see cref="ShellExecuteInfoType.GetProcessHandle" />, <see cref="Process" /> will be NULL if no process was launched.
			///   For example, if a document to be launched is a URL and an instance of Internet Explorer is already running, it will display the document.
			///   No new process is launched, and <see cref="Process" /> will be NULL.
			///   Note: <see cref="Shell32.ShellExecuteEx" /> does not always return an <see cref="Process"/>, even if a process is launched as the result of the call.
			///   For example, an <see cref="Process"/> does not return when you use <see cref="ShellExecuteInfoType.InvokeIdList" /> to invoke IContextMenu.
			/// </summary>
			public IntPtr Process;


			/// <summary>
			///   Initializes a <see cref="ShellExecuteInfo" /> correctly, in order to execute a given PIDL.
			/// </summary>
			/// <param name = "pidl">The PIDL to execute.</param>
			/// <param name = "show">Indicates how the application should be shown when it is opened.</param>
			public static ShellExecuteInfo ExecutePidl( IntPtr pidl, User32.WindowState show )
			{
				return new ShellExecuteInfo
				{
					StructSize = (uint)Marshal.SizeOf( typeof( ShellExecuteInfo ) ),
					Type = ShellExecuteInfoType.Pidl,
					Pidl = pidl,
					Show = show
				};
			}
		}


		/// <summary>
		///   Flags that indicate the content and validity of members in <see cref="ShellExecuteInfo" />.
		/// </summary>
		[Flags]
		public enum ShellExecuteInfoType : uint
		{
			/// <summary>
			///   Use default values.
			/// </summary>
			Default = 0x0000,
			/// <summary>
			///   Use the class name given by the className member. If both ClassKey and ClassName are set, the class key is used.
			/// </summary>
			ClassName = 0x0001,
			/// <summary>
			///   Use the class key given by the classKey member. If both ClassKey and ClassName are set, the class key is used.
			/// </summary>
			ClassKey = 0x0003,
			/// <summary>
			///   Use the item identifier list given by the pidl member. The pidl member must point to an ITEMIDLIST structure.
			/// </summary>
			Pidl = 0x0004,
			/// <summary>
			///   Use the IContextMenu interface of the selected item's shortcut menu handler.
			///   Use either file to identify the item by its file system path or pidl to identify the item by its PIDL.
			///   This flag allows applications to use ShellExecuteEx to invoke verbs from shortcut menu extensions instead of the static verbs listed in the registry.
			///   Note: <see cref="InvokeIdList" /> overrides and implies <see cref="Pidl" />.
			/// </summary>
			InvokeIdList = 0x000C,
			/// <summary>
			///   Use the icon given by the icon member. This flag cannot be combined with <see cref="Monitor" />.
			///   Note: This flag is used only in Windows XP and earlier. It is ignored as of Windows Vista.
			/// </summary>
			Icon = 0x0010,
			/// <summary>
			///   Use the keyboard shortcut given by the hotKey member.
			/// </summary>
			HotKey = 0x0020,
			/// <summary>
			///   Use to indicate that the process member receives the process handle. This handle is typically used to allow an application to find out when a process created with ShellExecuteEx terminates.
			///   In some cases, such as when execution is satisfied through a DDE conversation, no handle will be returned.
			///   The calling application is responsible for closing the handle when it is no longer needed.
			/// </summary>
			GetProcessHandle = 0x0040,
			/// <summary>
			///   Validate the share and connect to a drive letter. This enables reconnection of disconnected network drives. The file member is a UNC path of a file on a network.
			/// </summary>
			ConnectNetworkDrive = 0x0080,
			/// <summary>
			///   Wait for the execute operation to complete before returning. This flag should be used by callers that are using ShellExecute forms that might result in an async activation,
			///   for example DDE, and create a process that might be run on a background thread.
			///   (Note: <see cref="ShellExecuteEx" /> runs on a background thread by default if the caller's threading model is not Apartment.)
			///   Calls to <see cref="ShellExecuteEx" /> from processes already running on background threads should always pass this flag.
			///   Also, applications that exit immediately after calling <see cref="ShellExecuteEx" /> should specify this flag.
			///   If the execute operation is performed on a background thread and the caller did not specify the <see cref="AsyncOk" /> flag,
			///   then the calling thread waits until the new process has started before returning.
			///   This typically means that either CreateProcess has been called, the DDE communication has completed,
			///   or that the custom execution delegate has notified <see cref="ShellExecuteEx" /> that it is done.
			///   If the <see cref="WaitForInputIdle" /> flag is specified, then <see cref="ShellExecuteEx" /> calls WaitForInputIdle and waits for the new process to idle before returning,
			///   with a maximum timeout of 1 minute.
			///   For further discussion on when this flag is necessary, see the Remarks section.
			/// </summary>
			NoAsync = 0x0100,
			/// <summary>
			///   Expand any environment variables specified in the string given by the directory or file member.
			/// </summary>
			DoEnvironmentSubstitution = 0x0200,
			/// <summary>
			///   Do not display an error message box if an error occurs.
			/// </summary>
			NoErrorUi = 0x0400,
			/// <summary>
			///   Use this flag to indicate a Unicode application.
			/// </summary>
			Unicode = 0x4000,
			/// <summary>
			///   Use to inherit the parent's console for the new process instead of having it create a new console. It is the opposite of using a CREATE_NEW_CONSOLE flag with CreateProcess.
			/// </summary>
			NoConsole = 0x8000,
			/// <summary>
			///   The execution can be performed on a background thread and the call should return immediately without waiting for the background thread to finish.
			///   Note that in certain cases <see cref="ShellExecuteEx" /> ignores this flag and waits for the process to finish before returning.
			/// </summary>
			AsyncOk = 0x100000,
			/// <summary>
			///   Use this flag when specifying a monitor on multi-monitor systems. The monitor is specified in the monitor member. This flag cannot be combined with <see cref="Icon" />.
			/// </summary>
			Monitor = 0x200000,
			/// <summary>
			///   Introduced in Windows XP. Do not perform a zone check. This flag allows <see cref="ShellExecuteEx" /> to bypass zone checking put into place by IAttachmentExecute.
			/// </summary>
			NoZoneChecks = 0x800000,
			/// <summary>
			///   After the new process is created, wait for the process to become idle before returning, with a one minute timeout. See WaitForInputIdle for more details.
			/// </summary>
			WaitForInputIdle = 0x02000000,
			/// <summary>
			///   Introduced in Windows XP. Keep track of the number of times this application has been launched.
			///   Applications with sufficiently high counts appear in the Start Menu's list of most frequently used programs.
			/// </summary>
			LogUsage = 0x04000000,
			/// <summary>
			///   Introduced in Windows 8. The <see cref="ShellExecuteInfo.SitePointer" /> member is used to specify the IUnknown of an object that implements IServiceProvider.
			///   This object will be used as a site pointer. The site pointer is used to provide services to the ShellExecute function, the handler binding process, and invoked verb handlers.
			/// </summary>
			UseSitePointer = 0x08000000
		}


		/// <summary>
		///   Common verbs which can be passed as a parameter to <see cref="Shell32.ShellExecuteEx" />.
		/// </summary>
		public static class CommonShellExecuteVerbs
		{
			/// <summary>
			///   Launches an editor and opens the document for editing. If file is not a document file, the function will fail.
			/// </summary>
			public static string Edit = "edit";
			/// <summary>
			///   Explores the folder specified by file.
			/// </summary>
			public static string Explore = "explore";
			/// <summary>
			///   Initiates a search starting from the specified directory.
			/// </summary>
			public static string Find = "find";
			/// <summary>
			///   Opens the file specified by the file parameter. The file can be an executable file, a document file, or a folder.
			/// </summary>
			public static string Open = "open";
			/// <summary>
			///   Prints the document file specified by file. If file is not a document file, the function will fail.
			/// </summary>
			public static string Print = "print";
			/// <summary>
			///   Displays the file or folder's properties.
			/// </summary>
			public static string Properties = "properties";
		}

		public enum ShellExecuteError
		{
			/// <summary>
			///   File not found.
			/// </summary>
			FileNotFound = ErrorCode.FileNotFound,
			/// <summary>
			///   Path not found.
			/// </summary>
			PathNotFound = ErrorCode.PathNotFound,
			/// <summary>
			///   Access denied.
			/// </summary>
			AccessDenied = ErrorCode.AccessDenied,
			/// <summary>
			///   Out of memory.
			/// </summary>
			OutOfMemory = ErrorCode.OutOfMemory,
			/// <summary>
			///   Dynamic-link library not found.
			/// </summary>
			DllNotFound = 32,
			/// <summary>
			///   Cannot share an open file.
			/// </summary>
			CannotShareOpenFile = 26,
			/// <summary>
			///   File association information not complete.
			/// </summary>
			AssociationInformationIncomplete = 27,
			/// <summary>
			///   DDE operation timed out.
			/// </summary>
			DdeOperationTimedOut = 28,
			/// <summary>
			///   DDE operation failed.
			/// </summary>
			DdeOperationFailed = 29,
			/// <summary>
			///   DDE operation is busy.
			/// </summary>
			DdeOperationBusy = 30,
			/// <summary>
			///   File association not available.
			/// </summary>
			UnavailableFileAssociation = 31
		}

		/// <summary>
		///   Specify special retrieval options for known folders. These values supersede CSIDL values, which have parallel meanings.
		/// </summary>
		[Flags]
		public enum KnownFolderRetrievalFlags : uint
		{
			None = 0,
			/// <summary>
			///   Build a simple IDList (PIDL).
			///   This value can be used when you want to retrieve the file system path but do not specify this value if you are retrieving the localized display name of the folder because it might not resolve correctly.
			/// </summary>
			SimpleIdList = 0x00000100,
			/// <summary>
			///   Gets the folder's default path independent of the current location of its parent. <see cref="DefaultPath" /> must also be set.
			/// </summary>
			NotParentRelative = 0x00000200,
			/// <summary>
			///   Gets the default path for a known folder. If this flag is not set, the function retrieves the current—and possibly redirected—path of the folder.
			///   The execution of this flag includes a verification of the folder's existence unless <see cref="DontVerify" /> is set.
			/// </summary>
			DefaultPath = 0x00000400,
			/// <summary>
			///   Initializes the folder using its Desktop.ini settings. If the folder cannot be initialized, the function returns a failure code and no path is returned.
			///   This flag should always be combined with <see cref="Create" />.
			/// 
			///   If the folder is located on a network, the function might take a longer time to execute.
			/// </summary>
			InitializeDesktopIni = 0x00000800,
			/// <summary>
			///   Gets the true system path for the folder, free of any aliased placeholders such as %USERPROFILE%, returned by SHGetKnownFolderIDList and IKnownFolder::GetIDList.
			///   This flag has no effect on paths returned by SHGetKnownFolderPath and IKnownFolder::GetPath.
			///   By default, known folder retrieval functions and methods return the aliased path if an alias exists.
			/// </summary>
			NoAlias = 0x00001000,
			/// <summary>
			///   Stores the full path in the registry without using environment strings.
			///   If this flag is not set, portions of the path may be represented by environment strings such as %USERPROFILE%.
			///   This flag can only be used with SHSetKnownFolderPath and IKnownFolder::SetPath.
			/// </summary>
			DontUnexpand = 0x00002000,
			/// <summary>
			///   Do not verify the folder's existence before attempting to retrieve the path or IDList.
			///   If this flag is not set, an attempt is made to verify that the folder is truly present at the path.
			///   If that verification fails due to the folder being absent or inaccessible, the function returns a failure code and no path is returned.
			///   If the folder is located on a network, the function might take a longer time to execute. Setting this flag can reduce that lag time.
			/// </summary>
			DontVerify = 0x00004000,
			/// <summary>
			///   Forces the creation of the specified folder if that folder does not already exist. The security provisions predefined for that folder are applied.
			///   If the folder does not exist and cannot be created, the function returns a failure code and no path is returned.
			///   This value can be used only with the following functions and methods:
			///	  <list type="bullet">
			///     <item>SHGetKnownFolderPath</item>
			///     <item>SHGetKnownFolderIDList</item>
			///     <item>IKnownFolder::GetIDList</item>
			///     <item>IKnownFolder::GetPath</item>
			///     <item>IKnownFolder::GetShellItem</item>
			///   </list>
			/// </summary>
			Create = 0x00008000,
			/// <summary>
			///   Introduced in Windows 7: When running inside an app container, or when providing an app container token, this flag prevents redirection to app container folders.
			///   Instead, it retrieves the path that would be returned where it not running inside an app container.
			/// </summary>
			NoAppContainerRedirection = 0x00010000,
			/// <summary>
			///   Introduced in Windows 7. Return only aliased PIDLs. Do not use the file system path.
			/// </summary>
			AliasOnly = 0x80000000
		}

		/// <summary>
		///   Describes an event that has occurred.
		/// </summary>
		[Flags]
		public enum EventId
		{
			/// <summary>
			///   All events have occurred.
			/// </summary>
			AllEvents = unchecked( (int)0x7FFFFFFFL ),
			/// <summary>
			///   A file type association has changed.
			/// </summary>
			FileTypeAssociationChanged = unchecked( (int)0x08000000L ),
			/// <summary>
			///   The attributes of an item or folder have changed.
			/// </summary>
			AttributesChanged = unchecked( (int)0x00000800L ),
			/// <summary>
			///   A nonfolder item has been created.
			/// </summary>
			Created = unchecked( (int)0x00000002L ),
			/// <summary>
			///   A nonfolder item has been deleted.
			/// </summary>
			Deleted = unchecked( (int)0x00000004L ),
			/// <summary>
			///   A drive has been added. 
			/// </summary>
			DriveAdded = unchecked( (int)0x00000100L ),
			/// <summary>
			///   A drive has been removed.
			/// </summary>
			DriveRemoved = unchecked( (int)0x00000080L ),
			/// <summary>
			///   The amount of free space on a drive has changed.
			/// </summary>
			FreeSpaceChanged = unchecked( (int)0x00040000L ),
			/// <summary>
			///   Storage media has been inserted into a drive.
			/// </summary>
			MediaInserted = unchecked( (int)0x00000020L ),
			/// <summary>
			///   Storage media has been removed from a drive.
			/// </summary>
			MediaRemoved = unchecked( (int)0x00000040L ),
			/// <summary>
			///   A folder has been created.
			/// </summary>
			FolderCreated = unchecked( (int)0x00000008L ),
			/// <summary>
			///   A folder on the local computer is being shared via the network.
			/// </summary>
			FolderNetworkShared = unchecked( (int)0x00000200L ),
			/// <summary>
			///   A folder on the local computer is no longer being shared via the network.
			/// </summary>
			FolderNetworkUnshared = unchecked( (int)0x00000400L ),
			/// <summary>
			///   The name of a folder has changed.
			/// </summary>
			FolderRenamed = unchecked( (int)0x00020000L ),
			/// <summary>
			///   The name of a nonfolder item has changed.
			/// </summary>
			Renamed = unchecked( (int)0x00000001L ),
			/// <summary>
			///   A folder has been removed.
			/// </summary>
			FolderRemoved = unchecked( (int)0x00000010L ),
			/// <summary>
			///   The computer has disconnected from a server.
			/// </summary>
			ServerDisconnected = unchecked( (int)0x00004000L ),
			/// <summary>
			///   The contents of an existing folder have changed, but the folder still exists and has not been renamed.
			/// </summary>
			FolderContentsChanged = unchecked( (int)0x00001000L ),
			/// <summary>
			///   An image in the system image list has changed. 
			/// </summary>
			UpdatedImage = unchecked( (int)0x00008000L ),
			/// <summary>
			///   An existing item (a folder or a nonfolder) has changed, but the item still exists and has not been renamed.
			/// </summary>
			UpdatedItem = unchecked( (int)0x00002000L ),
			/// <summary>
			///   Specifies a combination of all of the disk event identifiers.
			/// </summary>
			DiskEvents = unchecked( (int)0x0002381FL ),
			/// <summary>
			///   Specifies a combination of all of the global event identifiers.
			/// </summary>
			GlobalEvents = unchecked( (int)0x0C0581E0L ),
			/// <summary>
			///   The specified event occurred as a result of a system interrupt. As this value modifies other event values, it cannot be used alone.
			/// </summary>
			Interrupt = unchecked( (int)0x80000000L ),
		}

		/// <summary>
		///   Flags that, when combined bitwise with SHCNF_TYPE, indicate the meaning of the item1 and item2 parameters of <see cref="SHChangeNotify" />.
		/// </summary>
		[Flags]
		public enum ChangeNotifyFlags
		{
			/// <summary>
			///   The item1 and item2 parameters are DWORD values.
			/// </summary>
			Dword = 3,
			/// <summary>
			///   item1 and item2 are the addresses of ITEMIDLIST structures that represent the item(s) affected by the change. Each ITEMIDLIST must be relative to the desktop folder.
			/// </summary>
			ItemIdList = 0,
			/// <summary>
			///   item1 and item2 are the addresses of null-terminated unicode strings of maximum length MAX_PATH that contain the full path names of the items affected by the change.
			/// </summary>
			Path = 5,
			/// <summary>
			///   item1 and item2 are the addresses of null-terminated unicode strings that represent the friendly names of the printer(s) affected by the change.
			/// </summary>
			Printer = 6,
			/// <summary>
			///   The function should not return until the notification has been delivered to all affected components.
			///   As this flag modifies other data-type flags, it cannot be used by itself.
			/// </summary>
			Flush = 0x1000,
			/// <summary>
			///   The function should begin delivering notifications to all affected components but should return as soon as the notification process has begun.
			///   As this flag modifies other data-type flags, it cannot by used by itself. This flag includes <see cref="Flush"/>.
			/// </summary>
			FlushNoWait = 0x2000,
			/// <summary>
			///   Notify clients registered for all children.
			/// </summary>
			NotifyRecursive = 0x10000
		}

		#endregion // Types


		#region Functions

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

		#endregion // Functions
	}
}
