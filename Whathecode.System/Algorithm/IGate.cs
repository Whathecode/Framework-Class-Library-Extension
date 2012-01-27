namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   Interface which helps checking for conditional entry.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public interface IGate
	{
		/// <summary>
		///   Attempts to enter the gate.
		/// </summary>
		/// <returns>True when allowed, false otherwise.</returns>
		bool TryEnter();
	}
}
