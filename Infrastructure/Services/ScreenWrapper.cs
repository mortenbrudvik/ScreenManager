using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ApplicationCore.Interfaces;
using ApplicationCore.ValueObjects;
using Infrastructure.Extensions;
using PInvoke;

namespace Infrastructure.Services
{
    internal class ScreenWrapper : IScreen
    {
        private readonly Screen _screen;

        public ScreenWrapper(Screen screen) => _screen = screen;

        public string Name => _screen.DeviceName;
        public Display WorkingArea => _screen.WorkingArea.ToDisplay();
        public Display Bounds => _screen.Bounds.ToDisplay();
        public bool IsPrimary => _screen.Primary;
        public Resolution Resolution => new Resolution(_screen.Bounds.Width, _screen.Bounds.Height);

        public IReadOnlyCollection<Resolution> GetResolutions() 
        {
            var resolutions = new HashSet<Resolution>();
            var devMode = new DEVMODE { dmSize = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };
            uint index = 0;

            while (User32.EnumDisplaySettingsEx(Name, index++, ref devMode, 0))
            {
                resolutions.Add(new Resolution(devMode.dmPelsWidth, devMode.dmPelsHeight));
            }

            return resolutions;
        }

        public override string ToString() => "[Bounds=" + Bounds + " WorkingArea=" + WorkingArea + " IsPrimary=" + IsPrimary + " Name=" + Name;
    }
}