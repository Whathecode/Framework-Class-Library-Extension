using System;


namespace Whathecode.System.Management
{
	public class ProcessInfo
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string CommandLine { get; private set; }


		public ProcessInfo( int id, string name, string commandLine )
		{
			Id = id;
			Name = name;
			CommandLine = commandLine;
		}


		public override string ToString()
		{
			return String.Format( "PID: {0}, Name: {1}, Arguments: {2}", Id, Name, CommandLine );
		}
	}
}
