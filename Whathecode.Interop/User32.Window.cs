using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains User32.dll definitions relating to window operations.
	/// </summary>
	public static partial class User32
	{
		#region Types

		/// <summary>
		///   Contains information about the placement of a window on the screen.
		/// </summary>
		/// <remarks>
		///   If the window is a top-level window that does not have the WS_EX_TOOLWINDOW window style,
		///   then the coordinates represented by the following members are in workspace coordinates:
		///   MinPosition, MaxPosition, and NormalPosition. Otherwise, these members are in screen coordinates.
		///   Workspace coordinates differ from screen coordinates in that they take the locations and sizes of application toolbars
		///   (including the taskbar) into account. Workspace coordinate (0,0) is the upper-left corner of the workspace area,
		///   the area of the screen not being used by application toolbars.
		///   The coordinates used in a WindowPlacement structure should be used only by the GetWindowPlacement and SetWindowPlacement functions.
		///   Passing workspace coordinates to functions which expect screen coordinates (such as SetWindowPos)
		///   will result in the window appearing in the wrong location. For example, if the taskbar is at the top of the screen,
		///   saving window coordinates using GetWindowPlacement and restoring them using SetWindowPos causes the window
		///   to appear to "creep" up the screen.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct WindowPlacement
		{
			/// <summary>
			///   The length of the structure, in bytes.
			///   Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof( WindowPlacement ).
			///   GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
			/// </summary>
			public int Length;
			/// <summary>
			///   The flags that control the position of the minimized window and the method by which the window is restored.
			///   TODO: Add the possible values as constants.
			/// </summary>
			public int Flags;
			/// <summary>
			///   The current show state of the window. This can be any value of <see cref="WindowState" />.
			/// </summary>
			public int ShowCommand;
			/// <summary>
			///   The coordinates of the window's upper-left corner when the window is minimized.
			/// </summary>
			public Point MinPosition;
			/// <summary>
			///   The coordinates of the window's upper-left corner when the window is maximized.
			/// </summary>
			public Point MaxPosition;
			/// <summary>
			///   The window's coordinates when the window is in the restored position.
			/// </summary>
			public Rectangle NormalPosition;


			/// <summary>
			///   Gets the default correctly initialized (empty) value.
			/// </summary>
			public static WindowPlacement Default
			{
				get
				{
					var result = new WindowPlacement();
					result.Length = Marshal.SizeOf( result );

					return result;
				}
			}   
		}


		/// <summary>
		///   The show state of the window.
		/// </summary>
		public enum WindowState
		{
			/// <summary>
			///   Hides the window and activates another window.
			/// </summary>
			Hide = 0,
			/// <summary>
			///   Activates and displays a window.
			///   If the window is minimized or maximized, the system restores it to its original size and position.
			///   An application should specify this flag when displaying the window for the first time.
			/// </summary>
			ShowNormal = 1,
			/// <summary>
			///   Activates the window and displays it as a minimized window.
			/// </summary>
			ShowMinimized = 2,
			/// <summary>
			///   Activates the window and displays it as a maximized window.
			/// </summary>
			ShowMaximized = 3,
			/// <summary>
			///   Maximizes the specified window.
			/// </summary>
			Maximized = 3,
			/// <summary>
			///   Displays a window in its most recent size and position.
			///   This value is similar to ShowNormal, except the window is not activated.
			/// </summary>
			ShowNormalNoActivate = 4,
			/// <summary>
			///   Activates the window and displays it in its current size and position.
			/// </summary>
			Show = 5,
			/// <summary>
			///   Minimizes the specified window and activates the next top-level window in the z-order.
			/// </summary>
			Minimize = 6,
			/// <summary>
			///   Displays the window as a minimized window. This value is similar to ShowMinimized, except the window is not activated.
			/// </summary>
			ShowMinimizedNoActivate = 7,
			/// <summary>
			///   Displays the window in its current size and position. This value is similar to Show, except the window is not activated.
			/// </summary>
			ShowNoActivate = 8,
			/// <summary>
			///   Activates and displays the window.
			///   If the window is minimized or maximized, the system restores it to its original size and position.
			///   An application should specify this flag when restoring a minimized window.
			/// </summary>
			Restore = 9,
			/// <summary>
			///   Sets the show state based on the value specified in the STARTUPINFO structure
			///   passed to the CreateProcess function by the program that started the application.
			/// </summary>
			ShowDefault = 10,
			/// <summary>
			///   Minimizes a window, even if the thread that owns the window is not responding.
			///   This flag should only be used when minimizing windows from a different thread.
			/// </summary>
			ForceMinimize = 11
		}

		/// <summary>
		///   The relationship between the specified window and the window whose handle is to be retrieved. This parameter can be one of the following values.
		/// </summary>
		public enum WindowRelationship
		{
			/// <summary>
			///   The retrieved handle identifies the window of the same type that is highest in the Z order.
			///   If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window.
			///   If the specified window is a child window, the handle identifies a sibling window.
			/// </summary>
			First = 0,
			/// <summary>
			///   The retrieved handle identifies the window of the same type that is lowest in the Z order.
			///   If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window.
			///   If the specified window is a child window, the handle identifies a sibling window.
			/// </summary>
			Last = 1,
			/// <summary>
			///   The retrieved handle identifies the window below the specified window in the Z order.
			///   If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window.
			///   If the specified window is a child window, the handle identifies a sibling window.
			/// </summary>
			Next = 2,
			/// <summary>
			///   The retrieved handle identifies the window above the specified window in the Z order.
			///   If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window.
			///   If the specified window is a child window, the handle identifies a sibling window.
			/// </summary>
			Previous = 3,
			/// <summary>
			///   The retrieved handle identifies the specified window's owner window, if any.
			/// </summary>
			Owner = 4,
			/// <summary>
			///   The retrieved handle identifies the child window at the top of the Z order, if the specified window is a parent window; otherwise, the retrieved handle is IntPtr.Zero.
			///   The function examines only child windows of the specified window. It does not examine descendant windows.
			/// </summary>
			Child = 5,
			/// <summary>
			///   The retrieved handle identifies the enabled popup window owned by the specified window (the search uses the first such window found using <see cref="Next" />);
			///   otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window.
			/// </summary>
			EnablePopup = 6
		}

		/// <summary>
		///   Can be used by GetWindowLongPtr to retrieve information from extra window memory.
		///   TODO: There are some extra options for dialog boxes. Do we ever need them?
		/// </summary>
		public enum GetWindowLongOptions
		{
			/// <summary>
			///   Retrieves the extended window styles.
			/// </summary>
			ExtendedStyles = -20,
			/// <summary>
			///   Retrieves a handle to the application instance.
			/// </summary>
			InstanceHandle = -6,
			/// <summary>
			///   Retrieves a handle to the parent window, if there is one.
			/// </summary>
			ParentWindowHandle = -8,
			/// <summary>
			///   Retrieves the identifier of the window.
			/// </summary>
			Identifier = -12,
			/// <summary>
			///   Retrieves the window styles.
			/// </summary>
			Styles = -16,
			/// <summary>
			///   Retrieves the user data associated with the window. This data is intended for use by the application that created the window. Its value is initially zero.
			/// </summary>
			UserData = -21,
			/// <summary>
			///   Retrieves the pointer to the window procedure, or a handle representing the pointer to the window procedure. You must use the CallWindowProc function to call the window procedure.
			/// </summary>
			Procedure = -4
		}

		/// <summary>
		///   The possible extended window styles which can be retrieved using GetWindowLongPtr.
		/// </summary>
		public enum ExtendedWindowStyles : long
		{
			/// <summary>
			///   The window has a border with a sunken edge.
			/// </summary>
			ClientEdge = 0x00000200L,
			/// <summary>
			///   The window has a three-dimensional border style intended to be used for items that do not accept user input.
			/// </summary>
			StaticEdge = 0x00020000L,
			/// <summary>
			///   The window has a border with a raised edge.
			/// </summary>
			WindowEdge = 0x00000100L,
			/// <summary>
			///   The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the style parameter.
			/// </summary>
			DialogModalFrame = 0x00000001L,
			/// <summary>
			///   The title bar of the window includes a question mark. When the user clicks the question mark, the cursor changes to a question mark with a pointer.
			///   If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure,
			///   which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
			///   ContextHelp cannot be used with the WS_MINIMIZEBOX or WS_MAXIMIZEBOX styles.
			/// </summary>
			ContextHelp = 0x00000400L,
			/// <summary>
			///   The window accepts drag-drop files.
			/// </summary>
			AcceptsDragAndDrop = 0x00000010L,
			/// <summary>
			///   Forces a top-level window onto the taskbar when the window is visible.
			/// </summary>
			ForceVisibleOnTaskbar = 0x00040000L,
			/// <summary>
			///   The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
			/// </summary>
			Topmost = 0x00000008L,
			/// <summary>
			///   The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
			///   The window appears transparent because the bits of underlying sibling windows have already been painted. To achieve transparency without these restrictions, use the SetWindowRgn function.
			/// </summary>
			Transparent = 0x00000020L,
			/// <summary>
			///   Paints all descendants of a window in bottom-to-top painting order using double-buffering.
			///   For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
			///   Windows 2000:  This style is not supported.
			/// </summary>
			Composited = 0x02000000L,
			/// <summary>
			///   The window itself contains child windows that should take part in dialog box navigation.
			///   If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
			/// </summary>
			ControlParent = 0x00010000L,
			/// <summary>
			///   The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
			///   Windows 8: The LayeredWindow style is supported for top-level windows and child windows. Previous Windows versions support WS_EX_LAYERED only for top-level windows.
			/// </summary>
			LayeredWindow = 0x00080000,
			/// <summary>
			///   The window is a MDI child window.
			/// </summary>
			MdiChild = 0x00000040L,
			/// <summary>
			///   A top-level window created with this style does not become the foreground window when the user clicks it.
			///   The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
			///   To activate the window, use the SetActiveWindow or SetForegroundWindow function. The window does not appear on the taskbar by default.
			///   To force the window to appear on the taskbar, use the ForceVisibleOnTaskbar style.
			/// </summary>
			NoActivate = 0x08000000L,
			/// <summary>
			///   The window does not pass its window layout to its child windows.
			/// </summary>
			NoInheritLayout = 0x00100000L,
			/// <summary>
			///   The child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
			/// </summary>
			NoParentNotify = 0x00000004L,
			/// <summary>
			///   The window does not render to a redirection surface. This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
			/// </summary>
			NoRedirectionBitmap = 0x00200000L,
			/// <summary>
			///   The window is intended to be used as a floating toolbar.
			///   A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
			///   A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
			///   If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
			/// </summary>
			ToolWindow = 0x00000080L,
			/// <summary>
			///   The window is an overlapped window.
			/// </summary>
			OverlappedWindow = WindowEdge | ClientEdge,
			/// <summary>
			///   The window is palette window, which is a modeless dialog box that presents an array of commands.
			/// </summary>
			PaletteWindow = WindowEdge | ToolWindow | Topmost,
			/// <summary>
			///   The window has generic left-aligned properties. This is the default.
			/// </summary>
			LeftAligned = 0x00000000L,
			/// <summary>
			///   The window has generic "right-aligned" properties. This depends on the window class.
			///   This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
			///   Using the RightAligned style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively.
			///   Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
			/// </summary>
			RightAligned = 0x00001000L,
			/// <summary>
			///   The window text is displayed using left-to-right reading-order properties. This is the default.
			/// </summary>
			LeftToRightReading = 0x00000000L,
			/// <summary>
			///   If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties.
			///   For other languages, the style is ignored.
			/// </summary>
			RightToLeftReading = 0x00002000L,
			/// <summary>
			///   If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal origin of the window is on the right edge.
			///   Increasing horizontal values advance to the left.
			/// </summary>
			LayoutRightToLeft = 0x00400000L,
			/// <summary>
			///   If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area.
			///   For other languages, the style is ignored.
			/// </summary>
			LeftScrollbar = 0x00004000L,
			/// <summary>
			///   The vertical scroll bar (if present) is to the right of the client area. This is the default.
			/// </summary>
			RightScrollbar = 0x00000000L,
		}

		/// <summary>
		///   Can be used by DeferWindowPos to determine the Z order of the window being positioned, when set as the insertAfterWindow parameter.
		/// </summary>
		public enum InsertAfterWindow
		{
			/// <summary>
			///   Places the window at the bottom of the Z order. If the windowHandle parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
			/// </summary>
			Bottom = 1,
			/// <summary>
			///   Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
			/// </summary>
			BehindTopMost = -2,
			/// <summary>
			///   Places the window at the top of the Z order.
			/// </summary>
			Top = 0,
			/// <summary>
			///   Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
			/// </summary>
			TopMost = -1
		}

		/// <summary>
		///   A combination of the following values affect the size and position of the windows when using DeferWindowPos.
		/// </summary>
		[Flags]
		public enum DeferWindowPosCommands
		{
			None = 0x0000,
			/// <summary>
			///   Retains the current size (ignores the width and height parameters).
			/// </summary>
			NoResize = 0x0001,
			/// <summary>
			///   Retains the current position (ignores the x and y parameters).
			/// </summary>
			NoMove = 0x0002,
			/// <summary>
			///   Retains the current Z order (ignores the insertAfterWindow parameter).
			/// </summary>
			NoZOrder = 0x0004,
			/// <summary>
			///   Does not redraw changes. If this flag is set, no repainting of any kind occurs.
			///   This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved.
			///   When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
			/// </summary>
			NoRedraw = 0x0008,
			/// <summary>
			///   Does not activate the window.
			///   If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the insertAfterWindow parameter).
			/// </summary>
			NoActivate = 0x0010,
			/// <summary>
			///   Draws a frame (defined in the window's class description) around the window.
			/// </summary>
			DrawFrame = 0x0020,
			/// <summary>
			///   Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
			/// </summary>
			FrameChanged = 0x0020,
			/// <summary>
			///   Displays the window.
			/// </summary>
			ShowWindow = 0x0040,
			/// <summary>
			///   Hides the window.
			/// </summary>
			HideWindow = 0x0080,
			/// <summary>
			///   Discards the entire contents of the client area.
			///   If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
			/// </summary>
			NoCopyBits = 0x0100,
			/// <summary>
			///   Does not change the owner window's position in the Z order.
			/// </summary>
			NoOwnerZOrder = 0x0200,
			/// <summary>
			///   Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
			/// </summary>
			NoSendChanging = 0x0400
		}

		/// <summary>
		///   Defines the behavior of the <see cref="SendMessageTimeout" /> function.
		/// </summary>
		[Flags]
		public enum SendMessageTimeoutFlags : uint
		{
			/// <summary>
			///   The calling thread is not prevented from processing other requests while waiting for the function to return.
			/// </summary>
			Normal = 0x0000,
			/// <summary>
			///   Prevents the calling thread from processing any other requests until the function returns.
			/// </summary>
			Block = 0x0001,
			/// <summary>
			///   The function returns without waiting for the time-out period to elapse if the receiving thread appears to not respond or "hangs."
			/// </summary>
			AbortIfHung = 0x0002,
			/// <summary>
			///   The function does not enforce the time-out period as long as the receiving thread is processing messages.
			/// </summary>
			NoTimeoutIfNotHung = 0x0008,
			/// <summary>
			///   The function should return 0 if the receiving window is destroyed or its owning thread dies while the message is being processed.
			/// </summary>
			ErrorOnExit = 0x0020
		}

		/// <summary>
		///   The message is posted to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows.
		///   The message is not posted to child windows.
		/// </summary>
		public static readonly IntPtr BroadcastToAllWindows = new IntPtr( 0xffff );

		#endregion // Types


		#region Functions

		/// <summary>
		///   An application-defined callback function used with functions enumerating windows.
		///   It receives window handles. The WNDENUMPROC type defines a pointer to this callback function.
		///   EnumWindowsProc is a placeholder for the application-defined function name.
		/// </summary>
		/// <param name="windowHandle">A handle to a top-level window.</param>
		/// <param name="lParam">The application-defined value given in EnumWindows or EnumDesktopWindows.</param>
		/// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
		/// <remarks>An application must register this callback function by passing its address to EnumWindows or EnumDesktopWindows.</remarks>
		public delegate bool EnumWindowsProc( IntPtr windowHandle, IntPtr lParam );


		/// <summary>
		///   Enumerates the child windows that belong to the specified parent window by passing the handle to each child window,
		///   in turn, to an application-defined callback function.
		///   EnumChildWindows continues until the last child window is enumerated or the callback function returns FALSE.
		/// </summary>
		/// <param name="windowHandle">
		///   A handle to the parent window whose child windows are to be enumerated.
		///   If this parameter is IntPtr.Zero, this function is equivalent to EnumWindows.
		/// </param>
		/// <param name="callback">A pointer to an application-defined callback function. For more information, see <see cref="EnumWindowsProc" />.</param>
		/// <param name="lParam">An application-defined value to be passed to the callback function.</param>
		/// <returns>The return value is not used.</returns>
		/// <remarks>
		///   If a child window has created child windows of its own, EnumChildWindows enumerates those windows as well.
		///   A child window that is moved or repositioned in the Z order during the enumeration process will be properly enumerated.
		///   The function does not enumerate a child window that is destroyed before being enumerated or that is created during the enumeration process.
		/// </remarks>
		[DllImport( Dll )]
		public static extern bool EnumChildWindows( IntPtr windowHandle, EnumWindowsProc callback, IntPtr lParam );

		/// <summary>
		///   Enumerates all nonchild windows associated with a thread by passing the handle to each window, in turn, to an application-defined callback function.
		///   EnumThreadWindows continues until the last window is enumerated or the callback function returns FALSE.
		///   To enumerate child windows of a particular window, use the EnumChildWindows function.
		/// </summary>
		/// <param name="threadId">The identifier of the thread whose windows are to be enumerated.</param>
		/// <param name="callback">A pointer to an application-defined callback function. For more information, see <see cref="EnumWindowsProc" />.</param>
		/// <param name="lParam">An application-defined value to be passed to the callback function.</param>
		/// <returns>
		///   If the callback function returns TRUE for all windows in the thread specified by <see cref="threadId" />, the return value is TRUE.
		///   If the callback function returns FALSE on any enumerated window, or if there are no windows found in the thread specified by <see cref="threadId" />,
		///   the return value is FALSE.
		/// </returns>
		[DllImport( Dll )]
		public static extern bool EnumThreadWindows( uint threadId, EnumWindowsProc callback, IntPtr lParam );

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
		///   Examines the Z order of the child windows associated with the specified parent window and retrieves a handle to the child window at the top of the Z order.
		/// </summary>
		/// <param name="windowHandle">A handle to the parent window whose child windows are to be examined. If this parameter is IntPtr.Zero, the function returns a handle to the window at the top of the Z order.</param>
		/// <returns>
		///   If the function succeeds, the return value is a handle to the child window at the top of the Z order. If the specified window has no child windows, the return value is IntPtr.Zero.
		///   To get extended error information, use the GetLastWin32Error function.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern IntPtr GetTopWindow( IntPtr windowHandle );

		/// <summary>
		///   Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
		/// </summary>
		/// <remarks>
		///   The EnumChildWindows function is more reliable than calling GetWindow in a loop.
		///   An application that calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window that has been destroyed.
		/// </remarks>
		/// <param name="windowHandle">A handle to a window. The window handle retrieved is relative to this window, based on the value of the relationship parameter.</param>
		/// <param name="relationship">The relationship between the specified window and the window whose handle is to be retrieved. This parameter can be one of the following values.</param>
		/// <returns>
		///   If the function succeeds, the return value is a window handle. If no window exists with the specified relationship to the specified window, the return value is IntPtr.Zero.
		///   To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern IntPtr GetWindow( IntPtr windowHandle, WindowRelationship relationship );

		/// <summary>
		///   Retrieves a handle to the desktop window. The desktop window covers the entire screen.
		///   The desktop window is the area on top of which other windows are painted.
		/// </summary>
		/// <returns>A handle to the desktop window.</returns>
		[DllImport( Dll )]
		public static extern IntPtr GetDesktopWindow();

		/// <summary>
		///   Retrieves a handle to the Shell's desktop window.
		/// </summary>
		/// <returns>The handle of the Shell's desktop window. If no Shell process is present, the return value is NULL.</returns>
		[DllImport( Dll )]
		public static extern IntPtr GetShellWindow();

		/// <summary>
		///   Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
		///   
		///   Note: To write code that is compatible with both 32-bit and 64-bit versions of Windows, use GetWindowLongPtr.
		///         When compiling for 32-bit Windows, GetWindowLongPtr is defined as a call to the GetWindowLong function.
		/// </summary>
		/// <remarks>Reserve extra window memory by specifying a nonzero value in the cbWndExtra member of the WNDCLASSEX structure used with the RegisterClassEx function.</remarks>
		/// <param name="windowHandle">A handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="index">
		///    The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.
		///    To retrieve any other value, specify one of the values as specified in <see cref="GetWindowLongOptions" />.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is the requested value.
		///   If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error" />.
		///   If SetWindowLong or SetWindowLongPtr has not been called previously, <see cref="GetWindowLongPtr" /> returns zero for values in the extra window or class memory.
		/// </returns>
		public static IntPtr GetWindowLongPtr( IntPtr windowHandle, int index )
		{
			// GetWindowLongPtr is only supported by Win64. By checking the pointer size the correct function can be called.
			return IntPtr.Size == 8
				? GetWindowLongPtr64( windowHandle, index )
				: new IntPtr( GetWindowLongPtr32( windowHandle, index ) );
		}
		[DllImport( Dll, EntryPoint="GetWindowLong", SetLastError = true )]
		static extern int GetWindowLongPtr32( IntPtr windowHandle, int index );
		[DllImport( Dll, EntryPoint="GetWindowLongPtr", SetLastError = true )]
		static extern IntPtr GetWindowLongPtr64( IntPtr windowHandle, int index );

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
		public static extern uint GetWindowThreadProcessId( IntPtr windowHandle, out uint processId );

		/// <summary>
		///   Determines whether the specified window handle identifies an existing window.
		/// </summary>
		/// <param name="windowHandle">A handle to the window to be tested.</param>
		/// <returns>
		///   If the window handle identifies an existing window, the return value is true.
		///   If the window handle does not identify an existing window, the return value is false.
		/// </returns>
		/// <remarks>
		///   A thread should not use IsWindow for a window that it did not create because the window could be destroyed after this function was called.
		///   Further, because window handles are recycled the handle could even point to a different window.
		/// </remarks>
		[DllImport( Dll )]
		public static extern bool IsWindow( IntPtr windowHandle );

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

		/// <summary>
		///   Sets the specified window's show state.
		/// </summary>
		/// <param name="windowHandle">A handle to the window.</param>
		/// <param name="showCommand">
		///   Controls how the window is to be shown. This parameter is ignored the first time an application calls ShowWindow,
		///   if the program that launched the application provides a STARTUPINFO structure.
		///   Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain function in its showCommand parameter.
		///   In subsequent calls, this parameter can be any Show... value of <see cref="WindowState" />.
		/// </param>
		/// <returns>If the window was previously visible, the return value is true. If the window was previously hidden, the return value is false.</returns>
		/// <remarks>
		///   To perform certain special effects when showing or hiding a window, use AnimateWindow.
		///   The first time an application calls ShowWindow, it should use the WinMain function's showCommand parameter as its showCommand parameter.
		///   Subsequent calls to ShowWindow must use one of the Show... values of <see cref="WindowState" />,
		///   instead of the one specified by the WinMain function's showCommand parameter.
		///   As noted in the discussion of the showCommand parameter, the showCommand value is ignored in the first call to ShowWindow
		///   if the program that launched the application specifies startup information in the structure.
		///   In this case, ShowWindow uses the information specified in the STARTUPINFO structure to show the window.
		///   On subsequent calls, the application must call ShowWindow with showCommand set to ShowDefault
		///   to use the startup information provided by the program that launched the application.
		///   This behavior is designed for the following situations:
		///   - Applications create their main window by calling CreateWindow with the WS_VISIBLE flag set.
		///   - Applications create their main window by calling CreateWindow with the WS_VISIBLE flag cleared,
		///     and later call ShowWindow with the Show flag set to make it visible.
		/// </remarks>
		[DllImport( Dll )]
		public static extern bool ShowWindow( IntPtr windowHandle, WindowState showCommand );

		/// <summary>
		///   Sets the show state of a window created by a different thread.
		/// </summary>
		/// <param name = "windowHandle">A handle to the window.</param>
		/// <param name = "showCommand">Controls how the window is to be shown. For a list of possible values, see the description of the ShowWindow function.</param>
		/// <returns>
		///   If the window was previously visible, the return value is nonzero.
		///   If the window was previously hidden, the return value is zero.
		/// </returns>
		/// <remarks>
		///   This function posts a show-window event to the message queue of the given window.
		///   An application can use this function to avoid becoming nonresponsive while waiting for a nonresponsive application to finish processing a show-window event.
		/// </remarks>
		[DllImport( Dll )]
		public static extern bool ShowWindowAsync( IntPtr windowHandle, WindowState showCommand );

		/// <summary>
		///   Retrieves a handle to the foreground window (the window with which the user is currently working).
		///   The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
		/// </summary>
		/// <returns>
		///   The return value is a handle to the foreground window.
		///   The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
		/// </returns>
		[DllImport( Dll )]
		public static extern IntPtr GetForegroundWindow();

		/// <summary>
		///   Allocates memory for a multiple-window- position structure and returns the handle to the structure.
		/// </summary>
		/// <remarks>
		///   The multiple-window-position structure is an internal structure; an application cannot access it directly.
		///   DeferWindowPos fills the multiple-window-position structure with information about the target position for one or more windows about to be moved.
		///   The EndDeferWindowPos function accepts the handle to this structure and repositions the windows by using the information stored in the structure.
		///   If any of the windows in the multiple-window- position structure have the DeferWindowPosCommands.HideWindow or DeferWindowPosCommands.ShowWindow flag set, none of the windows are repositioned.
		///   If the system must increase the size of the multiple-window- position structure beyond the initial size specified by the numberOfWindows parameter but cannot allocate enough memory to do so,
		///   the system fails the entire window positioning sequence (BeginDeferWindowPos, DeferWindowPos, and EndDeferWindowPos).
		///   By specifying the maximum size needed, an application can detect and process failure early in the process.
		/// </remarks>
		/// <param name="numberOfWindows">The initial number of windows for which to store position information. The DeferWindowPos function increases the size of the structure, if necessary.</param>
		/// <returns>
		///   If the function succeeds, the return value identifies the multiple-window-position structure.
		///   If insufficient system resources are available to allocate the structure, the return value is IntPtr.Zero. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern IntPtr BeginDeferWindowPos( int numberOfWindows );

		/// <summary>
		///   Updates the specified multiple-window – position structure for the specified window. The function then returns a handle to the updated structure.
		///   The EndDeferWindowPos function uses the information in this structure to change the position and size of a number of windows simultaneously.
		///   The BeginDeferWindowPos function creates the structure.
		/// </summary>
		/// <remarks>
		///   If a call to DeferWindowPos fails, the application should abandon the window-positioning operation and not call EndDeferWindowPos.
		/// 
		///   If DeferWindowPosCommands.NoZOrder is not specified, the system places the window identified by the windowHandle parameter in the position following the window identified by the instertAfterWindow parameter.
		///   If insertAfterWindow is IntPtr.Zero or InsertAfterWindow.Top, the system places the windowHandle window at the top of the Z order.
		///   If insertAfterWindow is set to InsertAfterWindow.Bottom, the system places the windowHandle window at the bottom of the Z order.
		/// 
		///   All coordinates for child windows are relative to the upper-left corner of the parent window's client area.
		/// 
		///   A window can be made a topmost window either by setting insertAfterWindow to the InsertAfterWindow.TopMost flag and ensuring that the DeferWindowPosCommands.NoZOrder flag is not set,
		///   or by setting the window's position in the Z order so that it is above any existing topmost windows.
		///   When a non-topmost window is made topmost, its owned windows are also made topmost. Its owners, however, are not changed.
		/// 
		///   If neither the DeferWindowPosCommands.NoActivate nor DeferWindowPosCommands.NoZOrder flag is specified
		///   (that is, when the application requests that a window be simultaneously activated and its position in the Z order changed),
		///   the value specified in insertAfterWindow is used only in the following circumstances:
		///     - Neither the InsertAfterWindow.TopMost nor InsertAfterWindow.BehindTopMost flag is specified in insertAfterWindow.
		///     - The window identified by windowHandle is not the active window.
		/// 
		///   An application cannot activate an inactive window without also bringing it to the top of the Z order.
		///   An application can change an activated window's position in the Z order without restrictions, or it can activate a window and then move it to the top of the topmost or non-topmost windows.
		/// 
		///   A topmost window is no longer topmost if it is repositioned to the bottom (InsertAfterWindow.Bottom) of the Z order or after any non-topmost window.
		///   When a topmost window is made non-topmost, its owners and its owned windows are also made non-topmost windows.
		/// 
		///   A non-topmost window may own a topmost window, but not vice versa.
		///   Any window (for example, a dialog box) owned by a topmost window is itself made a topmost window to ensure that all owned windows stay above their owner.
		/// </remarks>
		/// <param name="windowsPositionInfo">
		///   A handle to a multiple-window – position structure that contains size and position information for one or more windows.
		///   This structure is returned by BeginDeferWindowPos or by the most recent call to DeferWindowPos.
		/// </param>
		/// <param name="windowHandle">A handle to the window for which update information is stored in the structure. All windows in a multiple-window – position structure must have the same parent.</param>
		/// <param name="insertAfterWindow">
		///   A handle to the window that precedes the positioned window in the Z order. This parameter must be a window handle or a value of <see cref="InsertAfterWindow" />.
		///   This parameter is ignored if the DeferWindowPosCommands.NoZOrder flag is set in the flags parameter.
		/// </param>
		/// <param name="x">The x-coordinate of the window's upper-left corner.</param>
		/// <param name="y">The y-coordinate of the window's upper-left corner.</param>
		/// <param name="width">The window's new width, in pixels.</param>
		/// <param name="height">The window's new height, in pixels.</param>
		/// <param name="flags">Affect the size and position of the window being positioned.</param>
		/// <returns>
		///   The return value identifies the updated multiple-window – position structure. The handle returned by this function may differ from the handle passed to the function.
		///   The new handle that this function returns should be passed during the next call to the DeferWindowPos or EndDeferWindowPos function.
		///   If insufficient system resources are available for the function to succeed, the return value is NULL. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern IntPtr DeferWindowPos(
			IntPtr windowsPositionInfo, IntPtr windowHandle, IntPtr insertAfterWindow,
			int x, int y, int width, int height,
			[MarshalAs( UnmanagedType.U4 )]DeferWindowPosCommands flags );

		/// <summary>
		///   Simultaneously updates the position and size of one or more windows in a single screen-refreshing cycle.
		/// </summary>
		/// <remarks>The EndDeferWindowPos function sends the WM_WINDOWPOSCHANGING and WM_WINDOWPOSCHANGED messages to each window identified in the internal structure.</remarks>
		/// <param name="windowPositionInfo">
		///   A handle to a multiple-window – position structure that contains size and position information for one or more windows.
		///   This internal structure is returned by the BeginDeferWindowPos function or by the most recent call to the DeferWindowPos function.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is true.
		///   If the function fails, the return value is false. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool EndDeferWindowPos( IntPtr windowPositionInfo );

		/// <summary>
		///   Retrieves the window handle to the active window attached to the calling thread's message queue.
		/// </summary>
		/// <remarks>
		///   To get the handle to the foreground window, you can use GetForegroundWindow.
		///   To get the window handle to the active window in the message queue for another thread, use GetGUIThreadInfo.
		/// </remarks>
		/// <returns>The return value is the handle to the active window attached to the calling thread's message queue. Otherwise, the return value is IntPtr.Zero.</returns>
		[DllImport( Dll )]
		public static extern IntPtr GetActiveWindow();

		/// <summary>
		///   Switches focus to the specified window and brings it to the foreground.
		/// </summary>
		/// <param name="windowHandle">A handle to the window.</param>
		/// <param name="altTabSwitch">A TRUE for this parameter indicates that the window is being switched to using the Alt/Ctl+Tab key sequence. This parameter should be FALSE otherwise.</param>
		/// <remarks>
		///   This function is typically called to maintain window z-ordering. This function was not included in the SDK headers and libraries until Windows XP with Service Pack 1 (SP1) and Windows Server 2003.
		///   If you do not have a header file and import library for this function, you can call the function using LoadLibrary and GetProcAddress.
		/// </remarks>
		[DllImport( Dll )]
		public static extern void SwitchToThisWindow( IntPtr windowHandle, bool altTabSwitch );

		/// <summary>
		///   Brings the thread that created the specified window into the foreground and activates the window.
		///   Keyboard input is directed to the window, and various visual cues are changed for the user.
		///   The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
		/// </summary>
		/// <param name="windowHandle">A handle to the window that should be activated and brought to the foreground.</param>
		/// <remarks>
		///   The system restricts which processes can set the foreground window. A process can set the foreground window only if one of the following conditions is true:
		///   <list type="bullet">
		///     <item>The process is the foreground process.</item>
		///     <item>The process was started by the foreground process.</item>
		///     <item>The process received the last input event.</item>
		///     <item>There is no foreground process.</item>
		///     <item>The process is being debugged.</item>
		///     <item>The foreground process is not a Modern Application or the Start Screen.</item>
		///     <item>The foreground is not locked (see LockSetForegroundWindow).</item>
		///     <item>The foreground lock time-out has expired (see SPI_GETFOREGROUNDLOCKTIMEOUT in SystemParametersInfo).</item>
		///     <item>No menus are active.</item>
		///   </list>
		///   An application cannot force a window to the foreground while the user is working with another window. Instead, Windows flashes the taskbar button of the window to notify the user.
		/// 
		///   A process that can set the foreground window can enable another process to set the foreground window by calling the AllowSetForegroundWindow function.
		///   The process specified by dwProcessId loses the ability to set the foreground window the next time the user generates input, unless the input is directed at that process,
		///   or the next time a process calls AllowSetForegroundWindow, unless that process is specified.
		/// 
		///   The foreground process can disable calls to SetForegroundWindow by calling the LockSetForegroundWindow function.
		/// </remarks>
		/// <returns>
		///   If the window was brought to the foreground, the return value is nonzero.
		///   If the window was not brought to the foreground, the return value is zero.
		/// </returns>
		[DllImport( Dll )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetForegroundWindow( IntPtr windowHandle );

		/// <summary>
		///   Sends the specified message to a window or windows.
		///   The <see cref="SendMessage" /> function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		///   To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function.
		///   To post a message to a thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
		/// </summary>
		/// <param name="windowHandle">
		///   A handle to the window whose window procedure will receive the message.
		/// 
		///   If this parameter is <see cref="BroadcastToAllWindows" />, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
		///   overlapped windows, and pop-up windows; but the message is not sent to child windows.
		///   Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of lesser or equal integrity level.
		/// </param>
		/// <param name="message">The message to be sent. For lists of the system-provided messages, see System-Defined Messages.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		/// <remarks>
		///   When a message is blocked by UIPI the last error, retrieved with GetLastWin32Error, is set to 5 (access denied).
		///   Applications that need to communicate using <see cref="BroadcastToAllWindows" /> should use the <see cref="RegisterWindowMessage" /> function
		///   to obtain a unique message for inter-application communication.
		///   The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)).
		///   To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
		///   If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine.
		///   If the specified window was created by a different thread, the system switches to that thread and calls the appropriate window procedure.
		///   Messages sent between threads are processed only when the receiving thread executes message retrieval code.
		///   The sending thread is blocked until the receiving thread processes the message.
		///   However, the sending thread will process incoming nonqueued messages while waiting for its message to be processed.
		///   To prevent this, use <see cref="SendMessageTimeout" /> with <see cref="SendMessageTimeoutFlags.Block" /> set.
		///   For more information on nonqueued messages, see Nonqueued Messages.
		///   An accessibility application can use <see cref="SendMessage" /> to send WM_APPCOMMAND messages to the shell to launch applications.
		///   This functionality is not guaranteed to work for other types of applications.
		/// </remarks>
		[DllImport( Dll, CharSet = CharSet.Auto, SetLastError = true )]
		public static extern IntPtr SendMessage( IntPtr windowHandle, uint message, IntPtr wParam, IntPtr lParam );

		/// <summary>
		///   Sends the specified message to one or more windows.
		/// </summary>
		/// <param name="windowHandle">
		///   A handle to the window whose window procedure will receive the message.
		/// 
		///   If this parameter is <see cref="BroadcastToAllWindows" />, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows.
		///   The function does not return until each window has timed out. Therefore, the total wait time can be up to the value of timeout multiplied by the number of top-level windows.
		/// </param>
		/// <param name="message">The message to be sent. For lists of the system-provided messages, see System-Defined Messages.</param>
		/// <param name="wParam">Any additional message-specific information.</param>
		/// <param name="lParam">Any additional message-specific information.</param>
		/// <param name="flags">The behavior of this function.</param>
		/// <param name="timeout">
		///   The duration of the time-out period, in milliseconds. If the message is a broadcast message, each window can use the full time-out period.
		///   For example, if you specify a five second time-out period and there are three top-level windows that fail to process the message, you could have up to a 15 second delay.
		/// </param>
		/// <param name="result">The result of the message processing. The value of this parameter depends on the message that is specified.</param>
		/// <remarks>
		///   The function calls the window procedure for the specified window and, if the specified window belongs to a different thread,
		///   does not return until the window procedure has processed the message or the specified time-out period has elapsed.
		///   If the window receiving the message belongs to the same queue as the current thread, the window procedure is called directly - the time-out value is ignored.
		///   This function considers that a thread is not responding if it has not called GetMessage or a similar function within five seconds.
		///   The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
		/// </remarks>
		/// <returns>
		///   If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide information about individual windows timing out if <see cref="BroadcastToAllWindows" /> is used.
		///   If the function fails or times out, the return value is 0. To get extended error information, call GetLastWin32Error.
		///   If GetLastWin32Error returns <see cref="ErrorCode.Timeout" />, then the function timed out.
		///   Windows 2000: If GetLastWin32Error returns 0, then the function timed out.
		/// </returns>
		[DllImport( Dll, CharSet = CharSet.Auto, SetLastError = true )]
		public static extern IntPtr SendMessageTimeout( IntPtr windowHandle, uint message, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result );

		/// <summary>
		///   Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
		/// </summary>
		/// <param name="message">The message to be registered.</param>
		/// <remarks>
		///   The <see cref="RegisterWindowMessage" /> function is typically used to register messages for communicating between two cooperating applications.
		///   If two different applications register the same message string, the applications return the same message value. The message remains registered until the session ends.
		///   Only use <see cref="RegisterWindowMessage" /> when more than one application must process the same message.
		///   For sending private messages within a window class, an application can use any integer in the range WM_USER through 0x7FFF.
		///   (Messages in this range are private to a window class, not to an application. For example, predefined control classes such as BUTTON, EDIT, LISTBOX, and COMBOBOX may use values in this range.)
		/// </remarks>
		/// <returns>
		///   If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.
		///   If the function fails, the return value is zero. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, CharSet = CharSet.Auto, SetLastError = true )]
		public static extern uint RegisterWindowMessage( string message );

		#endregion // Functions
	}
}
