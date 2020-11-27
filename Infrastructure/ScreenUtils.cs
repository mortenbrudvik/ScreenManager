using System;
using System.Runtime.InteropServices;
using System.Windows;
using PInvoke;

namespace Infrastructure
{
    public class ScreenUtils
    {
        public static ChangeResult ChangeResolution(string deviceName, out Size oldResolution, Size newResolution)
        {
            oldResolution = Size.Empty;

            if (string.IsNullOrEmpty(deviceName))
                return ChangeResult.Failed;

            if (!TryGetCurrentMode(deviceName, out var devMode))
                return ChangeResult.Failed;
            oldResolution = new Size(devMode.dmPelsWidth, devMode.dmPelsHeight);

            return SetResolution(deviceName, devMode, newResolution);
        }

        public enum ChangeResult
        {
            Success, RestartRequired, Failed
        }

        public const int ENUM_CURRENT_SETTINGS = -1; 

        public const int DISP_CHANGE_SUCCESSFUL = 0;   
        public const int DISP_CHANGE_RESTART = 1;      


        [DllImport("User32.dll", EntryPoint = "ChangeDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ChangeDisplaySettingsEx([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "ChangeDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ChangeDisplaySettingsEx([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, [In] ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "EnumDisplaySettingsW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplaySettings([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        private static bool TryGetCurrentMode(string deviceName, out DEVMODE devMode)
        {
            devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };

            return EnumDisplaySettings(deviceName, ENUM_CURRENT_SETTINGS, ref devMode);
        }

        private static ChangeResult SetResolution(string deviceName, DEVMODE devMode, Size newResolution)
        {
            int result;

            if (newResolution != Size.Empty)
            {
                // Change the resolution.
                devMode.dmPelsWidth = (uint)newResolution.Width;
                devMode.dmPelsHeight = (uint)newResolution.Height;
                result = ChangeDisplaySettingsEx(deviceName, ref devMode, IntPtr.Zero, 0, IntPtr.Zero); // 1 instead of 0 to save to registry
            }
            else
            {
                // Revert the resolution.
                result = ChangeDisplaySettingsEx(deviceName, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero); 
            }

            return result switch
            {
                DISP_CHANGE_SUCCESSFUL => ChangeResult.Success,
                DISP_CHANGE_RESTART => ChangeResult.RestartRequired,
                _ => ChangeResult.Failed
            };
        }
    }
}