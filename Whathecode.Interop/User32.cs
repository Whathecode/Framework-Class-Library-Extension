using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains common User32.dll definitions and operations.
	///   TODO: Clean up remaining original documentation, converting it to the wrapper's equivalents.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static partial class User32
	{
		const string Dll = "user32.dll";


		#region Common types.

		/// <summary>
		///   The Point structure defines the x- and y- coordinates of a point.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct Point
		{
			/// <summary>
			///   The x-coordinate of the point.
			/// </summary>
			public int X;
			/// <summary>
			///   The y-coordinate of the point.
			/// </summary>
			public int Y;
		}


		/// <summary>
		///   The Rectangle structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
		/// </summary>
		/// <remarks>
		///   By convention, the right and bottom edges of the rectangle are normally considered exclusive.
		///   In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the rectangle.
		///   For example, when Rectangle is passed to the FillRect function, the rectangle is filled up to,
		///   but not including, the right column and bottom row of pixels.
		/// </remarks>
		[StructLayout( LayoutKind.Sequential )]
		public struct Rectangle
		{
			/// <summary>
			///   The x-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Left;
			/// <summary>
			///   The y-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Top;
			/// <summary>
			///   The x-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Right;
			/// <summary>
			///   The y-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Bottom;
		}

		#endregion // Common types.
	}
}
