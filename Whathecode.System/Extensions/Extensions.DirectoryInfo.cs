using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns an enumerable collection of file information that matches a regular expression.
		/// </summary>
		/// <param name = "directory">The directory to search in.</param>
		/// <param name = "searchOption">Specifies whether to search in all subdirectories as well.</param>
		/// <param name = "regexPattern">The regular expression which is used to see whether a file matches.</param>
		/// <param name = "regexOptions">Options for the regular expression.</param>
		public static IEnumerable<FileInfo> EnumerateFiles(
			this DirectoryInfo directory,
			SearchOption searchOption,
			string regexPattern,
			RegexOptions regexOptions = RegexOptions.None )
		{
			return directory.EnumerateFiles( "*", searchOption ).Where( file => Regex.IsMatch( file.FullName, regexPattern, regexOptions ) );
		}


		/// <summary>
		///   Returns the first common subdirectory if any, null otherwise.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "directory">The directory to check for a common subdirectory.</param>
		/// <returns>The first common subdirectory if any, null otherwise.</returns>
		public static DirectoryInfo GetCommonParentDirectory( this DirectoryInfo source, DirectoryInfo directory )
		{
			Func<DirectoryInfo, Stack<DirectoryInfo>> getParentDirs = d =>
			{
				Stack<DirectoryInfo> sourceParents = new Stack<DirectoryInfo>();
				while ( d.Parent != null )
				{
					sourceParents.Push( d );
					d = d.Parent;
				}
				sourceParents.Push( d );
				return sourceParents;
			};

			Stack<DirectoryInfo> parents1 = getParentDirs( source );
			Stack<DirectoryInfo> parents2 = getParentDirs( directory );

			DirectoryInfo lastMatching = null;
			while ( parents1.Count > 0 && parents2.Count > 0 )
			{
				DirectoryInfo p1 = parents1.Pop();
				DirectoryInfo p2 = parents2.Pop();
				if ( p1.FullName == p2.FullName )
				{
					lastMatching = p1;
				}
				else
				{
					break;
				}
			}

			return lastMatching;
		}
	}
}