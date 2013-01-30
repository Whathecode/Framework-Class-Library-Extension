namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   A gate which skips a specified amount of entries before opening.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SkipGate : AbstractGate
	{
		readonly int _skipCount;
		int _curCount;


		public SkipGate( int skipCount, bool autoReset = false )
			: base( autoReset )
		{
			_skipCount = skipCount;
		}


		protected override bool TryEnterGate()
		{
			return ++_curCount > _skipCount;
		}

		protected override void ResetGate()
		{
			_curCount = 0;
		}
	}
}
