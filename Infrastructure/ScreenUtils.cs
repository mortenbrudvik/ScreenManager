using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using PInvoke;

namespace Infrastructure
{
    public class ScreenUtils
    {
        public static ChangeResult ChangeResolution(string deviceName, out Size oldResolution, Size newResolution, bool savesToRegistory = true)
        {
            oldResolution = Size.Empty;

            if (String.IsNullOrEmpty(deviceName))
                return ChangeResult.Failed;

            DEVMODE devMode;
            if (!TryGetCurrentMode(deviceName, out devMode))
                return ChangeResult.Failed;

            return SetResolution(deviceName, devMode, out oldResolution, newResolution, savesToRegistory);
        }

        public enum ChangeResult
        {
            Success,
            RestartRequired,
            Failed
        }
        
        public enum DMDO
        {
            DEFAULT = 0,
            D90 = 1,
            D180 = 2,
            D270 = 3
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // CCHDEVICENAME is defined to be 32.
            public string dmDeviceName;

            public ushort dmSpecVersion;
            public ushort dmDriverVersion;
            public ushort dmSize;
            public ushort dmDriverExtra;
            public uint dmFields;
            public POINT dmPosition;
            public DMDO dmDisplayOrientation;
            public uint dmDisplayFixedOutput;
            public ushort dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // CCHFORMNAME is defined to be 32.
            public string dmFormName;

            public ushort dmLogPixels;
            public uint dmBitsPerPel;
            public uint dmPelsWidth;
            public uint dmPelsHeight;
            public uint dmDisplayFlags;
            public uint dmDisplayFrequency;
            public uint dmICMMethod;
            public uint dmICMIntent;
            public uint dmMediaType;
            public uint dmDitherType;
            public uint dmReserved1;
            public uint dmReserved2;
            public uint dmPanningWidth;
            public uint dmPanningHeight;
        }
        public const uint CDS_UPDATEREGISTRY = 0x00000001;
        
        public const int ENUM_CURRENT_SETTINGS = -1; 

        public const int DISP_CHANGE_SUCCESSFUL = 0;   
        public const int DISP_CHANGE_RESTART = 1;      


        [DllImport("User32.dll", EntryPoint = "ChangeDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ChangeDisplaySettingsEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszDeviceName,
            IntPtr lpDevMode,
            IntPtr hwnd,
            uint dwflags,
            IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "ChangeDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ChangeDisplaySettingsEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszDeviceName,
            [In]
            ref DEVMODE lpDevMode,
            IntPtr hwnd,
            uint dwflags,
            IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "EnumDisplaySettingsW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplaySettings(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszDeviceName,
            int iModeNum,
            ref DEVMODE lpDevMode);

        private static bool TryGetCurrentMode(string deviceName, out DEVMODE devMode)
        {
            devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };

            return EnumDisplaySettings(
                deviceName,
                ENUM_CURRENT_SETTINGS,
                ref devMode);
        }

        private static ChangeResult SetResolution(string deviceName, DEVMODE devMode, out Size oldResolution, Size newResolution, bool savesToRegistry)
        {
            oldResolution = new Size(devMode.dmPelsWidth, devMode.dmPelsHeight);

            int result;

            if (newResolution != Size.Empty)
            {
                // Change the resolution.
                devMode.dmPelsWidth = (uint)newResolution.Width;
                devMode.dmPelsHeight = (uint)newResolution.Height;

                result = ChangeDisplaySettingsEx(
                    deviceName,
                    ref devMode,
                    IntPtr.Zero,
                    (savesToRegistry ? CDS_UPDATEREGISTRY : 0),
                    IntPtr.Zero);
            }
            else
            {
                // Revert the resolution.
                result = ChangeDisplaySettingsEx(
                    deviceName,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    0,
                    IntPtr.Zero);
            }

            Debug.WriteLine(result);

            switch (result)
            {
                case DISP_CHANGE_SUCCESSFUL:
                    return ChangeResult.Success;

                case DISP_CHANGE_RESTART:
                    return ChangeResult.RestartRequired;

                default:
                    return ChangeResult.Failed;
            }
        }
    }
}