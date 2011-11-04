namespace Whathecode.System.Algorithm
{
	/// <summary>
	///   Option which specifies how a split operation should be handled.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public enum SplitOption
	{
		/// <summary>
		///   The location where is split is excluded from both results.
		/// </summary>
		None,
		/// <summary>
		///   The location where is split is included in both results.
		/// </summary>
		Both,
		/// <summary>
		///   The location where is split is only included in the left result set.
		/// </summary>
		Left,
		/// <summary>
		///   The location where is split is only included in the right result set.
		/// </summary>
		Right
	}
}