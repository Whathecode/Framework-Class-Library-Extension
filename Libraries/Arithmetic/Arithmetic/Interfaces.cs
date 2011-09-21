using System;
using System.Diagnostics.Contracts;


namespace Lambda.Generic.Arithmetic
{
	/// <summary>
	///   Rounding when given value can't be represented for this type.
	/// </summary>
	public enum TypeRounding
	{
		Up,
		Down,
		Nearest
	}


	#region Unconstrained Interfaces

	public interface IConversionProvider<T>
	{
		T ConvertFrom( ulong t );
		T ConvertFrom( long t );
		T ConvertFrom( double t );
		T ConvertFrom( double t, TypeRounding rounding );
		double ConvertToDouble( T from );
	}


	public interface IConstant<T>
	{
		T Value { get; }
	}


	public interface IAdder<T>
	{
		T Add( T a, T b );
	}


	public interface ISubtracter<T>
	{
		T Subtract( T a, T b );
	}


	public interface IZeroProvider<T>
	{
		T Zero { get; }
	}


	public interface INegater<T>
	{
		T Negate( T a );
	}


	public interface IMultiplier<T>
	{
		T Multiply( T a, T b );
	}


	public interface IDivider<T>
	{
		T Divide( T a, T b );
	}


	public interface IOneProvider<T>
	{
		T One { get; }
	}


	public interface IInverter<T>
	{
		T Invert( T a );
	}


	public interface IHasRoot<T>
	{
		T Sqrt( T a );
		T Sqr( T a );
	}


	public interface IMinValueProvider<T>
	{
		T MinValue { get; }
	}


	public interface IMaxValueProvider<T>
	{
		T MaxValue { get; }
	}


	public interface IEpsilonProvider<T>
	{
		T Epsilon { get; }
	}

	#endregion


	#region Constrained Interfaces

	public interface IComparer<T>
	{
		[Pure]
		int Compare( T a, T b );
		bool Equals( T a, T b );
		int GetHashCode( T a );
		T Abs( T a );
		T Max( T a, T b );
		T Min( T a, T b );
	}


	public interface ILimitProvider<T> :
		IMaxValueProvider<T>,
		IMinValueProvider<T>,
		IEpsilonProvider<T> {}


	public interface IBinaryMath<T>
	{
		T And( T a, T b );
		T Or( T a, T b );
		T Xor( T a, T b );
		T Not( T a );
	}


	/// <summary>
	///   This defines a Group with the operation +, the neutral element Zero,
	///   the inverse Negate and an operation - that is defined in terms of the inverse.
	/// 
	///   Axioms that have to be satisified by the operations:
	///   Commutativity of addition: Add(a,b)=Add(b,a) for all a,b in T
	///   Associativity of addition: Add(Add(a,b),c)=Add(a,Add(b,c))
	///   Inverse of addition: Add(a,Negate(a))==Zero
	///   Subtraction: Subtract(a,b)==Add(a,Negate(b))
	///   Neutral element: Add(Zero,a)==a for all a in T
	/// </summary>
	public interface IAdditionGroup<T> :
		IAdder<T>,
		ISubtracter<T>,
		INegater<T>,
		IZeroProvider<T> {}


	public interface ITrigonometry<T>
	{
		T Asin( T a );
		T Acos( T a );
		T Atan( T a );
		T Atan2( T a, T b );
		T Sin( T a );
		T Cos( T a );
		T Tan( T a );
	}


	public interface IExp<T>
	{
		T Exp( T a );
		T Log( T a );
	}


	/// <summary>
	///   This defines a Group with the operation *, the neutral element One,
	///   the inverse Inverse and an operation / that is defined in terms of the inverse.
	/// 
	///   Axioms that have to be satisified by the operations:
	///   Commutativity of multiplication: Multiply(a,b)=Multiply(b,a) for all a,b in T
	///   Associativity of multiplication: Multiply(Multiply(a,b),c)=Multiply(a,Multiply(b,c))
	///   Inverse of multiplication: Multiply(a,Inverse(a))==One for all a in T
	///   Divison: Divide(a,b)==Multiply(a,Inverse(b)) for all a in T
	///   Neutral element: Multiply(One,a)==a for all a in T
	/// </summary>
	public interface IMultiplicationGroup<T> :
		IMultiplier<T>,
		IDivider<T>,
		IInverter<T>,
		IOneProvider<T> {}


	/// <summary>
	///   This defines a Ring with the operations +,*        
	/// 
	///   Axioms that have to be satisified by the operations:
	///   The group axioms for +
	///   Associativity of *: a * (b*c) = (a*b) * c
	///   Neutral element of *: Multiply(One,a)==a for all a in T
	///   Distributivity: 
	///   a * (b+c) = (a*b) + (a*c) 
	///   (a+b) * c = (a*c) + (b*c)
	/// </summary>
	public interface IRing<T> :
		IAdditionGroup<T>,
		IMultiplier<T>,
		IOneProvider<T> {}


	/// <summary>
	///   This defines a Field with the operations +,-,*,/      
	///   /// 
	///   Axioms that have to be satisified by the operations:
	///   The group axioms for +
	///   The group axioms for *
	///   Associativity: a * (b*c) = (a*b) * c
	///   Distributivity: 
	///   a * (b+c) = (a*b) + (a*c) 
	///   (a+b) * c = (a*c) + (b*c)
	/// </summary>
	public interface IField<T> :
		IAdditionGroup<T>,
		IMultiplicationGroup<T> {}


	public interface IMath<T> :
		IAdder<T>,
		ISubtracter<T>,
		IMultiplier<T>,
		IDivider<T>,
		IOneProvider<T>,
		IZeroProvider<T>,
		IHasRoot<T>,
		IComparer<T>,
		ILimitProvider<T>,
		IConversionProvider<T> {}


	/// <summary>
	///   Use this interface for integer (whole number) types such a int,uint,byte.
	/// </summary>
	public interface IIntegerMath<T> : IMath<T>, IBinaryMath<T> {}


	/// <summary>
	///   Use this interface for unsigned types such as byte,ushort,uint,ulong.
	///   Unsigned types have no subtraction or inverse. I have added ISubtracter
	///   anyway since it might be useful in many situations.
	/// </summary>
	public interface IUnsignedMath<T> : IIntegerMath<T> {}


	/// <summary>
	///   Use this interface for signed types such as sbyte,short,int,long.
	///   signed types have a division, but no inverse.
	///   Strictly speaking, signed types do not support IHasRoot since e.g. 
	///   Sqrt(Negate(One)) is not element of T.
	///   But it would be very inconvenient to leave it out. If you do not want
	///   to support IHasRoot, just throw a NotImplementedException.
	/// </summary>
	public interface ISignedMath<T> :
		IIntegerMath<T>,
		IRing<T> {}


	/// <summary>
	///   Use this interface for rational types such as float,double,decimal.
	///   Rational types have an inverse.
	///   Strictly speaking, rational types do not support IHasRoot since e.g. 
	///   Sqrt(Negate(One)) is not element of T.
	///   But it would be very inconvenient to leave it out. If you do not want
	///   to support IHasRoot, just throw a NotImplementedException.
	/// </summary>
	public interface IRationalMath<T> :
		IMath<T>,
		IField<T>,
		ITrigonometry<T>,
		IExp<T> {}


	/// <summary>
	///   Use this interface for types such as complex numbers that satisfy 
	///   the field axioms but do not have a natural order.
	///   complex numbers of course do support IHasRoot.
	/// </summary>
	public interface IComplexMath<T> :
		IField<T>,
		IHasRoot<T>
	{
		T ConvertFrom( double x );
	}

	#endregion
}