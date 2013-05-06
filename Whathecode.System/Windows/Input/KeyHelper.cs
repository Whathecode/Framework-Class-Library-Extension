using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using Whathecode.System.Extensions;
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
		///   Returns all the keys which are currently down, excluding toggle keys.
		/// </summary>
		public static List<Key> GetNonToggleKeysState()
		{
			var keyStates = new byte[ 256 ];
			bool success = User32.GetKeyboardState( keyStates );

			var keysDown = new List<Key>();
			for ( int i = 0; i < keyStates.Length; ++i )
			{
				// Skip mouse keys, or non virtual keys (255).
				if ( IsMouseKey( i ) || i == 255 )
				{
					continue;
				}

				if ( (keyStates[ i ] & 0x80) != 0 )	// 0x80 refers to the highest order bit, which indicates whether the key is down or not.
				{
					keysDown.Add( KeyInterop.KeyFromVirtualKey( i ) );
				}
			}
			return keysDown;
		}

		static bool IsMouseKey( int key )
		{
			return key.EqualsAny( 1, 2, 4, 5, 6 );
		}

		/// <summary>
		///   Returns whether any key is currently down.
		///   TODO: This does not take into account e.g. Caps Lock!
		/// </summary>
		/// <returns>True when any key is down, false otherwise.</returns>
		public static bool IsAnyKeyDown()
		{
			var keyStates = new byte[ 256 ];
			bool success = User32.GetKeyboardState( keyStates );

			return keyStates.Any( b => b != 0 );
		}

		/// <summary>
		///   Simulate that a given key is pressed. (Down and up)
		/// </summary>
		public static void SimulateKeyPress( Key key )
		{
			var virtualKey = (ushort)KeyInterop.VirtualKeyFromKey( key );
			var keyPress = new []
			{
				new User32.Input
				{
					Type = User32.InputEventType.Keyboard,
					KeyboardInput = new User32.KeyboardInput
					{
						VirtualKey = virtualKey,
					}		
				},
				new User32.Input
				{
					Type = User32.InputEventType.Keyboard,
					KeyboardInput = new User32.KeyboardInput
					{
						VirtualKey = virtualKey,
						Flags = User32.KeyboardInputFlags.KeyUp
					}
				}
			};

			User32.SendInput( (uint)keyPress.Length, keyPress.ToArray(), User32.Input.Size );
		}
	}
}
