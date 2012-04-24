using System.Windows.Input;
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
	}
}
