using System.Windows.Input;
using Whathecode.System.Windows.Interop;
using W = System.Windows.Input;


namespace Whathecode.System.Windows.Input
{
	/// <summary>
	///   Provides static methods which allow for more direct access to the keyboard.
	/// </summary>
	public static class KeyInterop
	{
		/// <summary>
		///   Returns whether or not a given key is down.
		/// </summary>
		public static bool IsKeyDown( Key key )
		{
			short state = User32.GetKeyState( W.KeyInterop.VirtualKeyFromKey( key ) );
			return ((ushort)state & 0xFFFF) != 0;
		}
	}
}
