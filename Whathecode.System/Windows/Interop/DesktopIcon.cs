
using System.Drawing;
namespace Whathecode.System.Windows.Interop
{
    public class DesktopIcon
    {
        public Point Location { get; set; }
        public int DesktopIndex { get; set; }

        public DesktopIcon() { }
        public DesktopIcon(int index, Point location)
        {
            DesktopIndex = index;
            Location = location;
        }
    }
}
