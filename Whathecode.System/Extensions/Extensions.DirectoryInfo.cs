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
        /// <param name = "searchOption">Specifies whether to search in all subdirectories as wlel.</param>
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
    }
}
