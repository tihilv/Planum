using System.Drawing;
using System.Reactive.Subjects;
using Engine.Api;
using Language.Api;
using Language.Api.History;
using Language.Api.Semantic;
using Language.Api.Utils;
using Language.Common.Semantic;
using Svg;
using Svg.Transforms;

namespace SvgVisualizer;

public class DrawingModel
{
    private readonly IDocumentModel _documentModel;
    private readonly IEngine _engine;
    
    private SvgDocument? _document;
    private Dictionary<string, ISemanticElement> _urlsToSemantic;

    private Task _currentTask;
    private CancellationTokenSource? _currentCts;

    private readonly BehaviorSubject<SvgDocument?> _documentSubject;

    public IObservable<SvgDocument?> Document => _documentSubject;

    public DrawingModel(IDocumentModel documentModel, IEngine engine)
    {
        _documentModel = documentModel;
        _engine = engine;

        _documentSubject = new BehaviorSubject<SvgDocument?>(null);
        _documentModel.Changed += DocumentModelChanged;
        
        UpdateModel();
    }

    private void DocumentModelChanged(Object? sender, EventArgs e)
    {
        UpdateModel();
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
                        _document = SvgDocument.FromSvg<SvgDocument>(t.Result);
                        _documentSubject.OnNext(_document);
                        return _document;
                    });
        }

        if (undoManager.CanUndo())
            undoManager.Undo();
    }

    public Bitmap? CreateBitmap()
    {
        _document.Transforms = new SvgTransformCollection();
        _document.AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin);
        _document.Width = 500;
        _document.Height = 500;
        return _document?.Draw();
    }
}