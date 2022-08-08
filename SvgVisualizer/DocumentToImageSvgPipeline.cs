using Engine.Api;
using Language.Api;
using Language.Api.History;
using Language.Api.Semantic;
using Language.Api.Utils;
using Language.Common.Semantic;
using Svg;
using SvgVisualizer.ElementDrawers;
using Visualize.Api;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer;

public class DocumentToImageSvgPipeline : IDocumentToImagePipeline
{
    private readonly IDocumentModel _documentModel;
    private readonly IEngine _engine;
    private readonly IVectorImage _vectorImage;

    private Task _currentTask;
    private CancellationTokenSource? _currentCts;
    
    private Dictionary<string, ISemanticElement> _urlsToSemantic;
    private List<(RectangleD, string)>? _urls;
    private Dictionary<ISemanticElement, IVectorPrimitive> _elementToLinkPrimitive;

    public event EventHandler<EventArgs>? Changed;

    private static readonly Dictionary<Type, ISvgElementDrawer> _elementDrawers;
    static DocumentToImageSvgPipeline()
    {
        _elementDrawers = GetElementDrawers().ToDictionary(d => d.SvgType);   
    }

    public DocumentToImageSvgPipeline(IDocumentModel documentModel, IVectorImage vectorImage, IEngine engine)
    {
        _documentModel = documentModel;
        _vectorImage = vectorImage;
        _engine = engine;
        
        _documentModel.Changed += DocumentModelChanged;
        
        UpdateModel();
    }

    private void DocumentModelChanged(Object? sender, EventArgs e)
    {
        UpdateModel();
    }
    
    private static IEnumerable<ISvgElementDrawer> GetElementDrawers()
    {
        yield return new DocumentDrawer();
        yield return new DefinitionListDrawer();
        yield return new RectangleDrawer();
        yield return new TextDrawer();
        yield return new EllipseDrawer();
        yield return new PathDrawer();
        yield return new PolygonDrawer();
        yield return new GroupDrawer();
        yield return new LinkDrawer();
    }
    
    private void UpdateModel()
    {
        if (_currentCts != null)
            _currentCts.Cancel();
        _currentCts = new CancellationTokenSource();

        _urlsToSemantic = new Dictionary<String, ISemanticElement>();
        
        var undoManager = new UndoRedoManager();

        int index = 0;
        using (undoManager.CreateTransaction())
        {
            foreach(var element in _documentModel.SemanticModel)
                if (element is IUrlSemantic urlSemantic)
                {
                    var url = "planum://" + index++;
                    urlSemantic.Url = url;
                    _urlsToSemantic.Add(url, element);
                }

            var script = _documentModel.GetScript();
            _currentTask = Task.Run(() => script.GetAllText(), _currentCts.Token)
                .ContinueWith(t => _engine.GetPlantAsync(t.Result))
                .Unwrap().ContinueWith(t=>
                {
                    Draw(SvgDocument.FromSvg<SvgDocument>(t.Result));
                    Changed?.Invoke(this, EventArgs.Empty);
                });
        }

        if (undoManager.CanUndo())
            undoManager.Undo();
    }

    private void Draw(SvgDocument? document)
    {
        _vectorImage.Clear();
        if (document != null)
            VisualizeElement(document, primitive => _vectorImage.Add(primitive));
        ProcessLinks();
    }

    private void VisualizeElement(SvgElement element, Action<IVectorPrimitive> newPrimitiveRegisterAction)
    {
        if (_elementDrawers.TryGetValue(element.GetType(), out var drawer))
        {
            var primitive = drawer.Process(element);
            newPrimitiveRegisterAction(primitive);

            if (element.Children.Any())
            {
                if (primitive is CompositePrimitiveBase composite)
                    foreach (var item in element.Children)
                        VisualizeElement(item, p => composite.Children.Add(p));
                else
                    throw new InvalidOperationException($"Svg element '{element.GetType().FullName}' contains children, but primitive '{primitive.GetType().FullName} doesn't'");
            }
        }
        else
            throw new InvalidOperationException($"There is no drawer for Svg element '{element.GetType().FullName}'");
    }

    private void ProcessLinks()
    {
        _urls = new List<(RectangleD, String)>();
        _elementToLinkPrimitive = new Dictionary<ISemanticElement, IVectorPrimitive>();
        ProcessLinks(_vectorImage.Primitives);
    }

    private void ProcessLinks(IEnumerable<IVectorPrimitive> primitives)
    {
        foreach (var primitive in primitives)
        {
            if (primitive is LinkPrimitive link)
            {
                _urls.Add((link.GetBoundaries(), link.Url));
                _elementToLinkPrimitive.Add(_urlsToSemantic[link.Url], link);
            }

            if (primitive is CompositePrimitiveBase composite)
                ProcessLinks(composite.Children);
        }
    }

    public ISemanticElement? Select(PointD modelPoint)
    {
        double bestSquare = double.MaxValue;
        string bestUrl = string.Empty;

        if (_urls != null)
        {
            foreach (var pair in _urls)
            {
                var square = pair.Item1.Square;
                if (square < bestSquare)
                {
                    if (pair.Item1.Contains(modelPoint))
                    {
                        bestSquare = square;
                        bestUrl = pair.Item2;
                    }
                }
            }
        }

        if (bestUrl != String.Empty)
            return _urlsToSemantic[bestUrl];

        return null;
    }

    public void DrawToImage(ISemanticElement element, IVectorImage image)
    {
        image.Add(_elementToLinkPrimitive[element]);
    }
    
    public void Dispose()
    {
        _currentCts?.Cancel();
        _engine.Dispose();
        _currentTask.Dispose();
        _currentCts?.Dispose();
    }
}