using System.Linq;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   Condition which can only validate to true as long as other conditions didn't validate to true.
	/// </summary>
	public class ExclusiveCondition : AbstractCombineCondition
	{
		readonly AbstractCondition _condition;
		readonly AbstractCondition[] _excludeConditions;
		bool _canStillExecute = true;


		public ExclusiveCondition( AbstractCondition condition, params AbstractCondition[] excludeConditions )
			: base( excludeConditions.Concat( new [] { condition } ).ToArray() )
		{
			_condition = condition;
			_excludeConditions = excludeConditions;
		}


		public void Reset()
		{
			_canStillExecute = true;
		}

		protected override bool InputValidates()
		{
			if ( !_canStillExecute )
			{
				return false;
			}

			_canStillExecute = _excludeConditions.All( c => !c.Validates() );
			return _canStillExecute && _condition.Validates();
		}
	}
}
