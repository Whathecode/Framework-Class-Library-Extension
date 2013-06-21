using System;
using System.IO;

namespace Whathecode.System.Windows.Interop
{
    public class DesktopFolderSwitcher
    {
        /// <summary>
        /// Updates the desktop folder
        /// </summary>
        public static void ChangeDesktopFolder(string path)
        {
            if (!Directory.Exists(path)) return;
            Shell32.SHSetKnownFolderPath(ref KnownFolder.Desktop, 0, IntPtr.Zero, path);
            Shell32.SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
