using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;


namespace Whathecode.Interop
{
	/// <summary>
	///   Class through which advapi32.dll calls can be accessed.
	///   TODO: Clean up remaining original documentation, converting it to the wrapper's equivalents.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static partial class AdvApi32
	{
		const string Dll = "advapi32.dll";


		#region Access tokens.

		/// <summary>
		///   The OpenProcessToken function opens the access token associated with a process.
		/// </summary>
		/// <param name = "processHandle">A handle to the process whose access token is opened. The process must have the PROCESS_QUERY_INFORMATION access permission.</param>
		/// <param name = "desiredAccess">
		///   Specifies an access mask that specifies the requested types of access to the access token.
		///   These requested access types are compared with the discretionary access control list (DACL) of the token to determine which accesses are granted or denied.
		///   For a list of access rights for access tokens, see <see cref = "TokenAccessLevels" />.</param>
		/// <param name = "tokenHandle">A pointer to a handle that identifies the newly opened access token when the function returns.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error().</returns>
		[DllImport( Dll, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool OpenProcessToken( IntPtr processHandle, UInt32 desiredAccess, out SafeTokenHandle tokenHandle );

		/// <summary>
		///   The GetTokenInformation function retrieves a specified type of information about an access token.
		///   The calling process must have appropriate access rights to obtain the information.
		///   To determine if a user is a member of a specific group, use the CheckTokenMembership function.
		///   To determine group membership for app container tokens, use the CheckTokenMembershipEx function.
		/// </summary>
		/// <param name = "tokenHandle">
		///   A handle to an access token from which information is retrieved. If TokenInformationClass specifies TokenSource, the handle must have <see cref = "TokenAccessLevels.QuerySource"/> access.
		///   For all other TokenInformationClass values, the handle must have <see cref = "TokenAccessLevels.Query"/> access.</param>
		/// <param name = "tokenInformationClass">
		///   Specifies a value from the TOKEN_INFORMATION_CLASS enumerated type to identify the type of information the function retrieves.
		///   Any callers who check the TokenIsAppContainer and have it return 0 should also verify that the caller token is not an identify level impersonation token.
		///   If the current token is not an app container but is an identity level token, you should return AccessDenied.
		/// </param>
		/// <param name = "tokenInformation">
		///   A pointer to a buffer the function fills with the requested information.
		///   The structure put into this buffer depends upon the type of information specified by the TokenInformationClass parameter.
		/// </param>
		/// <param name = "tokenInformationLength">
		///   Specifies the size, in bytes, of the buffer pointed to by the TokenInformation parameter. If TokenInformation is NULL, this parameter must be zero.
		/// </param>
		/// <param name = "returnLength">
		///   A pointer to a variable that receives the number of bytes needed for the buffer pointed to by the tokenInformationLength parameter.
		///   If this value is larger than the value specified in the TokenInformationLength parameter, the function fails and stores no data in the buffer.
		///   If the value of the TokenInformationClass parameter is TokenDefaultDacl and the token has no default DACL,
		///   the function sets the variable pointed to by ReturnLength to sizeof(TOKEN_DEFAULT_DACL) and sets the DefaultDacl member of the TOKEN_DEFAULT_DACL structure to NULL.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is nonzero.
		///   If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error().
		/// </returns>
		[DllImport( Dll, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool GetTokenInformation( SafeTokenHandle tokenHandle, TokenInformationClass tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength );

		/// <summary>
		///   The DuplicateToken function creates a new access token that duplicates one already in existence.
		/// </summary>
		/// <param name = "existingTokenHandle">A handle to an access token opened with <see cref = "TokenAccessLevels.Duplicate" /> access.</param>
		/// <param name = "impersonationLevel">Specifies a <see cref = "SecurityImpersonationLevel" /> enumerated type that supplies the impersonation level of the new token.</param>
		/// <param name = "duplicateTokenHandle">
		///   A pointer to a variable that receives a handle to the duplicate token.
		///   This handle has <see cref = "TokenAccessLevels.Impersonate" /> and <see cref = "TokenAccessLevels.Query" /> access to the new token.
		/// </param>
		/// <returns>
		///   If the function succeeds, the return value is nonzero.
		///   If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error().
		/// </returns>
		/// <remarks>
		///   The DuplicateToken function creates an impersonation token, which you can use in functions such as SetThreadToken and ImpersonateLoggedOnUser.
		///   The token created by DuplicateToken cannot be used in the CreateProcessAsUser function, which requires a primary token.
		///   To create a token that you can pass to CreateProcessAsUser, use the DuplicateTokenEx function.
		/// </remarks>
		[DllImport( Dll, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public extern static bool DuplicateToken( SafeTokenHandle existingTokenHandle, SecurityImpersonationLevel impersonationLevel, out SafeTokenHandle duplicateTokenHandle );

		#endregion // Access tokens.


		#region Registry

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

		#endregion // Registry.
	}
}
