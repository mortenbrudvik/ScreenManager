using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ApplicationCore.Interfaces;
using Microsoft.Win32;

namespace Infrastructure.Services
{
    public class ScreenService : IScreenService
    {
        public event EventHandler Changed;

        public ScreenService()
        {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        public IReadOnlyCollection<IScreen> GetAll() => Screen.AllScreens.
            Select(screen => new ScreenWrapper(screen)).
            Cast<IScreen>().ToList();

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        ~ScreenService()
        {
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }
    }
}