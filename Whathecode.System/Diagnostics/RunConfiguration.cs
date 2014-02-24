using System;
using System.Diagnostics;
using System.Text;


namespace Whathecode.System.Diagnostics
{
	public static partial class ProcessHelper
	{
		/// <summary>
		///   Class which contains all the results from running an executable.
		/// </summary>
		public class RunResults
		{
			public int ExitCode;
			public StringBuilder Output = new StringBuilder();
			public StringBuilder ErrorOutput = new StringBuilder();
		}


		public class RunConfiguration
		{
			readonly Process _process;


			internal RunConfiguration( Process process )
			{
				_process = process;
			}


			public void Run()
			{
				_process.Start();
				_process.Dispose();
			}

			public RunResults RunAwaitResult()
			{
				var runResults = new RunResults();

				_process.StartInfo.RedirectStandardOutput = true;
				_process.StartInfo.RedirectStandardError = true;
				_process.OutputDataReceived +=
					( o, e ) => runResults.Output.Append( e.Data ).Append( Environment.NewLine );
				_process.ErrorDataReceived +=
					( o, e ) => runResults.ErrorOutput.Append( e.Data ).Append( Environment.NewLine );

				_process.Start();
				_process.BeginOutputReadLine();
				_process.BeginErrorReadLine();

				_process.WaitForExit();
				runResults.ExitCode = _process.ExitCode;

				_process.Dispose();
				return runResults;
			}
		}
	}
}
