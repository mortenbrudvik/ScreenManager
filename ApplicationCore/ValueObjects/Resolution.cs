using System;

namespace ApplicationCore.ValueObjects
{
    public readonly struct Resolution : IEquatable<Resolution>, IComparable<Resolution>
    {
        public uint Width { get; }
        public uint Height { get; }

        public Resolution(uint width, uint height) => (Width, Height) = (width, height); 
        public Resolution(int width, int height) => (Width, Height) = ((uint)width, (uint)height); 

        public static bool operator ==(Resolution r1, Resolution r2) => r1.Width == r2.Width && r1.Height == r2.Height;
        public static bool operator !=(Resolution r1, Resolution r2) => !(r1 == r2);

        public override int GetHashCode() => HashCode.Combine(Width, Height);

        public override bool Equals(object? obj) => obj is Resolution resolution && Equals(resolution);
        public bool Equals(Resolution other) => this == other;
        
        public override string ToString() => "{Width=" + Width + ", Height=" + Height + "}";

        public void Deconstruct(out double width, out double height) =>  (width, height) = (Width, Height);

        public int CompareTo(Resolution other)
        {
            var widthComparison = Width.CompareTo(other.Width);
            return widthComparison != 0 ? widthComparison : Height.CompareTo(other.Height);
        }
    }
}