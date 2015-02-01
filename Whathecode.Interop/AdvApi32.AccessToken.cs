using System;
using System.Runtime.InteropServices;
using System.Security.Principal;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains AdvApi32.dll definitions relating to access token operations.
	/// </summary>
	public static partial class AdvApi32
	{
		#region Types

		/// <summary>
		///   The <see cref="TokenElevationType"/> enumeration indicates the elevation type of token being queried by the GetTokenInformation function.
		/// </summary>
		public enum TokenElevationType
		{
			/// <summary>
			///   The token does not have a linked token.
			/// </summary>
			TokenElevationTypeDefault = 1,
			/// <summary>
			///   The token is an elevated token.
			/// </summary>
			TokenElevationTypeFull,
			/// <summary>
			///   The token is a limited token.
			/// </summary>
			TokenElevationTypeLimited
		}


		/// <summary>
		///   The <see cref="TokenInformationClass"/> enumeration contains values that specify the type of information being assigned to or retrieved from an access token.
		///   The GetTokenInformation function uses these values to indicate the type of token information to retrieve.
		///   The SetTokenInformation function uses these values to set the token information.
		/// </summary>
		public enum TokenInformationClass
		{
			/// <summary>
			///   The buffer receives a TOKEN_USER structure that contains the user account of the token.
			/// </summary>
			TokenUser = 1,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS structure that contains the group accounts associated with the token.
			/// </summary>
			TokenGroups,
			/// <summary>
			///   The buffer receives a TOKEN_PRIVILEGES structure that contains the privileges of the token.
			/// </summary>
			TokenPrivileges,
			/// <summary>
			///   The buffer receives a TOKEN_OWNER structure that contains the default owner security identifier (SID) for newly created objects.
			/// </summary>
			TokenOwner,
			/// <summary>
			///   The buffer receives a TOKEN_PRIMARY_GROUP structure that contains the default primary group SID for newly created objects.
			/// </summary>
			TokenPrimaryGroup,
			/// <summary>
			///   The buffer receives a TOKEN_DEFAULT_DACL structure that contains the default DACL for newly created objects.
			/// </summary>
			TokenDefaultDacl,
			/// <summary>
			///   The buffer receives a TOKEN_SOURCE structure that contains the source of the token. TOKEN_QUERY_SOURCE access is needed to retrieve this information.
			/// </summary>
			TokenSource,
			/// <summary>
			///   The buffer receives a TOKEN_TYPE value that indicates whether the token is a primary or impersonation token.
			/// </summary>
			TokenType,
			/// <summary>
			///   The buffer receives a SECURITY_IMPERSONATION_LEVEL value that indicates the impersonation level of the token. If the access token is not an impersonation token, the function fails.
			/// </summary>
			TokenImpersonationLevel,
			/// <summary>
			///   The buffer receives a TOKEN_STATISTICS structure that contains various token statistics.
			/// </summary>
			TokenStatistics,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS structure that contains the list of restricting SIDs in a restricted token.
			/// </summary>
			TokenRestrictedSids,
			/// <summary>
			///   The buffer receives a DWORD value that indicates the Terminal Services session identifier that is associated with the token.
			///   If the token is associated with the terminal server client session, the session identifier is nonzero.
			///   Windows Server 2003 and Windows XP: If the token is associated with the terminal server console session, the session identifier is zero.
			///   In a non-Terminal Services environment, the session identifier is zero.
			///   If TokenSessionId is set with SetTokenInformation,
			///   the application must have the Act As Part Of the Operating System privilege, and the application must be enabled to set the session ID in a token.
			/// </summary>
			TokenSessionId,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS_AND_PRIVILEGES structure that contains the user SID, the group accounts, the restricted SIDs, and the authentication ID associated with the token.
			/// </summary>
			TokenGroupsAndPrivileges,
			/// <summary>
			///   Reserved.
			/// </summary>
			TokenSessionReference,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if the token includes the SANDBOX_INERT flag.
			/// </summary>
			TokenSandBoxInert,
			/// <summary>
			///   Reserved.
			/// </summary>
			TokenAuditPolicy,
			/// <summary>
			///   The buffer receives a TOKEN_ORIGIN value.
			///   If the token resulted from a logon that used explicit credentials,
			///   such as passing a name, domain, and password to the LogonUser function, then the TOKEN_ORIGIN structure will contain the ID of the logon session that created it.
			///   If the token resulted from network authentication,
			///   such as a call to AcceptSecurityContext or a call to LogonUser with dwLogonType set to LOGON32_LOGON_NETWORK or LOGON32_LOGON_NETWORK_CLEARTEXT, then this value will be zero.
			/// </summary>
			TokenOrigin,
			/// <summary>
			///   The buffer receives a TOKEN_ELEVATION_TYPE value that specifies the elevation level of the token.
			/// </summary>
			TokenElevationType,
			/// <summary>
			///   The buffer receives a TOKEN_LINKED_TOKEN structure that contains a handle to another token that is linked to this token.
			/// </summary>
			TokenLinkedToken,
			/// <summary>
			///   The buffer receives a TOKEN_ELEVATION structure that specifies whether the token is elevated.
			/// </summary>
			TokenElevation,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if the token has ever been filtered.
			/// </summary>
			TokenHasRestrictions,
			/// <summary>
			///   The buffer receives a TOKEN_ACCESS_INFORMATION structure that specifies security information contained in the token.
			/// </summary>
			TokenAccessInformation,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if virtualization is allowed for the token.
			/// </summary>
			TokenVirtualizationAllowed,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if virtualization is enabled for the token.
			/// </summary>
			TokenVirtualizationEnabled,
			/// <summary>
			///   The buffer receives a TOKEN_MANDATORY_LABEL structure that specifies the token's integrity level. 
			/// </summary>
			TokenIntegrityLevel,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if the token has the UIAccess flag set.
			/// </summary>
			TokenUIAccess,
			/// <summary>
			///   The buffer receives a TOKEN_MANDATORY_POLICY structure that specifies the token's mandatory integrity policy.
			/// </summary>
			TokenMandatoryPolicy,
			/// <summary>
			///   The buffer receives the token's logon security identifier (SID).
			/// </summary>
			TokenLogonSid,
			/// <summary>
			///   The buffer receives a DWORD value that is nonzero if the token is an app container token.
			///   Any callers who check the TokenIsAppContainer and have it return 0 should also verify that the caller token is not an identify level impersonation token.
			///   If the current token is not an app container but is an identity level token, you should return AccessDenied.
			/// </summary>
			TokenIsAppContainer,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS structure that contains the capabilities associated with the token.
			/// </summary>
			TokenCapabilities,
			/// <summary>
			///   The buffer receives a TOKEN_APPCONTAINER_INFORMATION structure that contains the AppContainerSid associated with the token.
			///   If the token is not associated with an app container, the TokenAppContainer member of the TOKEN_APPCONTAINER_INFORMATION structure points to NULL.
			/// </summary>
			TokenAppContainerSid,
			/// <summary>
			///   The buffer receives a DWORD value that includes the app container number for the token. For tokens that are not app container tokens, this value is zero.
			/// </summary>
			TokenAppContainerNumber,
			/// <summary>
			///   The buffer receives a CLAIM_SECURITY_ATTRIBUTES_INFORMATION structure that contains the user claims associated with the token.
			/// </summary>
			TokenUserClaimAttributes,
			/// <summary>
			///   The buffer receives a CLAIM_SECURITY_ATTRIBUTES_INFORMATION structure that contains the device claims associated with the token.
			/// </summary>
			TokenDeviceClaimAttributes,
			/// <summary>
			///   This value is reserved.
			/// </summary>
			TokenRestrictedUserClaimAttributes,
			/// <summary>
			///   This value is reserved.
			/// </summary>
			TokenRestrictedDeviceClaimAttributes,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS structure that contains the device groups that are associated with the token.
			/// </summary>
			TokenDeviceGroups,
			/// <summary>
			///   The buffer receives a TOKEN_GROUPS structure that contains the restricted device groups that are associated with the token.
			/// </summary>
			TokenRestrictedDeviceGroups,
			/// <summary>
			///   This value is reserved.
			/// </summary>
			TokenSecurityAttributes,
			/// <summary>
			///   This value is reserved.
			/// </summary>
			TokenIsRestricted,
			/// <summary>
			///   The maximum value for this enumeration
			/// </summary>
			MaxTokenInfoClass
		}

		/// <summary>
		///   The <see cref = "SecurityImpersonationLevel" /> enumeration contains values that specify security impersonation levels.
		///   Security impersonation levels govern the degree to which a server process can act on behalf of a client process.
		/// </summary>
		public enum SecurityImpersonationLevel
		{
			/// <summary>
			///   The server process cannot obtain identification information about the client, and it cannot impersonate the client.
			///   It is defined with no value given, and thus, by ANSI C rules, defaults to a value of zero.
			/// </summary>
			SecurityAnonymous,
			/// <summary>
			///   The server process can obtain information about the client, such as security identifiers and privileges, but it cannot impersonate the client.
			///   This is useful for servers that export their own objects, for example, database products that export tables and views.
			///   Using the retrieved client-security information, the server can make access-validation decisions without being able to use other services that are using the client's security context.
			/// </summary>
			SecurityIdentification,
			/// <summary>
			///   The server process can impersonate the client's security context on its local system. The server cannot impersonate the client on remote systems.
			/// </summary>
			SecurityImpersonation,
			/// <summary>
			///   The server process can impersonate the client's security context on remote systems.
			/// </summary>
			SecurityDelegation
		}

		#endregion // Types


		#region Functions

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

		#endregion // Functions
	}
}
