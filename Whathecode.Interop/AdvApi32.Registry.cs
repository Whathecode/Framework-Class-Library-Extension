using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains AdvApi32.dll definitions relating to registry operations.
	/// </summary>
	public static partial class AdvApi32
	{
		#region Types

		/// <summary>
		///   Determines the behavior of <see cref="RegLoadMUIString" />.
		/// </summary>
		[Flags]
		public enum RegistryLoadMuiStringOptions : uint
		{
			None = 0,
			/// <summary>
			///   The string is truncated to fit the available size of the output buffer. If this flag is specified, copiedDataSize must be NULL.
			/// </summary>
			Truncate = 1
		}

		#endregion // Types


		#region Functions

		/// <summary>
		///   Loads the specified string from the specified key and subkey.
		/// </summary>
		/// <param name="registryKeyHandle">
		///   A handle to an open registry key. The key must have been opened with the KEY_QUERY_VALUE access right. For more information, see Registry Key Security and Access Rights.
		///   This handle is returned by the RegCreateKeyEx or RegOpenKeyEx function. It can also be one of the following predefined keys:
		///   HKEY_CLASSES_ROOT, HKEY_CURRENT_CONFIG, HKEY_CURRENT_USER, HKEY_LOCAL_MACHINE, HKEY_USERS
		/// </param>
		/// <param name="value">The name of the registry value.</param>
		/// <param name="outputBuffer">
		///   The buffer that receives the string. Strings of the following form receive special handling: @[path]\dllname,-strID
		///   The string with identifier strID is loaded from dllname; the path is optional. If the pszDirectory parameter is not NULL, the directory is prepended to the path specified in the registry data.
		///   Note that dllname can contain environment variables to be expanded.
		/// </param>
		/// <param name="outputBufferSize">The size of the outputBuffer buffer, in bytes.</param>
		/// <param name="requiredSize">
		///   A variable that receives the size of the data copied to the outputBuffer buffer, in bytes.
		///   If the buffer is not large enough to hold the data, the function returns <see cref="ErrorCode.MoreData" /> and stores the required buffer size in the variable pointed to by copiedDataSize.
		///   In this case, the contents of the buffer are undefined.
		/// </param>
		/// <param name="options">Determines how to load the string.</param>
		/// <param name="path">The directory path.</param>
		/// <returns>
		///   If the function succeeds, the return value is <see cref="ErrorCode.Success" />.
		///   If the function fails, the return value is a system error code.
		///   If the copiedDatasize buffer is too small to receive the string, the function returns <see cref="ErrorCode.MoreData" />.
		/// </returns>
		/// <remarks>
		///   To compile an application that uses this function, define _WIN32_WINNT as 0x0600 or later. For more information, see Using the Windows Headers.
		/// </remarks>
		[DllImport( Dll, CharSet = CharSet.Unicode )]
		public extern static int RegLoadMUIString(
			IntPtr registryKeyHandle, string value,
			StringBuilder outputBuffer, int outputBufferSize, out int requiredSize,
			RegistryLoadMuiStringOptions options, string path );

		#endregion // Functions
	}
}
