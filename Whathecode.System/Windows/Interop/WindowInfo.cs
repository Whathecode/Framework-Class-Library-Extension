using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Whathecode.System.Extensions;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Information about an application window.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[Serializable]
	public class WindowInfo : ISerializable
	{
		const int MaxClassnameLength = 128;	// TODO: Is there an actual maximum class name length?
		readonly Dictionary<User32.WindowState, WindowState> _windowStateMapping = new Dictionary<User32.WindowState, WindowState>
		{
			{ User32.WindowState.ShowNormal, WindowState.Open },
			{ User32.WindowState.Maximized, WindowState.Maximized },
			{ User32.WindowState.ShowMinimized, WindowState.Minimized },
			{ User32.WindowState.Hide, WindowState.Hidden }
		};

		internal readonly IntPtr Handle;


		/// <summary>
		///   Create a new WindowInfo object for the specified window handle.
		/// </summary>
		/// <param name="handle">The handle of the window to create a WindowInfo object for.</param>
		public WindowInfo( IntPtr handle )
		{
			Handle = handle;
		}

		public WindowInfo( SerializationInfo info, StreamingContext context )
		{
			long pointer = info.GetInt64( "Handle" );

			// The pointer is serialized as a 64 bit integer, but the system might be 32 bit.
			Handle = IntPtr.Size == 8 ? new IntPtr( pointer ) : new IntPtr( (int)pointer );
		}

		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "Handle", Handle.ToInt64() );
		}


		/// <summary>
		///   Enumerates the child windows that belong to this window.
		/// </summary>
		/// <returns></returns>
		public List<WindowInfo> GetChildWindows()
		{
			var windows = new List<WindowInfo>();
			User32.EnumChildWindows( Handle,
				( handle, lparam ) =>
				{
					windows.Add( new WindowInfo( handle ) );
					return true;
				},
				IntPtr.Zero );

			return windows;
		}

		/// <summary>
		///   Get the owner window of the window if any.
		/// </summary>
		/// <returns>The <see cref="WindowInfo" /> instance of the owner window or null if no owner window is present.</returns>
		public WindowInfo GetOwnerWindow()
		{
			IntPtr ownerHandle = User32.GetWindow( Handle, User32.WindowRelationship.Owner );
			return ownerHandle == IntPtr.Zero ? null : new WindowInfo( ownerHandle );
		}

		/// <summary>
		///   Retrieves the name of the class to which the specified window belongs.
		/// </summary>
		public string GetClassName()
		{
			var buffer = new StringBuilder( MaxClassnameLength );
			if ( User32.GetClassName( Handle, buffer, buffer.Capacity ) == 0 )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return buffer.ToString();
		}

		/// <summary>
		///   Determines whether the window is a dialog box.
		/// </summary>
		public bool IsDialogBox()
		{
			return GetClassName() == "#32770";
		}

		Process _process;
		ProcessThread _processThread;
		/// <summary>
		///   Retrieves the process that created the window, when available.
		/// </summary>
		/// <returns>The process when available, null otherwise.</returns>
		public Process GetProcess()
		{
			if ( _process == null )
			{
				int processId = 0;
				int threadId = User32.GetWindowThreadProcessId( Handle, ref processId );
				if ( threadId == 0 )
				{
					MarshalHelper.ThrowLastWin32ErrorException();
				}

				_process = processId == 0
					? null
					: Process.GetProcessById( processId );

				if ( _process != null )
				{
					_processThread = (
						from ProcessThread t in _process.Threads
						where t.Id == threadId
						select t ).First();
				}
			}

			return _process;
		}

		/// <summary>
		///   Retrieves the thread that created the specified window.
		/// </summary>
		public ProcessThread GetProcessThread()
		{
			if ( _processThread == null )
			{
				GetProcess();
			}

			return _processThread;
		}

		/// <summary>
		///   Retrieves the text in the window's title bar.
		/// </summary>
		public string GetTitle()
		{
			int length = User32.GetWindowTextLength( Handle );
			var buffer = new StringBuilder( length + 1 );
			if ( User32.GetWindowText( Handle, buffer, buffer.Capacity ) == 0 )
			{
				// The window might as well have no title/empty title,
				// but no exception will be thrown in that scenario since no error code is set.
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return buffer.ToString();
		}

		/// <summary>
		///   Retrieves the current state of the window.
		/// </summary>
		/// <returns></returns>
		public WindowState GetWindowState()
		{
			User32.WindowPlacement placement = GetWindowPlacement();
			var state = (User32.WindowState)placement.ShowCommand;

			return EnumHelper<User32.WindowState>.Convert( state, _windowStateMapping );
		}

		/// <summary>
		///   Retrieves placement information of a window in a User32.dll structure.
		/// </summary>
		User32.WindowPlacement GetWindowPlacement()
		{
			User32.WindowPlacement placement = User32.WindowPlacement.Default;
			if ( !User32.GetWindowPlacement( Handle, ref placement ) )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			return placement;
		}

		/// <summary>
		///   Hides the window and activates another window.
		/// </summary>
		public void Hide()
		{
			User32.ShowWindowAsync( Handle, User32.WindowState.Hide );
		}

		/// <summary>
		///   Verifies whether this window has been destroyed or not.
		/// </summary>
		/// <returns>True when the window no longer exists; false otherwise.</returns>
		public bool IsDestroyed()
		{
			return !User32.IsWindow( Handle );
		}

		/// <summary>
		///   Determines the visibility state of the window.
		/// </summary>
		public bool IsVisible()
		{
			return User32.IsWindowVisible( Handle );
		}

		/// <summary>
		///   Determines whether or not the window is set to be topmost, in which case it always stays on top of any other non-topmost windows.
		/// </summary>
		/// <returns></returns>
		public bool IsTopmost()
		{
			int extraOptions = (int)User32.GetWindowLongPtr( Handle, (int)User32.GetWindowLongOptions.ExtendedStyles );
			const int topmost = (int)User32.ExtendedWindowStyles.Topmost;

			return ( (extraOptions & topmost) == topmost );
		}

		/// <summary>
		///   Activates the window and displays it as a maximized window.
		/// </summary>
		public void Maximize()
		{
			User32.ShowWindowAsync( Handle, User32.WindowState.ShowMaximized );
		}

		/// <summary>
		///   Minimizes the window and activates the next top-level window in the z-order.
		/// </summary>
		public void Minimize()
		{
			User32.ShowWindowAsync( Handle, User32.WindowState.Minimize );
		}

		/// <summary>
		///   Activates the window and displays it in its current size and position.
		///   TODO: Is it possible that this doesn't activate the window when it isn't visible?
		/// </summary>
		/// <param name="activate">
		///	  When set to true, the window will be brought to the front and activated.
		///   Otherwise it will stay in it's previous state, e.g. minimized.
		/// </param>
		public void Show( bool activate = true )
		{
			User32.ShowWindowAsync(
				Handle,
				activate ? User32.WindowState.Show : User32.WindowState.ShowNoActivate );
		}

		/// <summary>
		///   Brings the thread that created the specified window into the foreground and activates the window.
		///   Keyboard input is directed to the window, and various visual cues are changed for the user.
		///   The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
		/// </summary>
		public void SetForegroundWindow()
		{
			User32.SetForegroundWindow( Handle );
		}

		public override bool Equals( object other )
		{
			var otherWindow = other as WindowInfo;
			if ( otherWindow == null )
			{
				return false;
			}

			return Equals( otherWindow );
		}

		public bool Equals( WindowInfo other )
		{
			if ( ReferenceEquals( null, other ) )
			{
				return false;
			}

			return ReferenceEquals( this, other ) || other.Handle.Equals( Handle );
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		public override string ToString()
		{
			string processName = GetProcess().IfNotNull( p => p.ProcessName ) ?? "not found";
			return string.Format(
				"Window \"{0}\" ({1}) state: {2}, classname: {3}",
				GetTitle(), processName, GetWindowState(), GetClassName() );
		}
	}
}
