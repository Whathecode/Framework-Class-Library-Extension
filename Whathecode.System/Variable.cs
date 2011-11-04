namespace Whathecode.System
{
	/// <summary>
	///   Helper class to do common operations on variables.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class Variable
	{
		/// <summary>
		///   Swaps the value of one variable with another.
		/// </summary>
		/// <typeparam name="T">The type of the variables.</typeparam>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		public static void Swap<T>( ref T first, ref T second )
		{
			T intermediate = first;
			first = second;
			second = intermediate;
		}
	}
}
