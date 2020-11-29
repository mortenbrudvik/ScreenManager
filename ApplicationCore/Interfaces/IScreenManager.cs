using System;
using System.Collections.Generic;
using ApplicationCore.Events;

namespace ApplicationCore.Interfaces
{
    public interface IScreenManager
    {
        event EventHandler<ScreenEventArgs> Changed;
     
        IReadOnlyCollection<IScreen> GetAll();
        IScreen GetPrimary();
    }
}