using System.Windows.Media.Media3D;


namespace Whathecode.System.Windows.Media.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Set the translation's offset.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "xOffset">The X-axis value of the translation's offset.</param>
		/// <param name = "yOffset">The Y-axis value of the translation's offset.</param>
		/// <param name = "zOffset">The Z-axis value of the translation's offset.</param>
		public static void SetOffset( this TranslateTransform3D source, double xOffset, double yOffset, double zOffset )
		{
			SetOffset( source, new Vector3D( xOffset, yOffset, zOffset ) );
		}

		/// <summary>
		///   Set the translation's offset.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "offset">The translation's offset.</param>
		public static void SetOffset( this TranslateTransform3D source, Vector3D offset )
		{
			source.OffsetX = offset.X;
			source.OffsetY = offset.Y;
			source.OffsetZ = offset.Z;
		}
	}
}