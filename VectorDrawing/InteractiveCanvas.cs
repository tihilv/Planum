using System.Drawing;
using System.Drawing.Imaging;
using Visualize.Api;
using Visualize.Api.Geometry;

namespace VectorDrawing;

public class InteractiveCanvas: IInteractiveCanvas
{
    private RectangleD _modelRectangle;
    private RectangleF _graphicsRectangle;
    private DrawingContext _globalContext; 

    private readonly IVectorImage[] _images;
    private readonly Bitmap?[] _currentBitmaps;
    private readonly long[] _generations;

    private bool _regenAll;

    public InteractiveCanvas(IEnumerable<IVectorImage> images)
    {
        _images = images.ToArray();
        _currentBitmaps = new Bitmap[_images.Length];
        _generations = new long[_images.Length];
    }

    public void Move(PointF graphicsVector)
    {
        var modelVector = new PointD(_globalContext.ToModel(graphicsVector.X), _globalContext.ToModel(graphicsVector.Y));
        _modelRectangle = _modelRectangle.Add(modelVector);
        _regenAll = true;
    }

    public void Zoom(PointF graphicsPoint, float zoomFactor)
    {
        var modelPoint = _globalContext.ToModel(graphicsPoint);
        var topLeft = _modelRectangle.TopLeft;
        var xr = (modelPoint.X - topLeft.X) / _modelRectangle.Width;
        var yr = (modelPoint.Y - topLeft.Y) / _modelRectangle.Height;

        var widthDelta = _modelRectangle.Width * (1 - zoomFactor);
        var heightDelta = _modelRectangle.Height * (1 - zoomFactor);
        
        _modelRectangle = new RectangleD(topLeft.X-xr * widthDelta, topLeft.Y-yr * heightDelta, _modelRectangle.Width + widthDelta, _modelRectangle.Height + heightDelta);
        _regenAll = true;
    }

    public void ZoomToExtents()
    {
        if (_images.Any())
        {
            _modelRectangle = _images[0].GetBoundaries();
            foreach (var nextImage in _images.Skip(1))
                _modelRectangle = _modelRectangle.Union(nextImage.GetBoundaries());
        }

        _regenAll = true;
    }

    public void Resize(RectangleF graphicsRectangle)
    {
        _graphicsRectangle = graphicsRectangle;
        _regenAll = true;
    }
    
    public void Refresh(Graphics g)
    {
        if (!_graphicsRectangle.IsEmpty)
        {
            if (_modelRectangle.IsEmpty)
                ZoomToExtents();
            
            _globalContext = new DrawingContext(_modelRectangle, _graphicsRectangle, null);

            for (var index = 0; index < _images.Length; index++)
                ProcessImage(index, g);
        }
    }

    public PointD ToModel(PointF graphicsPoint)
    {
        return _globalContext.ToModel(graphicsPoint);
    }

    private object _refreshLock = new Object();

    private void ProcessImage(int index, Graphics g)
    {
        lock (_refreshLock)
        {
            var newGeneration = _images[index].Generation;
            if (_regenAll || _currentBitmaps[index] == null || newGeneration != _generations[index])
            {
                var imageWidth = (int)_graphicsRectangle.Width;
                var imageHeight = (int)_graphicsRectangle.Height;

                Bitmap? bmp;
                if (_currentBitmaps[index] == null || _currentBitmaps[index].Width != imageWidth || _currentBitmaps[index].Height != imageHeight)
                {
                    _currentBitmaps[index]?.Dispose();
                    bmp = new Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppArgb);
                }
                else
                {
                    bmp = _currentBitmaps[index];
                    using (var clearG = Graphics.FromImage(bmp))
                        clearG.Clear(Color.Transparent);
                }

                using (var gg = Graphics.FromImage(bmp))
                    _images[index].Rasterize(_modelRectangle, _graphicsRectangle, gg);

                _currentBitmaps[index] = bmp;
                _generations[index] = newGeneration;
            }

            g.DrawImageUnscaled(_currentBitmaps[index]!, (int)_graphicsRectangle.Left, (int)_graphicsRectangle.Top);
        }
    }
}