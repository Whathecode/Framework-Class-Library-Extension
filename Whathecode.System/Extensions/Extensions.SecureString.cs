using System;
using System.Runtime.InteropServices;
using System.Security;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Converts a <see cref="SecureString" /> to a normal insecure string.
		/// </summary>
		/// <param name = "input">The <see cref="SecureString" /> to convert to an insecure string.</param>
		public static string ToInsecureString( this SecureString input )
		{
			string secured;

			IntPtr ptr = Marshal.SecureStringToBSTR( input );
			try
			{
				secured = Marshal.PtrToStringBSTR( ptr );
			}
			finally
			{
				Marshal.ZeroFreeBSTR( ptr );
			}

			return secured;
		}
	}
}
