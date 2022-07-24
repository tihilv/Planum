using System.Drawing;

namespace Visualize.Api.Geometry;

public readonly struct Point
{
    public static readonly Point Empty = new Point(0, 0);
    
    public readonly double X;
    public readonly double Y;

    public Point(Double x, Double y)
    {
        X = x;
        Y = y;
    }

    public override String ToString()
    {
        return $"({X}, {Y})";
    }

    public Point Subtract(Point other)
    {
        return new Point(X - other.X, Y - other.Y);
    }

    public Point Multiply(double mult)
    {
        return new Point(X * mult, Y * mult);
    }

    public Point Add(Point other)
    {
        return new Point(X + other.X, Y + other.Y);
    }

    public PointF ToPointF()
    {
        return new PointF((float)X, (float)Y);
    }

    public Point MinCombined(Point other)
    {
        return new Point(Math.Min(X, other.X), Math.Min(Y, other.Y));
    }

    public Point MaxCombined(Point other)
    {
        return new Point(Math.Max(X, other.X), Math.Max(Y, other.Y));
    }
}