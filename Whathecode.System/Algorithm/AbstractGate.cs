namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   Class which helps checking for conditional entry.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractGate
	{
		protected bool AutoReset { get; private set; }


		protected AbstractGate( bool autoReset )
		{
			AutoReset = autoReset;
		}


		/// <summary>
		///   Attempts to enter the gate.
		/// </summary>
		/// <returns>True when allowed, false otherwise.</returns>
		public bool TryEnter()
		{
			bool entered = TryEnterGate();
			if ( entered && AutoReset )
			{
				ResetGate();
			}

			return entered;
		}

		/// <summary>
		///   Resets the state of the gate to its original position.
		/// </summary>
		public void Reset()
		{
			ResetGate();
		}


		protected abstract bool TryEnterGate();
		protected abstract void ResetGate();
	}
}
