using System.Drawing;
using ApplicationCore.ValueObjects;

namespace Infrastructure.Extensions
{
    internal static class RectangleExtensions
    {
        public static Display ToDisplay(this Rectangle rect)
        {
            return new Display(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}