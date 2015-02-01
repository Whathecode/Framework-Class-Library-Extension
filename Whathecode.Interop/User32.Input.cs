using System;
using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains User32.dll definitions relating to input operations.
	/// </summary>
	public static partial class User32
	{
		#region Types

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

		#endregion // Types


		#region Functions

		/// <summary>
		///   Retrieves the status of the specified virtual key.
		///   The status specifies whether the key is up, down, or toggled (on, off—alternating each time the key is pressed).
		/// </summary>
		/// <remarks>
		///   The key status returned from this function changes as a thread reads key messages from its message queue.
		///   The status does not reflect the interrupt-level state associated with the hardware.
		///   Use the GetAsyncKeyState function to retrieve that information.
		/// </remarks>
		/// <returns>
		///   The return value specifies the status of the specified virtual key, as follows:
		///   - If the high-order bit is 1, the key is down; otherwise, it is up.
		///   - If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on.
		///     The key is off and untoggled if the low-order bit is 0.
		///     A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
		/// </returns>
		[DllImport( Dll )]
		public static extern short GetKeyState( int virtualkey );

		/// <summary>
		///   Copies the status of the 256 virtual keys to the specified buffer.
		/// </summary>
		/// <param name = "keyStates">The 256-byte array that receives the status data for each virtual key.</param>
		/// <returns>
		///   If the function fails, the return value is false; otherwise true. To get extended error information, call GetLastWin32Error.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern bool GetKeyboardState( byte[] keyStates );

		/// <summary>
		///   Synthesizes keystrokes, mouse motions, and button clicks.
		/// </summary>
		/// <param name = "inputCount">The number of structures in the inputs array.</param>
		/// <param name = "inputs">An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
		/// <param name = "structureSize">The size, in bytes, of an INPUT structure. If structureSize is not the size of an INPUT structure, the function fails.</param>
		/// <remarks>
		///   This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or lesser integrity level.
		///   The SendInput function inserts the events in the INPUT structures serially into the keyboard or mouse input stream.
		///   These events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse) or by calls to keybd_event, mouse_event, or other calls to SendInput.
		///   This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might interfere with the events that this function generates.
		///   To avoid this problem, check the keyboard's state with the GetAsyncKeyState function and correct as necessary.
		///   Because the touch keyboard uses the surrogate macros defined in winnls.h to send input to the system, a listener on the keyboard event hook must decode input originating from the touch keyboard.
		///   For more information, see Surrogates and Supplementary Characters.
		///   An accessibility application can use SendInput to inject keystrokes corresponding to application launch shortcut keys that are handled by the shell.
		///   This functionality is not guaranteed to work for other types of applications.
		/// </remarks>
		/// <returns>
		///   The function returns the number of events that it successfully inserted into the keyboard or mouse input stream.
		///   If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastWin32Error.
		///   This function fails when it is blocked by UIPI. Note that neither GetLastWin32Error nor the return value will indicate the failure was caused by UIPI blocking.
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		public static extern uint SendInput( uint inputCount, Input[] inputs, int structureSize );

		#endregion // Functions
	}
}
