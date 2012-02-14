using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Whathecode.System.IO
{
	/// <summary>
	///   A helper class for <see cref="Path" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class PathHelper
	{
		/// <summary>
		///   Replaces all invalid characters in a given path with a specified character.
		/// </summary>
		/// <param name="path">The path to validate and adjust.</param>
		/// <param name="validChar">The character to replace invalid characters with.</param>
		/// <returns>The original passed path, with all invalid characters replaced by the given character.</returns>
		/// <exception cref="ArgumentException">Thrown when validChar isn't a valid path character.</exception>
		public static string ReplaceInvalidChars( string path, char validChar )
		{
			// TODO: Separate invalid path chars from invalid filename chars?
			var invalidChars = Path.GetInvalidPathChars().Concat( Path.GetInvalidFileNameChars() );

			if ( invalidChars.Contains( validChar ) )
			{
				throw new ArgumentException( "The passed replacement character 'validChar' should be a valid path character." );
			}

			string charsString = invalidChars.Select( c => c.ToString() ).Aggregate( (a, b) => a + b );
			string regexMatchAnyChar = "[" + Regex.Escape( charsString ) + "]";
			return Regex.Replace( path, regexMatchAnyChar, validChar.ToString() );
		}
	}
}
