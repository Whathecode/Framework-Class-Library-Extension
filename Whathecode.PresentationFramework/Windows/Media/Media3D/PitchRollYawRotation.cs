using System.Windows.Media.Media3D;
using Whathecode.System.ComponentModel.Property;


namespace Whathecode.System.Windows.Media.Media3D
{
	/// <summary>
	///   Represents a 3-D rotation of specified angles about all three axis.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class PitchRollYawRotation
	{
		readonly Property<Point3D> _position = new Property<Point3D>();

		/// <summary>
		///   The position about which to rotate.
		/// </summary>
		public Point3D Position
		{
			get { return _position.GetValue(); }
			set
			{
				_position.SetValue(
					value,
					( oldValue, newValue ) =>
					{
						foreach ( var transform in new[] { _pitch, _roll, _yaw } )
						{
							transform.CenterX = newValue.X;
							transform.CenterY = newValue.Y;
							transform.CenterZ = newValue.Z;
						}
					} );
			}
		}

		readonly Property<Vector3D> _lookDirection = new Property<Vector3D>();

		/// <summary>
		///   Defines the direction around which is rolled, and from which is pitched up/down.
		/// </summary>
		public Vector3D LookDirection
		{
			get { return _lookDirection.GetValue(); }
			set
			{
				_lookDirection.SetValue(
					value,
					( oldValue, newValue ) =>
					{
						((AxisAngleRotation3D)_pitch.Rotation).Axis = GetPitchAxis();
						newValue.Negate();
						((AxisAngleRotation3D)_roll.Rotation).Axis = newValue;
					} );
			}
		}

		readonly Property<Vector3D> _upDirection = new Property<Vector3D>();

		/// <summary>
		///   Defines the up direction around which is yawed, and by which combined with LookDirection, 'horizontal' is determined.
		/// </summary>
		public Vector3D UpDirection
		{
			get { return _upDirection.GetValue(); }
			set
			{
				_upDirection.SetValue(
					value,
					( oldValue, newValue ) =>
					{
						((AxisAngleRotation3D)_pitch.Rotation).Axis = GetPitchAxis();
						((AxisAngleRotation3D)_yaw.Rotation).Axis = newValue;
					} );
			}
		}

		readonly RotateTransform3D _pitch;

		/// <summary>
		///   The angle in degrees for up/down elevation.
		///   A positive value corresponds to up, a negative value to down.
		/// </summary>
		public double Pitch
		{
			get { return ((AxisAngleRotation3D)_pitch.Rotation).Angle; }
			set { ((AxisAngleRotation3D)_pitch.Rotation).Angle = value; }
		}

		readonly RotateTransform3D _roll;

		/// <summary>
		///   The angle in degrees for the banking.
		///   A positive value corresponds to right banking, a negative value to left banking.
		/// </summary>
		public double Roll
		{
			get { return ((AxisAngleRotation3D)_roll.Rotation).Angle; }
			set { ((AxisAngleRotation3D)_roll.Rotation).Angle = value; }
		}

		readonly RotateTransform3D _yaw;

		/// <summary>
		///   The angle in degrees for the left/right heading.
		///   A positive value corresponds to a heading right, a negative value to heading left.
		/// </summary>
		public double Yaw
		{
			get { return ((AxisAngleRotation3D)_yaw.Rotation).Angle; }
			set { ((AxisAngleRotation3D)_yaw.Rotation).Angle = value; }
		}

		/// <summary>
		///   The 3-D transformation which represents the pitch, yaw and roll rotation.
		/// </summary>
		public Transform3D Transformation { get; private set; }


		public PitchRollYawRotation()
			: this( new Vector3D(), new Vector3D() ) {}

		public PitchRollYawRotation( Vector3D lookDirection, Vector3D upDirection )
		{
			LookDirection = lookDirection;
			UpDirection = upDirection;

			// Initialize axes.
			_pitch = new RotateTransform3D( new AxisAngleRotation3D( GetPitchAxis(), 0 ) );
			lookDirection.Negate();
			_roll = new RotateTransform3D( new AxisAngleRotation3D( lookDirection, 0 ) );
			_yaw = new RotateTransform3D( new AxisAngleRotation3D( upDirection, 0 ) );

			// Initialize transformation.            
			Transform3DGroup transformGroup = new Transform3DGroup();
			transformGroup.Children.Add( _pitch );
			transformGroup.Children.Add( _roll );
			transformGroup.Children.Add( _yaw );
			Transformation = transformGroup;
		}

		/// <summary>
		///   Returns the pitch axis for the current set look direction and up direction.
		/// </summary>
		Vector3D GetPitchAxis()
		{
			return Vector3D.CrossProduct( UpDirection, LookDirection );
		}
	}
}