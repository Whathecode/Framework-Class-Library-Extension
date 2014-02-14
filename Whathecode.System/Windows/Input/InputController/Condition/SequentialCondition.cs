namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   Condition which validates to true when a sequence of condition validates in order.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SequentialCondition : AbstractCombineCondition
	{
		int _currentPosition;


		public SequentialCondition( params AbstractCondition[] conditions )
			: base( conditions )
		{
		}


		protected override bool InputValidates()
		{
			if ( Conditions.Count == 0 )
			{
				return false;
			}

			if ( Conditions[ _currentPosition ].Validates() )
			{
				if ( ++_currentPosition == Conditions.Count )
				{
					_currentPosition = 0;
					return true;
				}
			}

			return false;
		}
	}
}
