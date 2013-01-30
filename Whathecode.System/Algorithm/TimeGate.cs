using System;


namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   A gate which waits an amount of time before opening, starting from the first attempt at passing it.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class TimeGate : AbstractGate
	{
		DateTime? _lastPassTime = null;
		readonly TimeSpan _waitTime;


		public TimeGate( TimeSpan waitTime, bool autoReset = false )
			: base( autoReset )
		{
			_waitTime = waitTime;
		}


		protected override bool TryEnterGate()
		{
			DateTime now = DateTime.Now;

			if ( _lastPassTime == null )
			{
				_lastPassTime = now;
			}

			if ( now - _lastPassTime > _waitTime )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void ResetGate()
		{
			//   TODO: When autoreset is set, right now the 'timer' starts running again from first reentry without taking the previously passed time into account.
			//         This might sometimes be necessary.
			_lastPassTime = null;
		}
	}
}
