using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Whathecode.Tests.System.Reflection.Emit
{
	[TestClass]
	public partial class ProxyTest
	{
		public interface ITest<T>
		{
			T GetTest();
			void SetTest( T value );
		}


		public interface IMultipleParametersTest<T1, T2>
		{
			T1 GetT1();
			void SetT1( T1 value );
			T2 GetT2();
			void SetT2( T2 value );
		}


		public interface IExtendedTest<T> : ITest<T>
		{
			T GetExtendedTest();
			void SetExtendedTest( T value );
		}


		public interface IComposition<T>
		{
			ITest<T> GetTest();
			void SetTest( ITest<T> test );
		}

		public interface IGenericMethod<T>
		{
			void Generic<TGeneric>( TGeneric generic );
		}

		public class Test<T> : ITest<T>
		{
			T _value;

			public T GetTest()
			{
				return _value;
			}

			public void SetTest( T value )
			{
				_value = value;
			}
		}


		public class MultipleParametersTest : IMultipleParametersTest<string, int>
		{
			string _t1Value;
			int _t2Value;

			public string GetT1()
			{
				return _t1Value;
			}

			public void SetT1( string value )
			{
				_t1Value = value;
			}

			public int GetT2()
			{
				return _t2Value;
			}

			public void SetT2( int value )
			{
				_t2Value = value;
			}
		}


		public class ExtendedInterfaceTest : IExtendedTest<string>
		{
			string _value;
			string _extendedValue;

			public string GetTest()
			{
				return _value;
			}

			public void SetTest( string value )
			{
				_value = value;
			}

			public string GetExtendedTest()
			{
				return _extendedValue;
			}

			public void SetExtendedTest( string value )
			{
				_extendedValue = value;
			}
		}


		public class CompositionTest : IComposition<string>
		{
			ITest<string> _test = new Test<string>();

			public ITest<string> GetTest()
			{
				return _test;
			}

			public void SetTest( ITest<string> test )
			{
				_test = test;
			}
		}

		public class GenericMethodTest : IGenericMethod<string>
		{
			public void Generic<TGeneric>( TGeneric generic )
			{
				// TODO: What to test?
			}
		}
	}
}