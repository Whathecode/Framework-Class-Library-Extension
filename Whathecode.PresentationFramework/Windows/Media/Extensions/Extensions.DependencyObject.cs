using System.Windows;
using System.Windows.Media;


namespace Whathecode.System.Windows.Media.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns the first parent of a specific type of the passed visual object.
		/// </summary>
		/// <typeparam name="T">The type of the parent to look for.</typeparam>
		/// <param name="reference">The visual whose parent is returned.</param>
		public static T FindParent<T>( this DependencyObject reference )
			where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent( reference );
			if ( parent == null )
			{
				return null;
			}
			else
			{
				return parent is T ? (T)parent : FindParent<T>( parent );
			}
		}
	}
}
