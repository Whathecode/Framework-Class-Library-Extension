using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Whathecode.System.Arithmetic.Interpolation;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider.Windows;


namespace InterpolationTest
{
	/// <summary>
	///   Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			// Create a set of known points.
			CumulativeKeyPointCollection<Point, double> keyPoints = new CumulativeKeyPointCollection<Point, double>(
				new PointInterpolationProvider()
				)
			{
				new Point( 50, 50 ),
				new Point( 100, 100 ),
				new Point( 100, 200 ),
				new Point( 300, 20 ),
				new Point( 350, 140 ),
				new Point( 60, 300 ),
				new Point( 300, 250 ),
				new Point( 300, 330 )
			};
			AbstractInterpolation<Point, double> interpolation = new CardinalSplineInterpolation<Point, double>( keyPoints );

			// Create a set of interpolated points between the known points.
			List<Point> interpolatedPoints = new List<Point>();
			double curPercentage = 0;
			while ( curPercentage < 1 )
			{
				interpolatedPoints.Add( interpolation.Interpolate( curPercentage ) );
				curPercentage += 0.001;
			}

			// Create line segments.
			IEnumerable<LineSegment> lines = interpolatedPoints.Select( p => new LineSegment( p, true ) );

			// Create graph.
			PathFigure graphFigure = new PathFigure( keyPoints[ 0 ], lines, false );
			PathGeometry graphGeometry = new PathGeometry( new List<PathFigure>
			{
				graphFigure
			} );
			Path graph = new Path
			{
				Stroke = Brushes.Black,
				Data = graphGeometry
			};
			DrawCanvas.Children.Add( graph );

			// Create points.
			const double pointSize = 5;
			foreach ( var p in keyPoints )
			{
				Rectangle rect = new Rectangle();
				Canvas.SetLeft( rect, p.X - pointSize / 2 );
				Canvas.SetTop( rect, p.Y - pointSize / 2 );
				rect.Width = pointSize;
				rect.Height = pointSize;
				rect.Fill = Brushes.Red;
				DrawCanvas.Children.Add( rect );
			}

			// Draw tangent at a desired location.
			const double tangentAtPercentage = 0.7;
			const double tangentLenght = 100;
			Point startTangent = interpolation.Interpolate( tangentAtPercentage );
			Point tangent = interpolation.TangentAt( tangentAtPercentage );
			Vector3D normalized = new Vector3D( tangent.X, tangent.Y, 0 );
			normalized.Normalize();
			tangent = new Point( normalized.X, normalized.Y );
			Line tangentLine = new Line
			{
				X1 = startTangent.X,
				Y1 = startTangent.Y,
				X2 = startTangent.X + tangent.X * tangentLenght,
				Y2 = startTangent.Y + tangent.Y * tangentLenght,
				Stroke = Brushes.Blue
			};

			DrawCanvas.Children.Add( tangentLine );
		}
	}
}