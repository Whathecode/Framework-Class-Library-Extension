using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Whathecode.Interop;
using Message = Whathecode.Interop.User32.ListViewMessage;


namespace Whathecode.System.Windows.Win32Controls
{
	/// <summary>
	///   A wrapper for a Win32 list-view control.
	/// </summary>
	public class ListView : AbstractWin32Control
	{
		public ListView( WindowInfo windowHandle )
			: base( windowHandle )
		{
		}


		/// <summary>
		///   Send a message to the list-control, with wParam and lParam set to <see cref="IntPtr.Zero" /> as default.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		IntPtr SendMessage( Message message, IntPtr? wParam = null, IntPtr? lParam = null )
		{
			return SendMessage( (uint)message, wParam, lParam );
		}

		/// <summary>
		///   Retrieves the number of items in a list-view control.
		/// </summary>
		public int GetItemCount()
		{
			return (int)SendMessage( Message.GetItemCount );
		}

		/// <summary>
		///   Retrieves the position of a list-view item.
		/// </summary>
		/// <param name="index">The index of the desired item.</param>
		/// <returns>The position of the item's upper-left corner.</returns>
		public Point GetItemPosition( int index )
		{
			var point = ReadFromAddress<User32.Point>( address =>
				{
					int success = (int)SendMessage( Message.GetItemPosition, new IntPtr( index ), address );
					if ( success == 0 )
					{
						throw new InvalidOperationException( "Could not retrieve position of list-view item with index " + index + "." );
					}
				} );
			return new Point( point.X, point.Y );
		}

		/// <summary>
		///   Moves an item to a specified position in a list-view control.
		/// </summary>
		/// <param name="index">Index of the list-view item for which to set the position.</param>
		/// <param name="position">The new position of the item, in list-view coordinates.</param>
		public void SetItemPosition( int index, Point position )
		{
			WriteToAddress(
				new User32.Point { X = (int)position.X, Y = (int)position.Y },
				address => SendMessage( Message.SetItemPosition32, new IntPtr( index ), address ) );
		}

		/// <summary>
		///   Retrieves the text of a list-view item.
		/// </summary>
		/// <param name="index">Index of the desired item of which to retrieve the text.</param>
		public string GetItemText( int index )
		{
			string text = "";
			using ( var memory = new ControlMemory( Window, Marshal.SizeOf( typeof( User32.ListViewItem ) ) ) )
			{
				// TODO: Any way of knowing the maximum length of item text here?
				// Trial and error might be needed, trying a bigger buffer when the whole string is not retrieved:
				// http://forums.codeguru.com/showthread.php?351972-Getting-listView-item-text-length;
				var stringBuffer = new ControlMemory( Window, Constants.MaximumPathLength * 2 );
				var itemData = new User32.ListViewItem
				{
					TextMax = Constants.MaximumPathLength,
					Text = stringBuffer.Address
				};
				memory.Write( itemData );
				int length = (int)SendMessage( Message.GetItemText, new IntPtr( index ), memory.Address );
				itemData = memory.Read<User32.ListViewItem>();
				if ( length > 0 )
				{
					byte[] textBuffer = stringBuffer.Read<byte>( length * 2 );
					text = Encoding.Unicode.GetString( textBuffer );
				}
				stringBuffer.Dispose();
			}

			return text;
		}
	}
}
