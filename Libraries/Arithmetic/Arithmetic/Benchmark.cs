using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


namespace Lambda.Generic.Arithmetic.Test
{
	using PointG = Point<int, IntMath>;
	using SizeG = Size<int, IntMath>;
	using PointFG = Point<float, FloatMath>;
	using SizeFG = Size<float, FloatMath>;
	using PointDG = Point<double, DoubleMath>;
	using SizeDG = Size<double, DoubleMath>;
	using PointDecG = Point<decimal, DecimalMath>;
	using SizeDecG = Size<decimal, DecimalMath>;


	public class GenericArithmeticsTest
	{
		static readonly List<int> list;

		static GenericArithmeticsTest()
		{
			list = new List<int>( 1000000 );
			for ( int i = 0; i < 1000000; i++ )
			{
				list.Add( i / 100000 );
			}
		}

		public static object TestPoint()
		{
			int sum = 0;
			for ( Point a = new Point( 0, 0 ); a.X < 1000; a += new Size( 1, 0 ) )
			{
				for ( Point b = a; b.Y < 1000; b += new Size( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestPointG()
		{
			int sum = 0;
			for ( PointG a = new PointG( 0, 0 ); a.X < 1000; a += new SizeG( 1, 0 ) )
			{
				for ( PointG b = a; b.Y < 1000; b += new SizeG( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestPointF()
		{
			float sum = 0;
			for ( PointF a = new PointF( 0, 0 ); a.X < 1000; a += new SizeF( 1, 0 ) )
			{
				for ( PointF b = a; b.Y < 1000; b += new SizeF( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestPointFG()
		{
			float sum = 0;
			for ( PointFG a = new PointFG( 0, 0 ); a.X < 1000; a += new SizeFG( 1, 0 ) )
			{
				for ( PointFG b = a; b.Y < 1000; b += new SizeFG( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestPointDG()
		{
			double sum = 0;
			for ( PointDG a = new PointDG( 0, 0 ); a.X < 1000; a += new SizeDG( 1, 0 ) )
			{
				for ( PointDG b = a; b.Y < 1000; b += new SizeDG( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestPointDecG()
		{
			decimal sum = 0;
			for ( PointDecG a = new PointDecG( 0, 0 ); a.X < 1000; a += new SizeDecG( 1, 0 ) )
			{
				for ( PointDecG b = a; b.Y < 1000; b += new SizeDecG( 0, 1 ) )
				{
					sum += b.X + b.Y;
				}
			}
			return sum;
		}

		public static object TestListSigma()
		{
			int avg = 0;
			for ( int i = 0; i < list.Count; i++ )
			{
				avg += list[ i ];
			}
			avg /= list.Count;
			int rms = 0;
			for ( int i = 0; i < list.Count; i++ )
			{
				int d = list[ i ] - avg;
				rms += d * d;
			}
			return (int)Math.Sqrt( rms / list.Count );
		}

		public static object TestListSigmaG()
		{
			int avg;
			return Lists<int, IntMath>.Sigma( list, out avg );
		}

		public static object TestListSigmaG2()
		{
			Signed<int, IntMath> avg = 0;
			for ( int i = 0; i < list.Count; i++ )
			{
				avg += list[ i ];
			}
			avg /= list.Count;
			Signed<int, IntMath> rms = 0;
			for ( int i = 0; i < list.Count; i++ )
			{
				Signed<int, IntMath> d = list[ i ] - avg;
				rms += d * d;
			}
			return (int)Math.Sqrt( rms / list.Count );
		}

		public static object TestListSigmaG3()
		{
			int avg;
			return Lists<int, CheckedIntMath>.Sigma( list, out avg );
		}


		public delegate object TestDelegate();


		public static void Benchmark( TestDelegate testMethod )
		{
			ThreadPriority p = Thread.CurrentThread.Priority;
			try
			{
#if(!DEBUG)
				Thread.CurrentThread.Priority = ThreadPriority.Highest;
#endif
				DateTime time0, time1, endtime;
				time0 = DateTime.Now;
				time1 = time0 + new TimeSpan( 0, 0, 1 );
				//for the JIT
				while ( (endtime = DateTime.Now) < time1 )
				{
					testMethod();
				}

				int i = 0;
				time0 = DateTime.Now;
				time1 = time0 + new TimeSpan( 0, 0, 5 );
				//this is the real test. Everything should be JITed by now.
				for ( i = 0; (endtime = DateTime.Now) < time1; i++ )
				{
					testMethod();
				}
				TimeSpan deltat = new TimeSpan( (endtime.Ticks - time0.Ticks) / i );

				Console.WriteLine(
					"Method {0} took {1} ",
					testMethod.Method.Name,
					deltat );
			}
			finally
			{
				Thread.CurrentThread.Priority = p;
			}
		}

		public static void Main()
		{
#if(DEBUG)
			Console.WriteLine( "Warning: benchmarking in debug mode will not produce meaningful results!" );
#endif
			Console.WriteLine( "Non-generic standard deviation of 1000000 int elements" );
			Benchmark( TestListSigma );
			Console.WriteLine( "Generic standard deviation of 1000000 int elements" );
			Benchmark( TestListSigmaG );
			Console.WriteLine( "Generic standard deviation of 1000000 int elements using wrapper structs" );
			Benchmark( TestListSigmaG2 );
			Console.WriteLine( "Generic standard deviation of 1000000 int elements using overflow checks" );
			Benchmark( TestListSigmaG3 );
			Console.WriteLine( "Non-generic System.Drawing.Point operations" );
			Benchmark( TestPoint );
			Console.WriteLine( "Generic Point<int> operations" );
			Benchmark( TestPointG );
			Console.WriteLine( "Non-generic System.Drawing.PointF operations" );
			Benchmark( TestPointF );
			Console.WriteLine( "Generic Point<float> operations" );
			Benchmark( TestPointFG );
			Console.WriteLine( "Generic Point<double> operations" );
			Benchmark( TestPointDG );
			Console.WriteLine( "Generic Point<decimal> operations" );
			Benchmark( TestPointDecG );
			Console.ReadLine();
		}
	}
}