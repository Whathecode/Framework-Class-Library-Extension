using System.Linq;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
    /// <summary>
    ///   Condition which validates to true when any of it's added conditions validate to true.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public class OrCondition : AbstractCombineCondition
    {
        public OrCondition() {}

        public OrCondition( params AbstractCondition[] conditions )
            : base( conditions ) {}


        protected override bool InputValidates()
        {
            return Conditions.Any( c => c.Validates() );
        }
    }
}