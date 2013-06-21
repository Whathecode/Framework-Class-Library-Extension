using System;

namespace Whathecode.System.Windows.Interop
{
    public class ShellEvents
    {
        public delegate void WindowCreatedEventHandler(WindowInfo newWindow);
        public delegate void WindowDestroyedEventHandler(IntPtr oldWindowHandle);
        public delegate void WindowActivatedEventHandler(WindowInfo window, bool fullscreen);
        public delegate void WindowReplacedEventHandler(WindowInfo oldWindow, WindowInfo newWindow);
        public delegate void WindowTitleChangeEventHandler(WindowInfo window, bool flash);
    }
    public enum ShellMessages
    {
        // ReSharper disable InconsistentNaming
        HSHELL_WINDOWCREATED = 1,
        HSHELL_WINDOWDESTROYED = 2,
        HSHELL_ACTIVATESHELLWINDOW = 3,
        HSHELL_WINDOWACTIVATED = 4,
        HSHELL_GETMINRECT = 5,
        HSHELL_REDRAW = 6,
        HSHELL_TASKMAN = 7,
        HSHELL_LANGUAGE = 8,
        HSHELL_SYSMENU = 9,
        HSHELL_ENDTASK = 10,
        HSHELL_ACCESSIBILITYSTATE = 11,
        HSHELL_APPCOMMAND = 12,
        HSHELL_WINDOWREPLACED = 13,
        HSHELL_WINDOWREPLACING = 14,
        HSHELL_HIGHBIT = 0x8000,
        HSHELL_FLASH = (HSHELL_REDRAW | HSHELL_HIGHBIT),
        HSHELL_RUDEAPPACTIVATED = (HSHELL_WINDOWACTIVATED | HSHELL_HIGHBIT)
        // ReSharper restore InconsistentNaming
    }
}
