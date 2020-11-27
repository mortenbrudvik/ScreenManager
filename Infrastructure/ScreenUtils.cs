using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ApplicationCore.ValueObjects;
using Ardalis.GuardClauses;
using PInvoke;

namespace Infrastructure
{
    public class ScreenUtils
    {
        public static IReadOnlyCollection<Resolution> GetResolutions(string deviceName)
        {
            var resolutions = new HashSet<Resolution>();
            var devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };
            uint index = 0;

            while (User32.EnumDisplaySettingsEx(deviceName, index++, ref devMode, 0))
            {
                resolutions.Add(new Resolution(devMode.dmPelsWidth, devMode.dmPelsHeight));
            }

            return resolutions;
        }

        public static bool ChangeResolution(string deviceName, Resolution newResolution)
        {
            Guard.Against.NullOrEmpty(deviceName, nameof(deviceName));
           
            if (!TryGetCurrentMode(deviceName, out var devMode))
                return false;

            devMode.dmPelsWidth = newResolution.Width;
            devMode.dmPelsHeight = newResolution.Height;
            var result = User32Ex.ChangeDisplaySettingsEx(deviceName, ref devMode, IntPtr.Zero, 0, IntPtr.Zero); 

            return result == User32Ex.DISP_CHANGE_SUCCESSFUL;
        }

        private static bool TryGetCurrentMode(string deviceName, out DEVMODE devMode)
        {
            devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };
            return User32Ex.EnumDisplaySettings(deviceName, User32Ex.ENUM_CURRENT_SETTINGS, ref devMode);
        }
    }
}