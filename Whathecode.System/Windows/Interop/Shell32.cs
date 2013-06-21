using System;
using System.Runtime.InteropServices;

namespace Whathecode.System.Windows.Interop
{
    class Shell32
    {
        const string Dll = "shell32.dll";

        /// <summary>
        /// No official documentation available
        /// </summary>
        [DllImport(Dll, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterShellHook(IntPtr hwnd, int flags);

        /// <summary>
        /// No official documentation available
        /// </summary>
        [DllImport(Dll, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SHSetKnownFolderPath(ref Guid folderId, uint flags, IntPtr token, [MarshalAs(UnmanagedType.LPWStr)] string path);

        /// <summary>
        /// No official documentation available
        /// </summary>
        [DllImport(Dll, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
    }
}
