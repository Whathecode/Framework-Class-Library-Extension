using System;
using System.Runtime.InteropServices;


namespace Whathecode.System.Windows.Interop
{
	partial class User32
	{
        /// <summary>
        /// Specifies or receives the attributes of a list-view item. This structure has been updated to support a new mask value (LVIF_INDENT) that enables item indenting. This structure supersedes the LV_ITEM structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Lvitem
        {
            public int Mask;
            public int Item;
            public int SubItem;
            public int State;
            public int StateMask;
            public IntPtr PszText; // string
            public int CchTextMax;
            public int Image;
            public IntPtr LParam;
            public int Indent;
            public int GroupId;
            public int CColumns;
            public IntPtr PuColumns;
        }
		/// <summary>
		///   The Point structure defines the x- and y- coordinates of a point.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct Point
		{
			/// <summary>
			///   The x-coordinate of the point.
			/// </summary>
			public int X;
			/// <summary>
			///   The y-coordinate of the point.
			/// </summary>
			public int Y;
		}


		/// <summary>
		///   The Rectangle structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
		/// </summary>
		/// <remarks>
		///   By convention, the right and bottom edges of the rectangle are normally considered exclusive.
		///   In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the rectangle.
		///   For example, when Rectangle is passed to the FillRect function, the rectangle is filled up to,
		///   but not including, the right column and bottom row of pixels.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct Rectangle
		{
			/// <summary>
			///   The x-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Left;
			/// <summary>
			///   The y-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Top;
			/// <summary>
			///   The x-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Right;
			/// <summary>
			///   The y-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Bottom;
		}


		#region Window types

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
        /// ListView API Constants
        /// </summary>
        public const uint LVM_FIRST = 0x1000;
        public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const uint LVM_GETITEMW = LVM_FIRST + 75;
        public const uint LVM_SETITEMPOSITION = 0x1000 + 15;
        public const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;

        public const int LVIF_TEXT = 0x0001;
		#endregion // Window types


		#region Input types

		/// <summary>
		///   Used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
		/// </summary>
		/// <remarks>
		///  InputEventType.Keyboard supports nonkeyboard input methods, such as handwriting recognition or voice recognition, as if it were text input by using the KeyboardInputFlags.Unicode flag.
		///  For more information, see the remarks section of <see cref="KeyboardInput" />.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct Input
		{
			/// <summary>
			///   The type of the input event.
			/// </summary>
			public InputEventType Type;

			/// <summary>
			///   The information related to the input event, dependent on the <see cref="InputEventType" />.
			/// </summary>
			public InputUnion InputInformation;

			public static int Size
			{
				get { return Marshal.SizeOf( typeof( Input ) ); }
			}
		}

		/// <summary>
		///   The information related to an input event.
		/// </summary>
		/// <remarks>
		///   By separating the union into its own structure, rather than placing the fields directly in the <see cref="Input" /> structure,
		///   we assure that the .Net structure will have the correct alignment on both 32 and 64 bit.
		/// </remarks>
		[StructLayout( LayoutKind.Explicit )]
		public struct InputUnion
		{
			/// <summary>
			///   The information about a simulated mouse event.
			/// </summary>
			[FieldOffset( 0 )]
			public MouseInput MouseInput;
			/// <summary>
			///   The information about a simulated keyboard event.
			/// </summary>
			[FieldOffset( 0 )]
			public KeyboardInput KeyboardInput;
			/// <summary>
			///   The information about a simulated hardware event.
			/// </summary>
			[FieldOffset( 0 )]
			public HardwareInput HardwareInput;
		}

		/// <summary>
		///   The type of an input event. 
		/// </summary>
		public enum InputEventType
		{
			Mouse = 0,
			Keyboard = 1,
			Hardware = 2
		}
		
		/// <summary>
		///   Contains information about a simulated mouse event.
		/// </summary>
		/// <remarks>
		///   If the mouse has moved, indicated by MouseInputFlags.Move, X and Y specify information about that movement. The information is specified as absolute or relative integer values.
		///   If MouseInputFlags.Absolute value is specified, X and Y contain normalized absolute coordinates between 0 and 65,535.
		///   The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right corner.
		///   In a multimonitor system, the coordinates map to the primary monitor.
		///   If MouseInputFlags.VirtualDesktop is specified, the coordinates map to the entire virtual desktop.
		///   If the MouseInputFlags.Absolute value is not specified, X and Y specify movement relative to the previous mouse event (the last reported position).
		///   Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up).
		///   Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet.
		///   You can obtain and set these values using the SystemParametersInfo function.  The system applies two tests to the specified relative mouse movement.
		///   If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance.
		///   If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two,
		///   the system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct MouseInput
		{
			/// <summary>
			///   The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the Flags member.
			///   Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.
			/// </summary>
			public int X;
			/// <summary>
			///   The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the Flags member.
			///   Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.
			/// </summary>
			public int Y;
			/// <summary>
			///   If Flags contains MouseInputFlags.Wheel, then MouseData specifies the amount of wheel movement.
			///   A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
			///   One wheel click is defined as WHEEL_DELTA, which is 120.
			/// 
			///   Windows Vista: If Flags contains MouseInputFlags.HorizontalWheel, then MouseData specifies the amount of wheel movement.
			///   A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.
			///   One wheel click is defined as WHEEL_DELTA, which is 120.
			/// 
			///   If Flags does not contain MouseInputFlags.Wheel, MouseInputFlags.XDown, or MouseInputFlags.XUp, then MouseData should be zero.
			/// 
			///   If Flags contains MouseInputFlags.XDown or MouseInputFlags.XUp, then MouseData specifies which X buttons were pressed or released. This value can be interpreted using <see cref="XButton" />.
			/// </summary>
			public uint MouseData;
			/// <summary>
			///   A set of bit flags that specify various aspects of mouse motion and button clicks.
			/// </summary>
			public uint Flags;
			/// <summary>
			///   The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
			/// </summary>
			public uint TimeStamp;
			/// <summary>
			///   An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
			/// </summary>
			public IntPtr ExtraInfo;
		}		

		/// <summary>
		///   A set of bit flags that specify various aspects of mouse motion and button clicks.
		///   The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
		///   For example, if the left mouse button is pressed and held down, LeftDown is set when the left button is first pressed, but not for subsequent motions.
		///   Similarly, LeftUp is set only when the button is first released.
		///   You cannot specify both the Wheel flag and either XDown or XUp flags simultaneously, because they both require use of the MouseInput.MouseData field.
		/// </summary>
		[Flags]
		public enum MouseInputFlags
		{
			/// <summary>
			///   The X and Y members contain normalized absolute coordinates. If the flag is not set, X and Y contain relative data (the change in position since the last reported position).
			///   This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system.
			///   For further information about relative mouse motion, see the Remarks section of <see cref="MouseInput" />.
			/// </summary>
			Absolute = 0x8000,
			/// <summary>
			///   The wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in MouseInput.MouseData. Unsupported for Windows XP/2000.
			/// </summary>
			HorizontalWheel = 0x01000,
			/// <summary>
			///   Movement occurred.
			/// </summary>
			Move = 0x0001,
			/// <summary>
			///   The WM_MOUSEMOVE messages will not be coalesced. The default behavior is to coalesce WM_MOUSEMOVE messages. Unsupported for Windows XP/2000.
			/// </summary>
			MoveNoCoalesce = 0x2000,
			LeftDown = 0x0002,
			LeftUp = 0x0004,
			RightDown = 0x0008,
			RightUp = 0x0010,
			MiddleDown = 0x0020,
			MiddleUp = 0x0040,
			/// <summary>
			///   Maps coordinates to the entire desktop. Must be used with Absolute.
			/// </summary>
			VirtualDesktop = 0x4000,
			/// <summary>
			///   The wheel was moved, if the mouse has a wheel. The amount of movement is specified in MouseInput.MouseData.
			/// </summary>
			Wheel = 0x0800,
			/// <summary>
			///   An X button was pressed.
			/// </summary>
			XDown = 0x0080,
			/// <summary>
			///   An X button was released.
			/// </summary>
			XUp = 0x0100
		}

		[Flags]
		public enum XButton
		{
			/// <summary>
			///   The first X button is pressed or released.
			/// </summary>
			XButton1 = 1,
			/// <summary>
			///   The second X button is pressed or released.
			/// </summary>
			XButton2 = 2
		}

		/// <summary>
		///   Contains information about a simulated keyboard event.
		/// </summary>
		/// <remarks>
		///   <see cref="KeyboardInput" /> supports nonkeyboard-input methods—such as handwriting recognition or voice recognition—as if it were text input by using the KeyboardInputFlags.Unicode flag.
		///   If KeyboardInputFLags.Unicode is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP message to the foreground thread's message queue with wParam equal to VK_PACKET.
		///   Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with the Unicode character originally specified by ScanCode.
		///   This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window.
		///   Set the KeyboardInputFlags.ScanCode flag to define keyboard input in terms of the scan code. This is useful to simulate a physical keystroke regardless of which keyboard is currently being used.
		///   The virtual key value of a key may alter depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct KeyboardInput
		{
			/// <summary>
			///   A virtual-key code. The code must be a value in the range 1 to 254. If the Flags member specifies KeyboardInputFlags.Unicode, VirtualKey must be 0.
			/// </summary>
			public ushort VirtualKey;
			/// <summary>
			///   A hardware scan code for the key. If Flags specifies KeyboardInputFlags.Unicode, ScanCode specifies a Unicode character which is to be sent to the foreground application.
			/// </summary>
			public ushort ScanCode;
			/// <summary>
			///   Specifies various aspects of a keystroke.
			/// </summary>
			public KeyboardInputFlags Flags;
			/// <summary>
			///   The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.
			/// </summary>
			public uint TimeStamp;
			/// <summary>
			///   An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.
			/// </summary>
			public IntPtr ExtraInfo;
		}

		/// <summary>
		///   Specifies various aspects of a keystroke.
		/// </summary>
		[Flags]
		public enum KeyboardInputFlags
		{
			/// <summary>
			///   If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
			/// </summary>
			ExtendedKey = 0x0001,
			/// <summary>
			///   If specified, the key is being released. If not specified, the key is being pressed.
			/// </summary>
			KeyUp = 0x0002,
			/// <summary>
			///   If specified, the system synthesizes a VK_PACKET keystroke. The VirtualKey parameter must be zero.
			///   This flag can only be combined with the KeyUp flag. For more information, see the Remarks section of KeyboardInput.
			/// </summary>
			Unicode = 0x0004,
			/// <summary>
			///   If specified, ScanCode identifies the key and VirtualKey is ignored.
			/// </summary>
			ScanCode = 0x0008,
		}

		/// <summary>
		///   Contains information about a simulated message generated by an input device other than a keyboard or mouse.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct HardwareInput
		{
			/// <summary>
			///   The message generated by the input hardware.
			/// </summary>
			public int Message;
			/// <summary>
			///   The low-order word of the lParam parameter for uMsg.
			/// </summary>
			public short ParamL;
			/// <summary>
			///   The high-order word of the lParam parameter for uMsg.
			/// </summary>
			public short ParamH;
		}

		#endregion // Input types
	}
}
