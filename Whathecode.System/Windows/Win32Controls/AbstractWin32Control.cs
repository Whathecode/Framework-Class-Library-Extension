using System;
using System.Runtime.InteropServices;
using Whathecode.Interop;


namespace Whathecode.System.Windows.Win32Controls
{
	/// <summary>
	///   An abstract wrapper for Win32 controls using Unicode.
	/// </summary>
	public abstract class AbstractWin32Control
	{
		internal readonly WindowInfo Window;


		protected AbstractWin32Control( WindowInfo windowHandle )
		{
			Window = windowHandle;

			// Ensure Unicode is always used, we don't care about old versions of Windows.
			bool isUsingUnicode = ( (int)SendMessage( User32.CommonControlMessage.GetUnicodeFormat ) ) != 0;
			if ( !isUsingUnicode )
			{
				// TODO: Try setting unicode format when ANSI controls are encountered?
				throw new NotSupportedException( "Only Unicode controls are supported." );
			}
		}


		/// <summary>
		///   Send a message to the control, with wParam and lParam set to <see cref="IntPtr.Zero" /> as default.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		protected IntPtr SendMessage( uint message, IntPtr? wParam = null, IntPtr? lParam = null )
		{
			IntPtr w = wParam ?? IntPtr.Zero;
			IntPtr l = lParam ?? IntPtr.Zero;
			return User32.SendMessage( Window.Handle, message, w, l );
		}

		/// <summary>
		///   Send a message to the control, with wParam and lParam set to <see cref="IntPtr.Zero" /> as default.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		protected IntPtr SendMessage( User32.CommonControlMessage message, IntPtr? wParam = null, IntPtr? lParam = null )
		{
			return SendMessage( (uint)message, wParam, lParam );
		}

		/// <summary>
		///   Read a structure of a given type from a memory address allocated by this function, but filled by the caller.
		/// </summary>
		/// <typeparam name="T">The type of the structure to read.</typeparam>
		/// <param name="fillMemory">Fills up the memory at the provided address, from which the structure will be read.</param>
		/// <returns>The read structure from the memory address.</returns>
		internal T ReadFromAddress<T>( Action<IntPtr> fillMemory )
			where T : struct
		{
			using ( var memory = new ControlMemory( Window, Marshal.SizeOf( typeof( T ) ) ) )
			{
				fillMemory( memory.Address );
				return memory.Read<T>();
			}
		}

		/// <summary>
		///   Writes a structure of a given type to a memory address allocated by this function, and subsequently use it.
		/// </summary>
		/// <typeparam name="T">The type of the structure to read.</typeparam>
		/// <param name="toWrite">The structure to write to memory.</param>
		/// <param name="useMemory">Method using the pointer in memory where the written object is located.</param>
		internal void WriteToAddress<T>( T toWrite, Action<IntPtr> useMemory )
			where T : struct
		{
			using ( var memory = new ControlMemory( Window, Marshal.SizeOf( typeof( T ) ) ) )
			{
				memory.Write( toWrite );
				useMemory( memory.Address );
			}
		}
	}
}
