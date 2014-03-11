using System;
using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	public static partial class Shell32
	{
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
	}
}
