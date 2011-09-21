namespace Lambda.Generic.Arithmetic
{


	#region Unsigned Wrapper

	public struct Unsigned<T, O>
		where O : IUnsignedMath<T>, new()
	{
		static readonly O o = new O();
		readonly T value;

		public Unsigned( T a )
		{
			value = a;
		}

		public static explicit operator Unsigned<T, O>( ulong a )
		{
			return o.ConvertFrom( a );
		}

		public static implicit operator Unsigned<T, O>( T a )
		{
			return new Unsigned<T, O>( a );
		}

		public static implicit operator T( Unsigned<T, O> a )
		{
			return a.value;
		}

		public static Unsigned<T, O> operator +( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Add( a, b );
		}

		public static Unsigned<T, O> operator -( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Subtract( a, b );
		}

		public static Unsigned<T, O> operator *( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Multiply( a, b );
		}

		public static Unsigned<T, O> operator /( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Divide( a, b );
		}

		public static Unsigned<T, O> Zero
		{
			get { return o.Zero; }
		}

		public static Unsigned<T, O> One
		{
			get { return o.One; }
		}

		public static bool operator ==( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Equals( a, b );
		}

		public static bool operator !=( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return !o.Equals( a, b );
		}

		public static bool operator <=( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Compare( a, b ) <= 0;
		}

		public static bool operator >=( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Compare( a, b ) >= 0;
		}

		public static bool operator <( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Compare( a, b ) < 0;
		}

		public static bool operator >( Unsigned<T, O> a, Unsigned<T, O> b )
		{
			return o.Compare( a, b ) > 0;
		}

		public override bool Equals( object a )
		{
			return a is T
				? o.Equals( value, (T)a )
				: false;
		}

		public override int GetHashCode()
		{
			return o.GetHashCode( value );
		}
	}

	#endregion


	#region Signed Wrapper

	public struct Signed<T, O>
		where O : ISignedMath<T>, new()
	{
		static readonly O o = new O();
		readonly T value;

		public Signed( T a )
		{
			value = a;
		}

		public static explicit operator Signed<T, O>( ulong a )
		{
			return o.ConvertFrom( a );
		}

		public static explicit operator Signed<T, O>( long a )
		{
			return o.ConvertFrom( a );
		}

		public static implicit operator Signed<T, O>( T a )
		{
			return new Signed<T, O>( a );
		}

		public static implicit operator T( Signed<T, O> a )
		{
			return a.value;
		}

		public static Signed<T, O> operator +( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Add( a, b );
		}

		public static Signed<T, O> operator -( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Subtract( a, b );
		}

		public static Signed<T, O> operator -( Signed<T, O> a )
		{
			return o.Negate( a );
		}

		public static Signed<T, O> operator *( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Multiply( a, b );
		}

		public static Signed<T, O> operator /( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Divide( a, b );
		}

		public static Signed<T, O> Zero
		{
			get { return o.Zero; }
		}

		public static Signed<T, O> One
		{
			get { return o.One; }
		}

		public static bool operator ==( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Equals( a, b );
		}

		public static bool operator !=( Signed<T, O> a, Signed<T, O> b )
		{
			return !o.Equals( a, b );
		}

		public static bool operator <=( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Compare( a, b ) <= 0;
		}

		public static bool operator >=( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Compare( a, b ) >= 0;
		}

		public static bool operator <( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Compare( a, b ) < 0;
		}

		public static bool operator >( Signed<T, O> a, Signed<T, O> b )
		{
			return o.Compare( a, b ) > 0;
		}

		public override bool Equals( object a )
		{
			return a is T
				? o.Equals( value, (T)a )
				: false;
		}

		public override int GetHashCode()
		{
			return o.GetHashCode( value );
		}
	}

	#endregion


	#region Rational Wrapper

	public struct Rational<T, O>
		where O : IRationalMath<T>, new()
	{
		static readonly O o = new O();
		readonly T value;

		public Rational( T a )
		{
			value = a;
		}

		public static implicit operator Rational<T, O>( T a )
		{
			return new Rational<T, O>( a );
		}

		public static implicit operator T( Rational<T, O> a )
		{
			return a.value;
		}

		public static explicit operator Rational<T, O>( ulong a )
		{
			return o.ConvertFrom( a );
		}

		public static explicit operator Rational<T, O>( long a )
		{
			return o.ConvertFrom( a );
		}

		public static explicit operator Rational<T, O>( double a )
		{
			return o.ConvertFrom( a );
		}

		public static Rational<T, O> operator +( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Add( a, b );
		}

		public static Rational<T, O> operator -( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Subtract( a, b );
		}

		public static Rational<T, O> operator -( Rational<T, O> a )
		{
			return o.Negate( a );
		}

		public static Rational<T, O> operator *( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Multiply( a, b );
		}

		public static Rational<T, O> operator /( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Divide( a, b );
		}

		public Rational<T, O> Invert
		{
			get { return o.Invert( value ); }
		}

		public static Rational<T, O> Zero
		{
			get { return o.Zero; }
		}

		public static Rational<T, O> One
		{
			get { return o.One; }
		}

		public static bool operator ==( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Equals( a, b );
		}

		public static bool operator !=( Rational<T, O> a, Rational<T, O> b )
		{
			return !o.Equals( a, b );
		}

		public static bool operator <=( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Compare( a, b ) <= 0;
		}

		public static bool operator >=( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Compare( a, b ) >= 0;
		}

		public static bool operator <( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Compare( a, b ) < 0;
		}

		public static bool operator >( Rational<T, O> a, Rational<T, O> b )
		{
			return o.Compare( a, b ) > 0;
		}

		public override bool Equals( object a )
		{
			return a is T
				? o.Equals( value, (T)a )
				: false;
		}

		public override int GetHashCode()
		{
			return o.GetHashCode( value );
		}
	}

	#endregion
}