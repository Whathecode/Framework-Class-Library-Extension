using System.Collections.Generic;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   Condition which bases it's result based on a list of other conditions.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public abstract class AbstractCombineCondition : AbstractCondition
	{
		public List<AbstractCondition> Conditions { get; private set; }


		protected AbstractCombineCondition()
		{
			Conditions = new List<AbstractCondition>();
		}

		protected AbstractCombineCondition( params AbstractCondition[] conditions )
			: this()
		{
			Conditions.AddRange( conditions );
		}


		public override void Update()
		{
			foreach ( var c in Conditions )
			{
				c.Update();
			}
		}

		protected abstract override bool InputValidates();
	}
}