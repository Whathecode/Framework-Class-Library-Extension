using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Class through which user32.dll calls can be accessed for which the .NET framework offers no alternative.
	/// </summary>
	/// <author>Steven Jeuris</author>
	static partial class User32
	{
		const string Dll = "user32.dll";


		/// <summary>
		///   An application-defined callback function used with the EnumWindows or EnumDesktopWindows function.
		///   It receives top-level window handles. The WNDENUMPROC type defines a pointer to this callback function.
		///   EnumWindowsProc is a placeholder for the application-defined function name.
		/// </summary>
		/// <param name="windowHandle">A handle to a top-level window.</param>
		/// <param name="lParam">The application-defined value given in EnumWindows or EnumDesktopWindows.</param>
		/// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
		/// <remarks>An application must register this callback function by passing its address to EnumWindows or EnumDesktopWindows.</remarks>
		public delegate bool EnumWindowsProc( IntPtr windowHandle, IntPtr lParam );


		/// <summary>
		///   Enumerates all top-level windows on the screen by passing the handle to each window, in turn, to an application-defined callback function.
		///   EnumWindows continues until the last top-level window is enumerated or the callback function returns FALSE.
		/// </summary>
		/// <param name="callback">A pointer to an application-defined callback function. For more information, see EnumWindowsProc.</param>
		/// <param name="lParam">An application-defined value to be passed to the callback function.</param>
		/// <returns>
		///   If the function succeeds, the return value is TRUE.
		///   If the function fails, the return value is FALSE. To get extended error information, call GetLastWin32Error.
		///   If EnumWindowsProc returns zero, the return value is also zero.
		///   In this case, the callback function should call SetLastError to obtain a meaningful error code to be returned to the caller of EnumWindows.
		/// </returns>
		/// <remarks>
		///   The EnumWindows function does not enumerate child windows, with the exception of a few top-level windows owned by the system that have the WS_CHILD style.
		///   This function is more reliable than calling the GetWindow function in a loop.
		///   An application that calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window that has been destroyed.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool EnumWindows( EnumWindowsProc callback, IntPtr lParam );

		/// <summary>
		///   Retrieves the name of the class to which the specified window belongs.
		/// </summary>
		/// <param name="windowHandle">A handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="classNameBuffer">Buffer to contain the class name string.</param>
		/// <param name="bufferLength">
		///   The length of the classNameBuffer, in characters. The buffer must be large enough to include the terminating null character;
		///   otherwise, the class name string is truncated to maxBufferCount - 1 characters.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is the number of characters copied to the buffer, not including the terminating null character.
		///   If the function fails, the return value is zero. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true, CharSet = CharSet.Auto )]
		public static extern int GetClassName( IntPtr windowHandle, StringBuilder classNameBuffer, int bufferLength );

		/// <summary>
		///   Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
		/// </summary>
		/// <param name="windowHandle">A handle to the window.</param>
		/// <param name="windowPlacement">
		///   A WindowPlacement structure that receives the show state and position information.
		///   Before calling GetWindowPlacement, set the length member to sizeof( WindowPlacement ).
		///   GetWindowPlacement fails if WindowPlacement's length is not set correctly.
		/// </param>
		/// <remakrs>
		///   The flags member of WindowPlacement retrieved by this function is always zero.
		///   If the window identified by the windowHandle parameter is maximized, the showCmd member is SW_SHOWMAXIMIZED.
		///   If the window is minimized, showCmd is SW_SHOWMINIMIZED. Otherwise, it is SW_SHOWNORMAL.
		///	  The length member of WindowPlacement must be set to sizeof( WindowPlacement ).
		///   If this member is not set correctly, the function returns FALSE.
		///   For additional remarks on the proper use of window placement coordinates, see WindowPlacement.
		/// </remakrs>
		/// <returns>
		///   If the function succeeds, the return value is true.
		///   If the function fails, the return value is false. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool GetWindowPlacement( IntPtr windowHandle, ref WindowPlacement windowPlacement );

		/// <summary>
		///   Copies the text of the specified window's title bar (if it has one) into a buffer.
		///   If the specified window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another application.
		/// </summary>
		/// <param name="windowHandle">A handle to the window or control containing the text.</param>
		/// <param name="windowTextBuffer">
		///   The buffer that will receive the text.
		///   If the string is as long or longer than the buffer, the string is truncated and terminated with a null character.
		/// </param>
		/// <param name="bufferLength">
		///   The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character.
		///   If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero.
		///   To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true, CharSet = CharSet.Auto )]
		public static extern int GetWindowText( IntPtr windowHandle, StringBuilder windowTextBuffer, int bufferLength );

		/// <summary>
		///   Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar).
		///   If the specified window is a control, the function retrieves the length of the text within the control.
		///   However, GetWindowTextLength cannot retrieve the length of the text of an edit control in another application.
		/// </summary>
		/// <param name="windowHandle">A handle to the window or control.</param>
		/// <returns>
		///   If the function succeeds, the return value is the length, in characters, of the text. Under certain conditions,
		///   this value may actually be greater than the length of the text. For more information, see the following Remarks section.
		/// </returns>
		/// <remarks>
		///   If the target window is owned by the current process, GetWindowTextLength causes a WM_GETTEXTLENGTH message to be sent to the specified window or control.
		///   Under certain conditions, the GetWindowTextLength function may return a value that is larger than the actual length of the text.
		///   This occurs with certain mixtures of ANSI and Unicode, and is due to the system allowing for the possible existence of double-byte character set (DBCS)
		///   characters within the text. The return value, however, will always be at least as large as the actual length of the text;
		///   you can thus always use it to guide buffer allocation. This behavior can occur when an application uses both ANSI functions and common dialogs, which use Unicode.
		///   It can also occur when an application uses the ANSI version of GetWindowTextLength with a window whose window procedure is Unicode,
		///   or the Unicode version of GetWindowTextLength with a window whose window procedure is ANSI. For more information on ANSI and ANSI functions,
		///   see Conventions for Function Prototypes.
		///   To obtain the exact length of the text, use the WM_GETTEXT, LB_GETTEXT, or CB_GETLBTEXT messages, or the GetWindowText function.
		/// </remarks>
		[DllImport( Dll )]
		public static extern int GetWindowTextLength( IntPtr windowHandle );

		/// <summary>
		///   Retrieves the identifier of the thread that created the specified window and, optionally,
		///   the identifier of the process that created the window.
		/// </summary>
		/// <param name="windowHandle">A handle to the window.</param>
		/// <param name="processId">
		///   A pointer to a variable that receives the process identifier.
		/// </param>
		/// <returns>The return value is the identifier of the thread that created the window.</returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern int GetWindowThreadProcessId( IntPtr windowHandle, ref int processId );

		/// <summary>
		///   Determines the visibility state of the specified window.
		/// </summary>
		/// <param name="windowHandle">A handle to the window to be tested.</param>
		/// <returns>
		///   If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero.
		///   Otherwise, the return value is zero.
		///   Because the return value specifies whether the window has the WS_VISIBLE style, it may be nonzero even if the window is totally obscured by other windows.
		/// </returns>
		/// <remarks>
		///   The visibility state of a window is indicated by the WS_VISIBLE style bit. When WS_VISIBLE is set,
		///   the window is displayed and subsequent drawing into it is displayed as long as the window has the WS_VISIBLE style.
		///   Any drawing to a window with the WS_VISIBLE style will not be displayed if the window is obscured by other windows or is clipped by its parent window.
		/// </remarks>
		[DllImport( Dll )]
		public static extern bool IsWindowVisible( IntPtr windowHandle );
	}
}
