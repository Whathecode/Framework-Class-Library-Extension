using System;
using System.Management;


namespace Whathecode.System.Management
{
	public class ProcessTracker : AbstractDisposable
	{
		bool _hasStarted;
		ManagementEventWatcher _startWatcher;
		ManagementEventWatcher _stopWatcher;

		/// <summary>
		///   Triggered when processes are run, including the command line parameters when applicable.
		/// </summary>
		public event Action<ProcessInfo> ProcessStarted = delegate { };
		/// <summary>
		///   Triggered when processes are stopped.
		/// </summary>
		public event Action<ProcessInfo> ProcessStopped = delegate { };


		public void Start()
		{
			var interval = new TimeSpan( 0, 0, 1 );
			const string isWin32Process = "TargetInstance isa \"Win32_Process\"";

			// Listen for started processes.
			var startQuery = new WqlEventQuery( "__InstanceCreationEvent", interval, isWin32Process );
			_startWatcher = new ManagementEventWatcher( startQuery );
			_startWatcher.Start();
			_startWatcher.EventArrived += OnStartEventArrived;

			// Listen for closed processes.
			var stopQuery = new WqlEventQuery( "__InstanceDeletionEvent", interval, isWin32Process );
			_stopWatcher = new ManagementEventWatcher( stopQuery );
			_stopWatcher.Start();
			_stopWatcher.EventArrived += OnStopEventArrived;

			_hasStarted = true;
		}

		public void Stop()
		{
			if ( !_hasStarted )
			{
				return;
			}

			_startWatcher.Stop();
			_stopWatcher.Stop();
			_hasStarted = false;
		}

		void OnStartEventArrived( object sender, EventArrivedEventArgs e )
		{
			var o = (ManagementBaseObject)e.NewEvent[ "TargetInstance" ];

			ProcessStarted( RetrieveProcessInfo( o ) );
		}

		void OnStopEventArrived( object sender, EventArrivedEventArgs e )
		{
			var o = (ManagementBaseObject)e.NewEvent[ "TargetInstance" ];

			ProcessStopped( RetrieveProcessInfo( o ) );
		}

		static ProcessInfo RetrieveProcessInfo( ManagementBaseObject o )
		{
			return new ProcessInfo(
				Convert.ToInt32( o[ "ProcessId" ] ),
				(string)o[ "Name" ],
				(string)o[ "CommandLine" ] );
		}

		protected override void FreeManagedResources()
		{
			Stop();
		}

		protected override void FreeUnmanagedResources()
		{
			// Nothing to do.
		}
	}
}
