using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface IScreenManager
    {
        event EventHandler Changed;
     
        IReadOnlyCollection<IScreen> GetAll();
        IScreen GetPrimary();
    }
}