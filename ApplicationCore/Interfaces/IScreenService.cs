using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface IScreenService
    {
        event EventHandler Changed;
     
        IReadOnlyCollection<IScreen> GetAll();
    }
}