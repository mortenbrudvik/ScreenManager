using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Events
{
    public class ScreenEventArgs : EventArgs
    {
        public IReadOnlyCollection<IScreen> Screens { get; }

        public ScreenEventArgs(IReadOnlyCollection<IScreen> screens)
        {
            Screens = screens;
        }
        
    }
}