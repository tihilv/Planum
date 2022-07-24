namespace Visualize.Api.Geometry;

public readonly struct RectangleD
{
    public static readonly RectangleD Empty = new RectangleD();
    
    public readonly Point Center;
    public readonly double Width;
    public readonly double Height;

    public RectangleD(Point center, Double width, Double height)
    {
        Center = center;
        Width = width;
        Height = height;
    }

    public RectangleD(Double x, Double y, Double width, Double height) : this(new Point(x + width / 2.0, y + height / 2.0), width, height)
    {
        
    }

    public Point TopLeft => new Point(Center.X - Width / 2.0, Center.Y - Height / 2.0);
    public Point BottomRight => new Point(Center.X + Width / 2.0, Center.Y + Height / 2.0);

    public RectangleD Union(RectangleD other)
    {
        var topLeft = TopLeft.MinCombined(other.TopLeft);
        var bottomRight = BottomRight.MaxCombined(other.BottomRight);

        return new RectangleD(topLeft.X, topLeft.Y, (bottomRight.X - topLeft.X), (bottomRight.Y - topLeft.Y));
    }
}