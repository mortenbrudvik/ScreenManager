using System.Collections.Generic;
using ApplicationCore.ValueObjects;

namespace ApplicationCore.Interfaces
{
    public interface IScreen
    {
        string Name { get; }
        Display WorkingArea { get; }
        Display Bounds { get; }
        bool IsPrimary { get; }
        IReadOnlyCollection<Resolution> Resolutions { get; }
    }
}