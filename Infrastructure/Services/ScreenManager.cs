using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ApplicationCore.Events;
using ApplicationCore.Interfaces;
using Microsoft.Win32;

namespace Infrastructure.Services
{
    public class ScreenManager : IScreenManager, IDisposable
    {
        public event EventHandler<ScreenEventArgs> Changed;

        public ScreenManager()
        {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        public IReadOnlyCollection<IScreen> GetAll() => Screen.AllScreens.
            Select(screen => new ScreenWrapper(screen)).
            Cast<IScreen>().ToList();

        public IScreen GetPrimary()
        {
            return GetAll().SingleOrDefault(screen => screen.IsPrimary);
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            Changed?.Invoke(this, new ScreenEventArgs(GetAll()));
        }

        public void Dispose()
        {
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }
    }
}