using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Whathecode.Interop;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System.Windows
{
	/// <summary>
	///   Can find and manage application windows.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class WindowManager
	{
		enum ErrorCode
		{
			AccessDenied = Interop.ErrorCode.AccessDenied,
			InvalidWindowHandle = 0x00000578,
			InvalidMultipleWindowPositionStructure = 0x0000057D
		}


		/// <summary>
		///   Enumerates all top-level windows on the screen.
		/// </summary>
		public static List<WindowInfo> GetWindows()
		{
			var windows = new List<WindowInfo>();
			bool success = User32.EnumWindows(
				( handle, lparam ) =>
				{
					windows.Add( new WindowInfo( handle ) );
					return true;
				},
				IntPtr.Zero );

			if ( !success )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return windows;
		}

		/// <summary>
		///   Gets the desktop window in which desktop icons are displayed.
		/// </summary>
		/// <returns></returns>
		public static WindowInfo GetDesktopWindow()
		{
			Func<WindowInfo, WindowInfo> findDesktopShell =
				w => w.GetChildWindows().FirstOrDefault( c => c.GetClassName() == "SHELLDLL_DefView" );

			// Before Windows 7, the desktop window could generally be found as a direct child of the shell.
			var shellWindow = new WindowInfo( User32.GetShellWindow() );
			if ( shellWindow == null )
			{
				throw new InvalidOperationException( "No Shell process is present." );
			}
			var desktop = findDesktopShell( shellWindow );
			if ( desktop != null )
			{
				return desktop;
			}

			// Windows 7 can have cycling wallpapers enabled, in which case the desktop window can be found in a window with class "WorkerW".
			var workerWindows = GetWindows().Where( w => w.GetClassName() == "WorkerW" );
			foreach ( WindowInfo w in workerWindows )
			{
				desktop = findDesktopShell( w );
				if ( desktop != null )
				{
					return desktop;
				}
			}

			throw new NotSupportedException( "Could not find desktop window where it was expected." );
		}

		/// <summary>
		///   Retrieves the window which currently has focus.
		/// </summary>
		/// <returns>The <see cref="WindowInfo" /> of the window which currently has focus.</returns>
		public static WindowInfo GetForegroundWindow()
		{
			IntPtr windowHandle = User32.GetForegroundWindow();
			while ( windowHandle == IntPtr.Zero )
			{
				// The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
				// Wait for a new window to get focus.
				windowHandle = User32.GetForegroundWindow();
			}

			return new WindowInfo( windowHandle );
		}

		/// <summary>
		///   Retrieves the top-level window which is currently at the top.
		/// </summary>
		/// <returns>The <see cref="WindowInfo" /> of the window which is currently on top.</returns>
		public static WindowInfo GetTopWindow()
		{
			return new WindowInfo( User32.GetTopWindow( IntPtr.Zero ) );
		}

		/// <summary>
		///   Find the next window below the specified window.
		/// </summary>
		/// <returns>The next window below the specified window, or null when the specified window is at the bottom.</returns>
		public static WindowInfo GetWindowBelow( WindowInfo window )
		{
			IntPtr windowHandle = User32.GetWindow( window.Handle, User32.WindowRelationship.Next );
			return windowHandle == IntPtr.Zero ? null : new WindowInfo( windowHandle );
		}

		/// <summary>
		///   Reposition a set of windows in one operation.
		/// </summary>
		/// <param name="toPosition">The windows to reposition.</param>
		/// <param name="throwUnresponsiveException">
		///   Throw a <see cref="UnresponsiveWindowsException" /> when some of the windows become unresponsive during the operation.
		///   This exception is thrown once the total operation lasts longer than two seconds.
		///   An attempt is made to still position the windows which are responsive.
		/// </param>
		/// <param name="changeZOrder">
		///   When true, the windows's Z orders are changed to reflect the order of the toPosition list.
		///   The first item in the list will appear at the top, while the last item will appear at the bottom.
		/// </param>
		public static void RepositionWindows( List<RepositionWindowInfo> toPosition, bool throwUnresponsiveException, bool changeZOrder = false )
		{
			Action operation = () =>
			{
				bool changeVisibility = toPosition.Any( w => w.HasVisibilityChanged() );
				if ( changeVisibility )
				{
					RepositionWindowsInner( toPosition, false, true );
				}
				RepositionWindowsInner( toPosition, changeZOrder, false );
			};

			// Early out when caller decides not to check for unresponsive windows.
			if ( !throwUnresponsiveException )
			{
				operation();
				return;
			}

			// When windows don't process Win32 messages, they can lock the operation. Therefore, run operation on a separate thread and watch for a timeout.
			var waitOperation = Task.Factory.StartNew( operation );
			bool completed = waitOperation.Wait( TimeSpan.FromSeconds( 2 ) );
			if ( completed )
			{
				return;
			}

			// There might be some unresponsive windows. Skip them and try again.
			List<RepositionWindowInfo> unresponsive = toPosition.Where( w => !w.IsResponding() ).ToList();
			if ( unresponsive.Count > 0 )
			{
				RepositionWindows( toPosition.Except( unresponsive ).ToList(), true, changeZOrder );

				// Unresponsive windows are skipped and an exception is thrown indicating which windows could not be positioned.
				throw new UnresponsiveWindowsException( unresponsive.Select( w => w.ToPosition ).ToList() );
			}
		}

		/// <summary>
		///   Reposition/resize a set of windows, or change their visiblity. When changing visibilty, windows can't be repositioned/resized.
		///   This is a limitation of the underlying Win32 API.
		/// </summary>
		/// <remarks>
		///   When invalid window handles are passed, they are simply ignored.
		/// </remarks>
		/// <param name="windows">The windows to perform the operation on.</param>
		/// <param name="changeZOrder">
		///   When true and changeVisibility is not set, the windows's Z orders are changed to reflect the order of the toPosition list.
		///   The first item in the list will appear at the top, while the last item will appear at the bottom.
		/// </param>
		/// <param name="changeVisibility">When set to true only the visiblity of windows can be changed. When set to false, only the other parameters can be changed.</param>
		static void RepositionWindowsInner( ICollection<RepositionWindowInfo> windows, bool changeZOrder, bool changeVisibility )
		{
			if ( windows.Count == 0 )
			{
				// Nothing to reposition.
				return;
			}

			bool succeeded = false;
			var windowList = windows.ToList();
			while ( !succeeded )
			{
				IntPtr windowsPositionInfo = User32.BeginDeferWindowPos( windowList.Count );

				bool errorEncountered = false;
				for ( int i = 0; i < windowList.Count; ++i )
				{
					RepositionWindowInfo window = windowList[ i ];

					// Activating windows is outside of the scope of this function, otherwise it gets too complex.
					var commands = User32.DeferWindowPosCommands.NoActivate;

					if ( changeVisibility && window.HasVisibilityChanged() )
					{
						commands |= window.Visible ? User32.DeferWindowPosCommands.ShowWindow : User32.DeferWindowPosCommands.HideWindow;
					}

					if ( changeVisibility || !window.HasSizeChanged() )
					{
						commands |= User32.DeferWindowPosCommands.NoResize;
					}
					if ( changeVisibility || !window.HasPositionChanged() )
					{
						commands |= User32.DeferWindowPosCommands.NoMove;
					}
					if ( changeVisibility || (i == 0 || !changeZOrder) )
					{
						commands |= User32.DeferWindowPosCommands.NoZOrder;
						commands |= User32.DeferWindowPosCommands.NoOwnerZOrder;
					}

					windowsPositionInfo = User32.DeferWindowPos(
						windowsPositionInfo,
						window.ToPosition.Handle,
						i == 0 ? IntPtr.Zero : windowList[ i - 1 ].ToPosition.Handle,
						window.X, window.Y,
						window.Width, window.Height,
						commands );

					// Handle possible errors.
					if ( windowsPositionInfo == IntPtr.Zero )
					{
						var error = (ErrorCode)Marshal.GetLastWin32Error();
						switch ( error )
						{
							case ErrorCode.InvalidWindowHandle:
								windowList = windowList.Where( w => !w.ToPosition.IsDestroyed() ).ToList();
								break;
							case ErrorCode.InvalidMultipleWindowPositionStructure:
								// Nothing to do, EndDeferWindowPos will fail, and a new operation will be attempted.
								break;
							case ErrorCode.AccessDenied:
								windowList.RemoveAt( i );
								break;
						}

						// Try again starting over with a new iteration.
						errorEncountered = true;
						break;
					}
				}

				if ( !errorEncountered )
				{
					succeeded = User32.EndDeferWindowPos( windowsPositionInfo );
					if ( succeeded && User32.GetActiveWindow() == IntPtr.Zero )
					{
						// All windows are hidden and there is no more active window.
						// This causes a bug next time a window is shown which doesn't show up on the taskbar. Another window is shown on the taskbar, but not made visible.
						// To prevent this, activate the start bar.
						WindowInfo startBar = GetWindows().FirstOrDefault( w => w.GetClassName() == "Shell_TrayWnd" );
						if ( startBar != null )
						{
							startBar.SetForegroundWindow();
						}
					}
				}
			}
		}

		/// <summary>
		///   Fully refreshes the desktop window.
		/// </summary>
		public static void RefreshDesktop()
		{
			// TODO: Why does this work?
			Shell32.SHChangeNotify( Shell32.EventId.FileTypeAssociationChanged, Shell32.ChangeNotifyFlags.Flush, IntPtr.Zero, IntPtr.Zero );
		}
	}
}
