using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Language.Api.Semantic;
using Language.Processing;
using Planum.Ui.Utils;
using SvgVisualizer;
using VectorDrawing;
using Visualize.Api;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Point = Avalonia.Point;

namespace Planum.Ui.ViewModels;

public class CanvasViewModel: INotifyPropertyChanged, IDisposable
{
    public IInteractiveCanvas Canvas { get; }
    
    public List<ISemanticElement> SelectedElements { get; }

    private int _width;
    private int _height;

    private readonly IDocumentToImagePipeline _pipeline;
    private readonly IVectorImage _hoverVectorImage;
    private readonly IVectorImage _selectVectorImage;
    
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

    public CanvasViewModel(DocumentModel documentModel)
    {
        VectorImage vectorImage = new VectorImage();
        _hoverVectorImage = new VectorImage(new HighlightTransformer(Color.Aqua));
        _selectVectorImage = new VectorImage(new HighlightTransformer(Color.Turquoise));
        _pipeline = new DocumentToImageSvgPipelineFactory().Create(documentModel, vectorImage);
        _pipeline.Changed += (sender, args) => OnBitmapChanged();

        Canvas = new InteractiveCanvas(new IVectorImage[] { vectorImage, _hoverVectorImage, _selectVectorImage });
        SelectedElements = new List<ISemanticElement>();
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

    protected virtual void OnSelectionChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedElements)));
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

    public void SelectByPoint(Point position)
    {
        var modelPoint = Canvas.ToModel(new PointF((float)position.X, (float)position.Y));
        var selection = _pipeline.Select(modelPoint);
        bool changed = SelectedElements.Any() || selection != null;
        if (changed)
        {
            SelectedElements.Clear();
            if (selection != null)
                SelectedElements.Add(selection);
            
            OnSelectionChanged();
        }

        if (changed)
        {
            _selectVectorImage.Clear();
            foreach (var selectedElement in SelectedElements)
                _pipeline.DrawToImage(selectedElement, _selectVectorImage);
            
            OnBitmapChanged();
        }
    }

    public void Dispose()
    {
        _bitmap?.Dispose();
        _pipeline.Dispose();
    }
}