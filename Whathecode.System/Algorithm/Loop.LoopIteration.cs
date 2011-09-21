using System;


namespace Whathecode.System.Algorithm
{
	public partial class Loop
	{
		/// <summary>
		///   Represents one interation in a loop.
		/// </summary>
		/// <author>Steven Jeuris</author>
		public struct LoopIteration
		{
			/// <summary>
			///   The operation to perform this iteration.
			/// </summary>
			public Action Operation { get; set; }

			/// <summary>
			///   The zero based index which specifies which interation this is.
			/// </summary>
			public int Index { get; set; }
		}
	}
}