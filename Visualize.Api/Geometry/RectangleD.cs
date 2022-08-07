namespace Visualize.Api.Geometry;

public readonly struct RectangleD
{
    public static readonly RectangleD Empty = new RectangleD();
    
    public readonly PointD Center;
    public readonly double Width;
    public readonly double Height;

    public RectangleD(PointD center, Double width, Double height)
    {
        Center = center;
        Width = width;
        Height = height;
    }

    public RectangleD(Double x, Double y, Double width, Double height) : this(new PointD(x + width / 2.0, y + height / 2.0), width, height)
    {
        
    }

    public PointD TopLeft => new PointD(Center.X - Width / 2.0, Center.Y - Height / 2.0);
    public PointD BottomRight => new PointD(Center.X + Width / 2.0, Center.Y + Height / 2.0);
    public bool IsEmpty => Width == 0 && Height == 0;
    public Double Square => Width * Height;

    public RectangleD Union(RectangleD other)
    {
        var topLeft = TopLeft.MinCombined(other.TopLeft);
        var bottomRight = BottomRight.MaxCombined(other.BottomRight);

        return new RectangleD(topLeft.X, topLeft.Y, (bottomRight.X - topLeft.X), (bottomRight.Y - topLeft.Y));
    }

    public RectangleD Add(PointD point)
    {
        return new RectangleD(Center.Add(point), Width, Height);
    }

    public bool Contains(PointD modelPoint)
    {
        var topLeft = TopLeft;
        var bottomRight = BottomRight;
        return
            topLeft.X <= modelPoint.X && modelPoint.X <= bottomRight.X &&
            topLeft.Y <= modelPoint.Y && modelPoint.Y <= bottomRight.Y;
    }
}