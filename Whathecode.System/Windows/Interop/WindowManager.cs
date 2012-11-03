using System;
using System.Collections.Generic;
using System.Linq;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Can find and manage application windows.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class WindowManager
	{
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
		///   TODO: Handle any scenarios where repositioning windows fails.
		/// </summary>
		/// <param name="toPosition">The windows to reposition.</param>
		/// <param name="changeZOrder">
		///   When true, the windows's Z orders are changed to reflect the order of the toPosition list.
		///   The first item in the list will appear at the top, while the last item will appear at the bottom.
		/// </param>
		public static void RepositionWindows( List<RepositionWindowInfo> toPosition, bool changeZOrder = false )
		{
			bool changeVisibility = toPosition.Any( w => w.HasVisibilityChanged() );
			if ( changeVisibility )
			{
				RepositionWindows( toPosition, false, true );
			}
			RepositionWindows( toPosition, changeZOrder, false );
		}

		/// <summary>
		///   Reposition/resize a set of windows, or change their visiblity. When changing visibilty, windows can't be repositioned/resized.
		///   This is a limitation of the underlying Win32 API.
		/// </summary>
		/// <param name="windows">The windows to perform the operation on.</param>
		/// <param name="changeZOrder">
		///   When true and changeVisibility is not set, the windows's Z orders are changed to reflect the order of the toPosition list.
		///   The first item in the list will appear at the top, while the last item will appear at the bottom.
		/// </param>
		/// <param name="changeVisibility">When set to true only the visiblity of windows can be changed. When set to false, only the other parameters can be changed.</param>
		static void RepositionWindows( IList<RepositionWindowInfo> windows, bool changeZOrder, bool changeVisibility )
		{
			if ( windows.Count == 0 )
			{
				// Nothing to reposition.
				return;
			}

			IntPtr windowsPositionInfo = User32.BeginDeferWindowPos( windows.Count );

			for ( int i = 0; i < windows.Count; ++i )
			{
				RepositionWindowInfo window = windows[ i ];

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
					i == 0 ? IntPtr.Zero : windows[ i - 1 ].ToPosition.Handle,
					window.X, window.Y,
					window.Width, window.Height,
					commands );
			}

			User32.EndDeferWindowPos( windowsPositionInfo );			
		}
	}
}
