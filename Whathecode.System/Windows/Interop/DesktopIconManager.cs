using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Whathecode.System.Runtime.InteropServices;

namespace Whathecode.System.Windows.Interop
{
    public class DesktopIconManager
    {
        private static int  MakeLParam(int loWord, int hiWord)
        {
            return ((hiWord << 16) | (loWord & 0xffff));
        }
        public static List<DesktopIcon> SaveDestopIcons()
        {
            var vHandle = User32.FindWindow("Progman", "Program Manager");
            vHandle = User32.FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = User32.FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

            var vItemCount = (int)User32.SendMessage(vHandle, User32.LVM_GETITEMCOUNT, 0, 0);
            var icons = new List<DesktopIcon>();

            uint vProcessId;
            User32.GetWindowThreadProcessId(vHandle, out vProcessId);

            var vProcess = Kernel32.OpenProcess(Kernel32.PROCESS_VM_OPERATION | Kernel32.PROCESS_VM_READ |
                Kernel32.PROCESS_VM_WRITE, false, vProcessId);

            var vPointer = Kernel32.VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT, Kernel32.PAGE_READWRITE);
            try
            {
                for (var j = 0; j < vItemCount; j++)
                {
                    var vBuffer = new byte[256];
                    var vItem = new User32.Lvitem[1];
                    vItem[0].Mask = User32.LVIF_TEXT;
                    vItem[0].Item = j;
                    vItem[0].SubItem = 0;
                    vItem[0].CchTextMax = vBuffer.Length;
                    vItem[0].PszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(User32.Lvitem)));
                    uint vNumberOfBytesRead = 0;

                    Kernel32.WriteProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(User32.Lvitem)), ref vNumberOfBytesRead);
                    User32.SendMessage(vHandle, User32.LVM_GETITEMW, j, vPointer.ToInt32());
                    Kernel32.ReadProcessMemory(vProcess, (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(User32.Lvitem))), Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0), vBuffer.Length, ref vNumberOfBytesRead);
                    User32.SendMessage(vHandle, User32.LVM_GETITEMPOSITION, j, vPointer.ToInt32());
                    var vPoint = new Point[1];
                    Kernel32.ReadProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0), Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);
                    icons.Add(new DesktopIcon(j, vPoint[0]));
                }
            }
            catch (Exception ex)
            {
                Kernel32.VirtualFreeEx(vProcess, vPointer, 0, Kernel32.MEM_RELEASE);
                Kernel32.CloseHandle(vProcess);
                Console.WriteLine("Saving desktop icons Error: " + ex);
            }
            Console.WriteLine("SAVE Icon count: " + icons.Count + " / LVItem count: " + vItemCount);
            return icons;
        }
        public static void ArrangeDesktopIcons(List<DesktopIcon> icons)
        {
            var vHandle = User32.FindWindow("Progman", "Program Manager");
            vHandle = User32.FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = User32.FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

            int vItemCount;
            IntPtr vProcess;
            IntPtr vPointer;
            GetDesktopIconCount(vHandle, out vItemCount, out vProcess, out vPointer);

            if (vItemCount != icons.Count)
            {
                GetDesktopIconCount(vHandle, out vItemCount, out vProcess, out vPointer);
            }
            else
            {
                for (var j = 0; j < vItemCount; j++)
                {
                    SendMsg(vHandle, User32.LVM_GETITEMW, j, vPointer.ToInt32());
                    Console.WriteLine(icons[j].Location.ToString());
                    SendMsg(vHandle, User32.LVM_SETITEMPOSITION, j, MakeLParam(Convert.ToInt32(icons[j].Location.X),Convert.ToInt32( icons[j].Location.Y)));
                }
                Kernel32.VirtualFreeEx(vProcess, vPointer, 0, Kernel32.MEM_RELEASE);
                Kernel32.CloseHandle(vProcess);
            }
        }
        private static void SendMsg(IntPtr hWnd, uint msg, int wParam, int lParam)
        {
            var result = User32.SendMessage(hWnd, User32.LVM_SETITEMPOSITION, wParam, lParam);

            if (result != IntPtr.Zero) return;
            var err = Marshal.GetLastWin32Error();

            throw new Win32Exception(err);
        }
        private static void GetDesktopIconCount(IntPtr vHandle, out int vItemCount, out IntPtr vProcess, out IntPtr vPointer)
        {
            //Get total count of the icons on the desktop
            vItemCount = (int)User32.SendMessage(vHandle, User32.LVM_GETITEMCOUNT, 0, 0);

            uint vProcessId;
            User32.GetWindowThreadProcessId(vHandle, out vProcessId);

            vProcess = Kernel32.OpenProcess(Kernel32.PROCESS_VM_OPERATION | Kernel32.PROCESS_VM_READ |
                Kernel32.PROCESS_VM_WRITE, false, vProcessId);
            vPointer = Kernel32.VirtualAllocEx(vProcess, IntPtr.Zero, 4096, Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT, Kernel32.PAGE_READWRITE);
        }
    }
}
