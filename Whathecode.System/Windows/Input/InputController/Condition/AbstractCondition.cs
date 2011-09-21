using System;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   A class which can check whether a certain input is done.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractCondition
	{
		/// <summary>
		///   The amount of time the input needs to be valid for the inputcondition to validate.
		///   By default this time is set to 0 so the condition triggers immediatly.
		/// </summary>
		public TimeSpan Delay { get; set; }

		/// <summary>
		///   Does the input validate at this time.
		/// </summary>
		bool _inputValidates;

		/// <summary>
		///   Time when the input started being valid.
		/// </summary>
		DateTime _validatedStartTime;


		protected AbstractCondition()
		{
			Delay = new TimeSpan( 0 );
		}


		/// <summary>
		///   Update so the state of the input condition can change.
		/// </summary>
		public abstract void Update();

		/// <summary>
		///   Check whether input is currently valid. (Different from Validates()!)
		/// </summary>
		/// <returns>True when input is done, false otherwise.</returns>
		protected abstract bool InputValidates();


		/// <summary>
		///   Check whether input condition validates.
		///   This also takes into account the Duration property.
		/// </summary>
		/// <returns>True if condition validates, false otherwise.</returns>
		public bool Validates()
		{
			bool validatedBefore = _inputValidates;
			_inputValidates = InputValidates();

			if ( _inputValidates )
			{
				DateTime now = DateTime.Now;

				if ( !validatedBefore )
				{
					// Starting from now the condition validates to true.
					_validatedStartTime = now;
				}

				// Duration has passed?
				return now >= _validatedStartTime + Delay;
			}

			return false;
		}
	}
}