using System;


namespace Lambda.Generic
{


	#region Unsigned Maths

	namespace Arithmetic
	{
		using T = Boolean;


		public struct BooleanMath : IBinaryMath<T>, IZeroProvider<T>, IOneProvider<T>
		{
			public T And( T a, T b )
			{
				return (a & b);
			}

			public T Or( T a, T b )
			{
				return (a | b);
			}

			public T Xor( T a, T b )
			{
				return (a ^ b);
			}

			public T Not( T a )
			{
				return (!a);
			}

			public T Zero
			{
				get { return false; }
			}

			public T One
			{
				get { return true; }
			}
		}
	}


	namespace Arithmetic
	{
		using T = Byte;


		public struct ByteMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (T)(a + b);
			}

			public T Subtract( T a, T b )
			{
				return (T)(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return (T)(a * b);
			}

			public T Divide( T a, T b )
			{
				return (T)(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (T)(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return (T)Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedByteMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked((T)(a + b));
			}

			public T Subtract( T a, T b )
			{
				return checked((T)(a - b));
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return checked((T)(a * b));
			}

			public T Divide( T a, T b )
			{
				return checked((T)(a / b));
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((T)(a * a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return checked((T)Math.Abs( a ));
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = UInt16;


		public struct UShortMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (T)(a + b);
			}

			public T Subtract( T a, T b )
			{
				return (T)(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return (T)(a * b);
			}

			public T Divide( T a, T b )
			{
				return (T)(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (T)(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return (T)Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedUShortMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked((T)(a + b));
			}

			public T Subtract( T a, T b )
			{
				return checked((T)(a - b));
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return checked((T)(a * b));
			}

			public T Divide( T a, T b )
			{
				return checked((T)(a / b));
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((T)(a * a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return checked((T)Math.Abs( a ));
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = UInt32;


		public struct UIntMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return (T)Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedUIntMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked(a + b);
			}

			public T Subtract( T a, T b )
			{
				return checked(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return checked(a * b);
			}

			public T Divide( T a, T b )
			{
				return checked(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((a * a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return checked((T)Math.Abs( a ));
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = UInt64;


		public struct ULongMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return a;
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedULongMath : IUnsignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked(a + b);
			}

			public T Subtract( T a, T b )
			{
				return checked(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Multiply( T a, T b )
			{
				return checked(a * b);
			}

			public T Divide( T a, T b )
			{
				return checked(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((a * a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return a;
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return checked(a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}

	#endregion


	#region Signed Maths

	namespace Arithmetic
	{
		using T = SByte;


		public struct SByteMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (T)(a + b);
			}

			public T Subtract( T a, T b )
			{
				return (T)(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (T)(-a);
			}

			public T Multiply( T a, T b )
			{
				return (T)(a * b);
			}

			public T Divide( T a, T b )
			{
				return (T)(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (T)(One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (T)(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedSByteMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked((T)(a + b));
			}

			public T Subtract( T a, T b )
			{
				return checked((T)(a - b));
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return checked((T)(-a));
			}

			public T Multiply( T a, T b )
			{
				return checked((T)(a * b));
			}

			public T Divide( T a, T b )
			{
				return checked((T)(a / b));
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return checked((T)(One / a));
			}

			public T Sqrt( T a )
			{
				return (sbyte)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((T)(a * a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = Int16;


		public struct ShortMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (T)(a + b);
			}

			public T Subtract( T a, T b )
			{
				return (T)(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (T)(-a);
			}

			public T Multiply( T a, T b )
			{
				return (T)(a * b);
			}

			public T Divide( T a, T b )
			{
				return (T)(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (T)(One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return (T)(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedShortMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked((T)(a + b));
			}

			public T Subtract( T a, T b )
			{
				return checked((T)(a - b));
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return checked((T)(-a));
			}

			public T Multiply( T a, T b )
			{
				return checked((T)(a * b));
			}

			public T Divide( T a, T b )
			{
				return checked((T)(a / b));
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return checked((T)(One / a));
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public T Sqrt( T a )
			{
				return (sbyte)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked((T)(a * a));
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return (T)(a & b);
			}

			public T Or( T a, T b )
			{
				return (T)(a | b);
			}

			public T Xor( T a, T b )
			{
				return (T)(a ^ b);
			}

			public T Not( T a )
			{
				return (T)(~a);
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = Int32;


		public struct IntMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedIntMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked(a + b);
			}

			public T Subtract( T a, T b )
			{
				return checked(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return checked(-a);
			}

			public T Multiply( T a, T b )
			{
				return checked(a * b);
			}

			public T Divide( T a, T b )
			{
				return checked(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return checked(One / a);
			}

			public T Sqrt( T a )
			{
				return (sbyte)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = Int64;


		public struct LongMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return (T)a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return (T)Math.Ceiling( a );
					case TypeRounding.Down:
						return (T)Math.Floor( a );
					case TypeRounding.Nearest:
						return (T)Math.Round( a );
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct CheckedLongMath : ISignedMath<T>
		{
			public T Add( T a, T b )
			{
				return checked(a + b);
			}

			public T Subtract( T a, T b )
			{
				return checked(a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return checked(-a);
			}

			public T Multiply( T a, T b )
			{
				return checked(a * b);
			}

			public T Divide( T a, T b )
			{
				return checked(a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return checked(One / a);
			}

			public T Sqrt( T a )
			{
				return (sbyte)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return checked(a * a);
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T And( T a, T b )
			{
				return a & b;
			}

			public T Or( T a, T b )
			{
				return a | b;
			}

			public T Xor( T a, T b )
			{
				return a ^ b;
			}

			public T Not( T a )
			{
				return ~a;
			}

			public T ConvertFrom( ulong a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( long a )
			{
				return checked(a);
			}

			public T ConvertFrom( double a )
			{
				return checked((T)a);
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				switch ( rounding )
				{
					case TypeRounding.Up:
						return checked((T)Math.Ceiling( a ));
					case TypeRounding.Down:
						return checked((T)Math.Floor( a ));
					case TypeRounding.Nearest:
						return checked((T)Math.Round( a ));
					default:
						throw new NotSupportedException();
				}
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}

	#endregion


	#region Rational Maths

	namespace Arithmetic
	{
		using T = Single;


		public struct FloatMath : IRationalMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Asin( T a )
			{
				return (T)Math.Asin( a );
			}

			public T Acos( T a )
			{
				return (T)Math.Acos( a );
			}

			public T Atan( T a )
			{
				return (T)Math.Atan( a );
			}

			public T Atan2( T a, T b )
			{
				return (T)Math.Atan2( a, b );
			}

			public T Sin( T a )
			{
				return (T)Math.Sin( a );
			}

			public T Cos( T a )
			{
				return (T)Math.Cos( a );
			}

			public T Tan( T a )
			{
				return (T)Math.Tan( a );
			}

			public T Log( T a )
			{
				return (T)Math.Log( a );
			}

			public T Exp( T a )
			{
				return (T)Math.Exp( a );
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return T.Epsilon; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return (T)a;
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct FuzzyFloatMath<F> : IRationalMath<T>
			where F : IConstant<T>, new()
		{
			static readonly T epsilon = new F().Value;

			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T Asin( T a )
			{
				return (T)Math.Asin( a );
			}

			public T Acos( T a )
			{
				return (T)Math.Acos( a );
			}

			public T Atan( T a )
			{
				return (T)Math.Atan( a );
			}

			public T Atan2( T a, T b )
			{
				return (T)Math.Atan2( a, b );
			}

			public T Sin( T a )
			{
				return (T)Math.Sin( a );
			}

			public T Cos( T a )
			{
				return (T)Math.Cos( a );
			}

			public T Tan( T a )
			{
				return (T)Math.Tan( a );
			}

			public T Log( T a )
			{
				return (T)Math.Log( a );
			}

			public T Exp( T a )
			{
				return (T)Math.Exp( a );
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return epsilon; }
			}

			public int Compare( T a, T b )
			{
				return a < b - epsilon
					? -1
					: a > b + epsilon ? 1 : 0;
			}

			public bool Equals( T a, T b )
			{
				return Math.Abs( a - b ) <= Epsilon;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return (T)a;
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = Double;


		public struct DoubleMath : IRationalMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T Asin( T a )
			{
				return Math.Asin( a );
			}

			public T Acos( T a )
			{
				return Math.Acos( a );
			}

			public T Atan( T a )
			{
				return Math.Atan( a );
			}

			public T Atan2( T a, T b )
			{
				return Math.Atan2( a, b );
			}

			public T Sin( T a )
			{
				return Math.Sin( a );
			}

			public T Cos( T a )
			{
				return Math.Cos( a );
			}

			public T Tan( T a )
			{
				return Math.Tan( a );
			}

			public T Log( T a )
			{
				return Math.Log( a );
			}

			public T Exp( T a )
			{
				return Math.Exp( a );
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return One; }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return a;
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}


		public struct FuzzyDoubleMath<F> : IRationalMath<T>
			where F : IConstant<T>, new()
		{
			static readonly T epsilon = new F().Value;

			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return Math.Sqrt( a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T Asin( T a )
			{
				return Math.Asin( a );
			}

			public T Acos( T a )
			{
				return Math.Acos( a );
			}

			public T Atan( T a )
			{
				return Math.Atan( a );
			}

			public T Atan2( T a, T b )
			{
				return Math.Atan2( a, b );
			}

			public T Sin( T a )
			{
				return Math.Sin( a );
			}

			public T Cos( T a )
			{
				return Math.Cos( a );
			}

			public T Tan( T a )
			{
				return Math.Tan( a );
			}

			public T Log( T a )
			{
				return Math.Log( a );
			}

			public T Exp( T a )
			{
				return Math.Exp( a );
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return epsilon; }
			}

			public int Compare( T a, T b )
			{
				return a < b - epsilon
					? -1
					: a > b + epsilon ? 1 : 0;
			}

			public bool Equals( T a, T b )
			{
				return Math.Abs( a - b ) <= Epsilon;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return a;
			}

			public double ConvertToDouble( T from )
			{
				return from;
			}
		}
	}


	namespace Arithmetic
	{
		using T = Decimal;


		public struct DecimalMath : IRationalMath<T>
		{
			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( (double)a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T Asin( T a )
			{
				return (T)Math.Asin( (double)a );
			}

			public T Acos( T a )
			{
				return (T)Math.Acos( (double)a );
			}

			public T Atan( T a )
			{
				return (T)Math.Atan( (double)a );
			}

			public T Atan2( T a, T b )
			{
				return (T)Math.Atan2( (double)a, (double)b );
			}

			public T Sin( T a )
			{
				return (T)Math.Sin( (double)a );
			}

			public T Cos( T a )
			{
				return (T)Math.Cos( (double)a );
			}

			public T Tan( T a )
			{
				return (T)Math.Tan( (double)a );
			}

			public T Log( T a )
			{
				return (T)Math.Log( (double)a );
			}

			public T Exp( T a )
			{
				return (T)Math.Exp( (double)a );
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return new decimal( 1, 0, 0, false, 28 ); }
			}

			public int Compare( T a, T b )
			{
				return a.CompareTo( b );
			}

			public bool Equals( T a, T b )
			{
				return a == b;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return (T)a;
			}

			public double ConvertToDouble( T from )
			{
				return (double)from;
			}
		}


		public struct FuzzyDecimalMath<F> : IRationalMath<T>
			where F : IConstant<T>, new()
		{
			static readonly T epsilon = new F().Value;

			public T Add( T a, T b )
			{
				return (a + b);
			}

			public T Subtract( T a, T b )
			{
				return (a - b);
			}

			public T Zero
			{
				get { return 0; }
			}

			public T Negate( T a )
			{
				return (-a);
			}

			public T Multiply( T a, T b )
			{
				return (a * b);
			}

			public T Divide( T a, T b )
			{
				return (a / b);
			}

			public T One
			{
				get { return 1; }
			}

			public T Invert( T a )
			{
				return (One / a);
			}

			public T Sqrt( T a )
			{
				return (T)Math.Sqrt( (double)a );
			}

			public T Sqr( T a )
			{
				return a * a;
			}

			public T Asin( T a )
			{
				return (T)Math.Asin( (double)a );
			}

			public T Acos( T a )
			{
				return (T)Math.Acos( (double)a );
			}

			public T Atan( T a )
			{
				return (T)Math.Atan( (double)a );
			}

			public T Atan2( T a, T b )
			{
				return (T)Math.Atan2( (double)a, (double)b );
			}

			public T Sin( T a )
			{
				return (T)Math.Sin( (double)a );
			}

			public T Cos( T a )
			{
				return (T)Math.Cos( (double)a );
			}

			public T Tan( T a )
			{
				return (T)Math.Tan( (double)a );
			}

			public T Log( T a )
			{
				return (T)Math.Log( (double)a );
			}

			public T Exp( T a )
			{
				return (T)Math.Exp( (double)a );
			}

			public T MinValue
			{
				get { return T.MinValue; }
			}

			public T MaxValue
			{
				get { return T.MaxValue; }
			}

			public T Epsilon
			{
				get { return epsilon; }
			}

			public int Compare( T a, T b )
			{
				return a < b - epsilon
					? -1
					: a > b + epsilon ? 1 : 0;
			}

			public bool Equals( T a, T b )
			{
				return Math.Abs( a - b ) <= Epsilon;
			}

			public int GetHashCode( T a )
			{
				return a.GetHashCode();
			}

			public T Abs( T a )
			{
				return Math.Abs( a );
			}

			public T Min( T a, T b )
			{
				return Math.Min( a, b );
			}

			public T Max( T a, T b )
			{
				return Math.Max( a, b );
			}

			public T ConvertFrom( ulong a )
			{
				return a;
			}

			public T ConvertFrom( long a )
			{
				return a;
			}

			public T ConvertFrom( double a )
			{
				return (T)a;
			}

			public T ConvertFrom( double a, TypeRounding rounding )
			{
				return (T)a;
			}

			public double ConvertToDouble( T from )
			{
				return (double)from;
			}
		}
	}

	#endregion
}