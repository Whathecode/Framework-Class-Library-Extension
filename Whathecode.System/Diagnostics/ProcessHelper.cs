using System;
using System.Diagnostics;
using System.IO;


namespace Whathecode.System.Diagnostics
{
	/// <summary>
	///   A helper class to do common <see cref="Process" /> operations.
	/// </summary>
	/// <author>Eric White</author>
	/// <author>Steven Jeuris <email>mailto:steven.jeuris@gmail.com</email></author>
	/// <remarks>
	///   Based on the work of Eric White.
	///   http://blogs.msdn.com/ericwhite/archive/2008/08/07/running-an-executable-and-collecting-the-output.aspx
	/// </remarks>
	public static partial class ProcessHelper
	{
		/// <summary>
		///   Sets up a process, ready for execution.
		/// </summary>
		/// <param name = "executablePath">Path to the executable.</param>
		/// <param name = "arguments">The arguments to pass along.</param>
		/// <param name = "workingDirectory">The directory to use as working directory when running the executable.</param>
		/// <param name = "hideWindow">Determines whether the window of the launched process should be hidden or not.</param>
		/// <returns>A RunResults object which contains the output of the executable, plus runtime information.</returns>
		public static RunConfiguration SetUp( string executablePath, string arguments, string workingDirectory = "", bool hideWindow = false )
		{
			Process proc = new Process();

			if ( File.Exists( executablePath ) )
			{
				proc.StartInfo.FileName = executablePath;
				proc.StartInfo.Arguments = arguments;
				proc.StartInfo.WorkingDirectory = workingDirectory;
				if ( hideWindow )
				{
					proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					proc.StartInfo.CreateNoWindow = true;
				}
				proc.StartInfo.UseShellExecute = false;
			}
			else
			{
				throw new ArgumentException( "Invalid executable path.", "executablePath" );
			}

			return new RunConfiguration( proc );
		}
	}
}