using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;
using Whathecode.System.Linq;


namespace Whathecode.Tests.System
{
	/// <summary>
	///   Unit tests for <see href = "EnumHelper{T}" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class EnumHelperTest
	{
		[Flags]
		public enum FlagsEnum
		{
			Flag1,
			Flag2,
			Flag3,
			Flag4,
			Flag5
		}


		[TestMethod]
		public void GetFlaggedValuesTest()
		{
			const FlagsEnum flags = FlagsEnum.Flag1 | FlagsEnum.Flag3;
			IEnumerable<FlagsEnum> setFlags = EnumHelper<FlagsEnum>.GetFlaggedValues( flags );

			Assert.IsTrue( setFlags.ContainsOnly( new[] { FlagsEnum.Flag1, FlagsEnum.Flag3 } ) );
		}
	}
}
