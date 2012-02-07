using System.Runtime.InteropServices;


namespace Whathecode.System.Windows.Interop
{
	partial class User32
	{
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
			ShowMInimizedNoActivate = 7,
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
	}
}
