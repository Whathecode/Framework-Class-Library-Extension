namespace Whathecode.System.Windows.Input.InputController.Condition
{
    /// <summary>
    ///   Condition which validates to true when any of it's added conditions validate to true.
    /// </summary>
    /// <author>Steven Jeuris <email>mailto:steven@aimproductions.be</email></author>
    public class AndCondition : AbstractCombineCondition
    {
        public AndCondition() {}

        public AndCondition( params AbstractCondition[] conditions )
            : base( conditions ) {}


        protected override bool InputValidates()
        {
            return Conditions.Count != 0 && Conditions.TrueForAll( c => c.Validates() );
        }
    }
}