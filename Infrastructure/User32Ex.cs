using System;
using System.Runtime.InteropServices;
using PInvoke;

namespace Infrastructure
{
    public class User32Ex
    {
        public const int ENUM_CURRENT_SETTINGS = -1; 
        public const int DISP_CHANGE_SUCCESSFUL = 0;   

        [DllImport("User32.dll", EntryPoint = "ChangeDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ChangeDisplaySettingsEx([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, [In] ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "EnumDisplaySettingsW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplaySettings([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
    }
}