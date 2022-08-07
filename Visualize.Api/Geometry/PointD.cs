using System.Drawing;

namespace Visualize.Api.Geometry;

public readonly struct PointD
{
    public static readonly PointD Empty = new PointD(0, 0);
    
    public readonly double X;
    public readonly double Y;

    public PointD(Double x, Double y)
    {
        X = x;
        Y = y;
    }

    public override String ToString()
    {
        return $"({X}, {Y})";
    }

    public PointD Subtract(PointD other)
    {
        return new PointD(X - other.X, Y - other.Y);
    }

    public PointD Multiply(double mult)
    {
        return new PointD(X * mult, Y * mult);
    }

    public PointD Add(PointD other)
    {
        return new PointD(X + other.X, Y + other.Y);
    }

    public PointF Add(PointF other)
    {
        return new PointF((float)(X + other.X), (float)(Y + other.Y));
    }

    public PointF ToPointF()
    {
        return new PointF((float)X, (float)Y);
    }

    public PointD MinCombined(PointD other)
    {
        return new PointD(Math.Min(X, other.X), Math.Min(Y, other.Y));
    }

    public PointD MaxCombined(PointD other)
    {
        return new PointD(Math.Max(X, other.X), Math.Max(Y, other.Y));
    }
}