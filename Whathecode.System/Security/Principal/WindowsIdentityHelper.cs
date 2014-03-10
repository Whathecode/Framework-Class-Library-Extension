using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Whathecode.Interop;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System.Security.Principal
{
	/// <summary>
	///   A helper class to do common <see cref="WindowsIdentity" /> operations.
	/// </summary>
	public static class WindowsIdentityHelper
	{
		/// <summary>
		///   Checks whether the primary access token of the process belongs to a user account that is a member of the local Administrator group, even if it currently is not elevated.
		/// </summary>
		/// <returns>
		///   True if the primary access token of the process belongs to a user account that is a member of the Administrators group; false otherwise.
		/// </returns>
		public static bool IsUserInAdminGroup()
		{
			// Default token's received aren't impersonation tokens, we are looking for an impersonation token.
			bool isImpersonationToken = false;

			// Open the access token of the current process.
			SafeTokenHandle processToken;
			if ( !AdvApi32.OpenProcessToken( Process.GetCurrentProcess().Handle, (uint)(TokenAccessLevels.Query | TokenAccessLevels.Duplicate), out processToken ) )
			{
				MarshalHelper.ThrowLastWin32ErrorException();
			}

			// Starting from Vista linked tokens are supported which need to be checked.
			if ( EnvironmentHelper.VistaOrHigher )
			{
				// Determine token type: limited, elevated, or default.
				int marshalSize = sizeof( AdvApi32.TokenElevationType );
				var elevationTypeHandle = SafeUnmanagedMemoryHandle.FromNewlyAllocatedMemory( marshalSize );
				if ( !AdvApi32.GetTokenInformation( processToken, AdvApi32.TokenInformationClass.TokenElevationType, elevationTypeHandle.DangerousGetHandle(), marshalSize, out marshalSize ) )
				{
					MarshalHelper.ThrowLastWin32ErrorException();
				}
				var tokenType = (AdvApi32.TokenElevationType)Marshal.ReadInt32( elevationTypeHandle.DangerousGetHandle() );

				// If limited, get the linked elevated token for further check.
				if ( tokenType == AdvApi32.TokenElevationType.TokenElevationTypeLimited )
				{
					// Get the linked token.
					marshalSize = IntPtr.Size;
					var linkedTokenHandle = SafeUnmanagedMemoryHandle.FromNewlyAllocatedMemory( marshalSize );
					if ( !AdvApi32.GetTokenInformation( processToken, AdvApi32.TokenInformationClass.TokenLinkedToken, linkedTokenHandle.DangerousGetHandle(), marshalSize, out marshalSize ) )
					{
						MarshalHelper.ThrowLastWin32ErrorException();
					}
					processToken = new SafeTokenHandle( Marshal.ReadIntPtr( linkedTokenHandle.DangerousGetHandle() ) );
					isImpersonationToken = true; // Linked tokens are already impersonation tokens.
				}
			}

			// We need an impersonation token in order to check whether it contains admin SID.
			if ( !isImpersonationToken )
			{
				SafeTokenHandle impersonatedToken;
				if ( !AdvApi32.DuplicateToken( processToken, AdvApi32.SecurityImpersonationLevel.SecurityIdentification, out impersonatedToken ) )
				{
					MarshalHelper.ThrowLastWin32ErrorException();
				}
				processToken = impersonatedToken;
			}

			// Check if the token to be checked contains admin SID.
			var identity= new WindowsIdentity( processToken.DangerousGetHandle() );
			var principal = new WindowsPrincipal( identity );
			return principal.IsInRole( WindowsBuiltInRole.Administrator );
		}
	}
}
