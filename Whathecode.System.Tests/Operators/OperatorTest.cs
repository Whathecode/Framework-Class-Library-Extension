using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Operators;


namespace Whathecode.Tests.System.Operators
{
	/// <summary>
	///   Unit test for <see cref="Operator{T}" />.
	/// </summary>
	[TestClass]
	public class OperatorTest
	{
		struct CustomOperatorsClass
		{
			public int Value { get; private set; }


			public CustomOperatorsClass( int value )
				: this()
			{
				Value = value;
			}

			static public CustomOperatorsClass operator +( CustomOperatorsClass a, CustomOperatorsClass b )
			{
				return new CustomOperatorsClass( a.Value + b.Value );
			}
		}

		/// <summary>
		///   Test whether the operator class works for classes with overloaded operators.
		/// </summary>
		[TestMethod]
		public void OperatorOverloadTest()
		{
			var a = new CustomOperatorsClass( 40 );
			var b = new CustomOperatorsClass( 2 );
			var add = Operator<CustomOperatorsClass>.Add;
			var answer = add( a, b );
			Assert.AreEqual( answer.Value, 42 );
		}
	}
}
