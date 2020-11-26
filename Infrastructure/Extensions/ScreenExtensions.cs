using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using ApplicationCore.Interfaces;
using ApplicationCore.ValueObjects;
using PInvoke;

namespace Infrastructure.Extensions
{
    public static class ScreenExtensions
    {
        //public static IEnumerable<Resolution> GetAllResolutions(this IScreen screen)
        //{
        //    var scope = new ManagementScope();

        //    var query = new ObjectQuery("SELECT * FROM CIM_VideoControllerResolution");

        //    using var searcher = new ManagementObjectSearcher(scope, query);
        //    var results = searcher.Get();

        //    foreach (var result in results)
        //    {
        //        var width = (uint)result["HorizontalResolution"];
        //        var height = (uint)result["VerticalResolution"];

        //        yield return new Resolution(width, height);
        //    }
        //}

        //public const uint EDS_RAWMODE = 0x00000002;

        [DllImport("User32.dll", EntryPoint = "EnumDisplaySettingsExW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplaySettingsEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszDeviceName,
            int iModeNum,
            ref DEVMODE lpDevMode,
            uint dwFlags);


        public static IEnumerable<Resolution> GetAvailableResolutions(this IScreen screen) 
        {
            var devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };

            var index = 0;
            while (EnumDisplaySettingsEx(screen.Name, index++, ref devMode, 0))
            {
                var width = devMode.dmPelsWidth;
                var height = devMode.dmPelsHeight;

                yield return new Resolution(width, height);
            }
        }
    }
}