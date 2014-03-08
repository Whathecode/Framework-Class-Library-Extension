using System;
using System.Text;
using Microsoft.Win32;
using Whathecode.System.Windows.Interop;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Retrieves the multilingual string associated with the specified name. Returns null if the name/value pair does not exist in the registry.
		///   The key must have been opened using 
		/// </summary>
		/// <param name = "key">The registry key to load the string from.</param>
		/// <param name = "name">The name of the string to load.</param>
		/// <returns>The language-specific string, or null if the name/value pair does not exist in the registry.</returns>
		public static string LoadMuiStringValue( this RegistryKey key, string name )
		{
			const int initialBufferSize = 1024;
			var output = new StringBuilder( initialBufferSize );
			int requiredSize;
			IntPtr keyHandle = key.Handle.DangerousGetHandle();
			ErrorCode result = (ErrorCode)AdvApi32.RegLoadMUIString( keyHandle, name, output, output.Capacity, out requiredSize, AdvApi32.RegistryLoadMuiStringOptions.None, null );

			if ( result == ErrorCode.MoreData )
			{
				output.EnsureCapacity( requiredSize );
				result = (ErrorCode)AdvApi32.RegLoadMUIString( keyHandle, name, output, output.Capacity, out requiredSize, AdvApi32.RegistryLoadMuiStringOptions.None, null );
			}

			return result == ErrorCode.Success ? output.ToString() : null;
		}
	}
}
