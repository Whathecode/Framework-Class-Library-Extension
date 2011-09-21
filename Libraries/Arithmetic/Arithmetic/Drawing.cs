namespace Lambda.Generic.Arithmetic
{
	public struct Size<T, C>
		where C :
			IAdder<T>, ISubtracter<T>, IComparer<T>,
			new()
	{
		readonly T w, h;
		static readonly C c = new C();

		public static Size<T, C> Empty
		{
			get { return new Size<T, C>(); }
		}

		public bool IsEmpty
		{
			get { return c.Equals( Width, Empty.Width ) && c.Equals( Height, Empty.Height ); }
		}

		public T Width
		{
			get { return w; }
		}

		public T Height
		{
			get { return h; }
		}

		public Size( T width, T height )
		{
			w = width;
			h = height;
		}

		public static bool operator ==( Size<T, C> a, Size<T, C> b )
		{
			return c.Equals( a.Width, b.Width ) && c.Equals( a.Height, b.Height );
		}

		public static bool operator !=( Size<T, C> a, Size<T, C> b )
		{
			return !c.Equals( a.Width, b.Width ) || !c.Equals( a.Height, b.Height );
		}

		public override int GetHashCode()
		{
			return c.GetHashCode( Width ) ^ c.GetHashCode( Height );
		}

		public override bool Equals( object obj )
		{
			if ( obj is Size<T, C> )
			{
				return this == (Size<T, C>)obj;
			}
			else
			{
				return false;
			}
		}
	}


	public struct Point<T, C>
		where C :
			IAdder<T>, ISubtracter<T>, IComparer<T>,
			new()
	{
		static readonly C c = new C();
		readonly T x, y;

		public static Point<T, C> Empty
		{
			get { return new Point<T, C>(); }
		}

		public bool IsEmpty
		{
			get { return c.Equals( X, Empty.X ) && c.Equals( Y, Empty.Y ); }
		}

		public T X
		{
			get { return x; }
		}

		public T Y
		{
			get { return y; }
		}

		public Point( T x, T y )
		{
			this.x = x;
			this.y = y;
		}

		public static Point<T, C> operator +( Point<T, C> a, Size<T, C> b )
		{
			return new Point<T, C>( c.Add( a.X, b.Width ), c.Add( a.Y, b.Height ) );
		}

		public static Point<T, C> operator -( Point<T, C> a, Size<T, C> b )
		{
			return new Point<T, C>( c.Subtract( a.X, b.Width ), c.Subtract( a.Y, b.Height ) );
		}

		public static bool operator ==( Point<T, C> a, Point<T, C> b )
		{
			return c.Equals( a.X, b.X ) && c.Equals( a.Y, b.Y );
		}

		public static bool operator !=( Point<T, C> a, Point<T, C> b )
		{
			return !c.Equals( a.X, b.X ) || !c.Equals( a.Y, b.Y );
		}

		public override int GetHashCode()
		{
			return c.GetHashCode( X ) ^ c.GetHashCode( Y );
		}

		public override bool Equals( object obj )
		{
			return obj is Point<T, C>
				? this == (Point<T, C>)obj
				: false;
		}
	}
}