namespace Whathecode.System.Windows.Interop
{
	/// <summary>
	///   Contains information about how a certain window should be repositioned when calling RepositionWindows() from <see cref="WindowManager" />.
	/// </summary>
	public class RepositionWindowInfo
	{
		internal WindowInfo ToPosition;

		readonly int _originalX;
		public int X { get; set; }

		readonly int _originalY;
		public int Y { get; set; }

		public bool HasPositionChanged()
		{
			return X != _originalX || Y != _originalY;
		}

		readonly int _originalWidth;
		public int Width { get; set; }

		readonly int _originalHeight;
		public int Height { get; set; }

		public bool HasSizeChanged()
		{
			return Width != _originalWidth || Height != _originalHeight;
		}


		readonly bool _originalVisible;
		public bool Visible { get; set; }

		public bool HasVisibilityChanged()
		{
			return _originalVisible != Visible;
		}


		public RepositionWindowInfo( WindowInfo toPosition )
		{
			ToPosition = toPosition;

			_originalVisible = toPosition.IsVisible();
			Visible = _originalVisible;

			var placement = new User32.WindowPlacement();
			User32.GetWindowPlacement( toPosition.Handle, ref placement );

			User32.Rectangle position = placement.NormalPosition;
			_originalX = position.Left;
			X = _originalX;
			_originalY = position.Top;
			Y = _originalY;
			// TODO: According to the documentation, the pixel at (right, bottom) lies immediately outside the rectangle. Do we need to subtract with 1?
			_originalWidth = position.Right - position.Left;
			Width = _originalWidth;
			_originalHeight = position.Bottom - position.Top;
			Height = _originalHeight;
		}
	}
}
