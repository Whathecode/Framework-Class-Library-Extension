using System;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   Condition which delegates it's behavior.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class DelegateCondition : AbstractCondition
	{
		readonly Func<bool> _inputValidates;


		public DelegateCondition( Func<bool> inputValidates )
		{
			Contract.Requires( inputValidates != null );

			_inputValidates = inputValidates;
		}


		public override void Update()
		{
			// Nothing to do.
		}

		protected override bool InputValidates()
		{
			return _inputValidates();
		}
	}
}
