using System;
using System.Diagnostics.Contracts;
using System.Security;
using Whathecode.System.Algorithm;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Split a string into two strings at a specified position.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "index">The index where to split the string.</param>
		/// <param name = "splitOption">Option specifying whether to include the character where is split in the resulting strings.</param>
		/// <returns>A string array containing both strings after the split.</returns>
		public static string[] SplitAt( this string source, int index, SplitOption splitOption = SplitOption.None )
		{
			Contract.Requires( index >= 0 && index < source.Length );

			return new[]
			{
				source.Substring( 0, index + (splitOption.EqualsAny( SplitOption.Left, SplitOption.Both ) ? 1 : 0) ),
				source.Substring( index + (splitOption.EqualsAny( SplitOption.Right, SplitOption.Both ) ? 0 : 1) )
			};
		}

		/// <summary>
		///   Ensure a string is unique by applying a certain suffix.
		///   TODO: Allow more choices than a counting int suffix.
		/// </summary>
		/// <param name = "source">The source of this extension method</param>
		/// <param name = "isUnique">Checks whether a certain path is unique or not.</param>
		/// <param name = "format">A standard or custom suffix format string. (see Remarks)</param>
		/// <returns>The original string with optionally a suffix applied to it to make it unique.</returns>
		/// <remarks>
		///   The <paramref name="format" /> parameter should contain a format pattern
		///   that defines the format of the suffix which will be applied to the string.
		///   - "i" can be used to represent a number which increases per found duplicate.
		/// </remarks>
		public static string MakeUnique( this string source, Func<string, bool> isUnique, string format )
		{
			int count = 1;			
			string current = source;
			while ( !isUnique( current ) )
			{
				string suffix = format.Replace( "i", count++.ToString() );
				current = source + suffix;
			}
			return current;
		}

		/// <summary>
		///   Converts an insecure string to a <see cref="SecureString" />.
		/// </summary>
		/// <param name = "insecure">The insecure string to convert to a <see cref="SecureString" />.</param>
		public static SecureString ToSecureString( this string insecure )
		{
			var secure = new SecureString();

			foreach ( char c in insecure )
			{
				secure.AppendChar( c );
			}
			secure.MakeReadOnly();

			return secure;
		}
	}
}