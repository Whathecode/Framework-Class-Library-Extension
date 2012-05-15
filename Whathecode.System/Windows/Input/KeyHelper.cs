using System.Windows.Input;
using System.Linq;
using Whathecode.System.Windows.Interop;


namespace Whathecode.System.Windows.Input
{
	/// <summary>
	///   Provides static methods which allow for more direct access to the keyboard.
	/// </summary>
	public static class KeyHelper
	{
		/// <summary>
		///   Returns whether or not Caps Lock is enabled.
		/// </summary>
		public static bool IsCapsLockEnabled()
		{
			short state = User32.GetKeyState( KeyInterop.VirtualKeyFromKey( Key.CapsLock ) );
			return ((ushort)state & 0xFFFF) != 0;
		}

		/// <summary>
		///   Returns whether any key is currently down.
		///   TODO: This does not take into account e.g. Caps Lock!
		/// </summary>
		/// <returns>True when any key is down, false otherwise.</returns>
		public static bool IsAnyKeyDown()
		{
			byte[] keyStates = new byte[ 256 ];
			bool success = User32.GetKeyboardState( keyStates );

			return keyStates.Any( b => b != 0 );
		}
	}
}
