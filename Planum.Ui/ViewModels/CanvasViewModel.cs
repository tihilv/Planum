using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Bundle.Uml;
using Language.Api.Semantic;
using Language.Common;
using Language.Processing;
using Planum.Ui.Utils;
using SvgVisualizer;
using VectorDrawing;
using Visualize.Api;
using Visualize.Api.Primitives;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Point = Avalonia.Point;

namespace Planum.Ui.ViewModels;

public class CanvasViewModel: INotifyPropertyChanged, IDisposable
{
    public IInteractiveCanvas Canvas { get; }

    private int _width;
    private int _height;

    private readonly IDocumentToImagePipeline _pipeline;
    private readonly IVectorImage _hoverVectorImage;
    
    private System.Drawing.Bitmap? _bitmap;

    public Bitmap? Bitmap
    {
        get
        {
            if (_bitmap != null)
                _bitmap.Dispose();

            if (_width > 0 && _height > 0)
            {
                _bitmap = new System.Drawing.Bitmap(_width, _height);
                using (var g = Graphics.FromImage(_bitmap))
                    Canvas.Refresh(g);
            }

            return _bitmap?.ConvertToAvaloniaBitmap();
        }
    }

    public CanvasViewModel()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var documentModel = new DocumentModel(new TextScript(GetSimpleScript()), builder);
        VectorImage vectorImage = new VectorImage();
        _hoverVectorImage = new VectorImage(new HightlightTransformer());
        _pipeline = new DocumentToImageSvgPipelineFactory().Create(documentModel, vectorImage);
        _pipeline.Changed += (sender, args) => OnBitmapChanged();

        Canvas = new InteractiveCanvas(new IVectorImage[] { vectorImage, _hoverVectorImage });
    }

    private static List<String> GetSimpleScript()
    {
        var sb = new List<String>();
        sb.Add("@startuml");
        sb.Add("left to right direction");
        sb.Add("actor \"Food Critic\" as fc");
        sb.Add("rectangle Restaurant {");
        sb.Add("    usecase \"Eat Food\" as UC1");
        sb.Add("    usecase \"Pay for Food\" as UC2");
        sb.Add("    usecase \"Drink\" as UC3");
        sb.Add("}");
        sb.Add("fc --> UC1");
        sb.Add("fc --> UC2");
        sb.Add("fc --> UC3");
        sb.Add("@enduml");
        return sb;
    }
    
    public void Resize(double width, double height)
    {
        Canvas.Resize(new RectangleF(0, 0, (float)width, (float)height));
        _width = (int)width;
        _height = (int)height;
        OnBitmapChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnBitmapChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bitmap)));
    }

    public void Zoom(Point position, Double delta)
    {
        Canvas.Zoom(new PointF((float)position.X, (float)position.Y), (float)delta);
        OnBitmapChanged();
    }

    public void ZoomToExtents()
    {
        Canvas.ZoomToExtents();
        OnBitmapChanged();
    }

    public void Move(Point shift)
    {
        Canvas.Move(new PointF((float)shift.X, (float)shift.Y));
        OnBitmapChanged();
    }

    private ISemanticElement? _lastSelection;
    public void SetCursor(Point newPosition)
    {
        var modelPoint = Canvas.ToModel(new PointF((float)newPosition.X, (float)newPosition.Y));
        var selection = _pipeline.Select(modelPoint);
        if (selection != _lastSelection)
        {
            _lastSelection = selection;
            if (selection != null)
            {
                _hoverVectorImage.Clear();
                _pipeline.DrawToImage(selection, _hoverVectorImage);
            }
            else
                _hoverVectorImage.Clear();
            
            OnBitmapChanged();
        }
    }

    public void Dispose()
    {
        _bitmap?.Dispose();
        _pipeline.Dispose();
    }
}

public class HightlightTransformer : IColorTransformer
{
    public Pen GetPen(IVectorPrimitive primitive)
    {
        return new Pen(Color.Aqua);
    }

    public Brush GetFillBrush(IVectorPrimitive primitive)
    {
        return new HatchBrush(HatchStyle.Percent10, Color.Aqua, primitive.BackColor);
    }

    public Brush GetBrush(IVectorPrimitive primitive)
    {
        return new HatchBrush(HatchStyle.Percent50, Color.Aqua, primitive.ForeColor);
    }
}