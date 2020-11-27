using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ApplicationCore.Exceptions;
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
            try
            {
                return ScreenUtils.GetResolutions(Name);
            }
            catch (Exception e)
            {
                throw new ScreenException($"An error occurred when retrieving resolutions for screen \"{Name}\"", e);
            }
        }

        public bool ChangeResolution(Resolution newResolution)
        {
            try
            {
                return ScreenUtils.ChangeResolution(Name, newResolution);
            }
            catch (Exception e)
            {
                throw new ScreenException($"An error occurred when trying to change screen resolution for screen \"{Name}\"", e);
            }
        }
        public override string ToString() => "[Bounds=" + Bounds + " WorkingArea=" + WorkingArea + " IsPrimary=" + IsPrimary + " Name=" + Name;
    }
}