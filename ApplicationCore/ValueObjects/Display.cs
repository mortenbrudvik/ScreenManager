using System;

namespace ApplicationCore.ValueObjects
{
    public struct Display : IEquatable<Display>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Display(int x, int y, int width, int height) => (X, Y, Width, Height) = (x, y, width, height);

        public override readonly bool Equals(object? obj) => obj is Display && Equals((Display)obj);
        public readonly bool Equals(Display other) => this == other;

        public static bool operator ==(Display left, Display right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
        public static bool operator !=(Display left, Display right) => !(left == right);

        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        public override readonly string ToString() => "{Width=" + X + ",Height=" + Y + ",Width=" + Width + ",Height=" + Height + "}";
    }
}