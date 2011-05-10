using System.Windows.Media.Media3D;


namespace Whathecode.System.Windows.Media.Media3D
{
    /// <summary>
    ///   A helper class to do common <see cref = "Matrix3D">Matrix3D</see> operations.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public static class Matrix3DHelper
    {
        /// <summary>
        ///   Construct a matrix from three given axes.
        /// </summary>
        /// <returns>A matrix constructed from three given axes.</returns>
        public static Matrix3D FromAxes( Vector3D uAxis, Vector3D vAxis, Vector3D wAxis )
        {
            return new Matrix3D
            {
                // x
                M11 = uAxis.X,
                M21 = vAxis.X,
                M31 = wAxis.X,
                // y
                M12 = uAxis.Y,
                M22 = vAxis.Y,
                M32 = wAxis.Y,
                // z
                M13 = uAxis.Z,
                M23 = vAxis.Z,
                M33 = wAxis.Z,
                // No transformation.
                M44 = 1
            };
        }
    }
}