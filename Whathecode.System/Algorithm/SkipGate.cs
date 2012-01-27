namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   A gate which skips a specified amount of entries before opening.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class SkipGate : IGate
	{
		readonly int _skipCount;
		int _curCount;


		public SkipGate( int skipCount )
		{
			_skipCount = skipCount;
		}


		public bool TryEnter()
		{
			return ++_curCount > _skipCount;
		}
	}
}
