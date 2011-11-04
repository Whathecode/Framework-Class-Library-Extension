using System.Diagnostics.Contracts;
using Whathecode.System.Algorithm;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Split a string into two strings at a specified position.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "index">The index where to split the string.</param>
		/// <param name = "splitOption">Option specifying whether to include the character where is split in the resulting strings.</param>
		/// <returns>A string array containing both strings after the split.</returns>
		public static string[] SplitAt( this string source, int index, SplitOption splitOption = SplitOption.None )
		{
			Contract.Requires( index >= 0 && index < source.Length );

			return new[]
			{
				source.Substring( 0, index + (splitOption.EqualsAny( SplitOption.Left, SplitOption.Both ) ? 1 : 0) ),
				source.Substring( index + (splitOption.EqualsAny( SplitOption.Right, SplitOption.Both ) ? 0 : 1) )
			};
		}
	}
}