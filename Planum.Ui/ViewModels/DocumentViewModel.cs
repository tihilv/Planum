using System;
using System.ComponentModel;
using Language.Api;
using Language.Api.Operations;
using Language.Common;
using Language.Common.Operations;
using Language.Processing;

namespace Planum.Ui.ViewModels;

public class DocumentViewModel : ViewModelBase
{
    private readonly IBundleBuilder _builder;
    private readonly DocumentModel _documentModel;
    private readonly IOperationProcessor _operationProcessor;
    
    public CanvasViewModel Canvas { get; }
    public PropertiesViewModel Selection { get; }

    public DocumentViewModel(IScript script, DefaultBundleBuilder builder)
    {
        _builder = builder;
        _documentModel = new DocumentModel(script, builder);
        _operationProcessor = new DefaultOperationProcessor(builder);
        
        Canvas = new CanvasViewModel(_documentModel);
        Selection = new PropertiesViewModel(_documentModel, _operationProcessor);
        
        Canvas.PropertyChanged += CanvasOnPropertyChanged;
    }

    private void CanvasOnPropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CanvasViewModel.SelectedElements))
        {
            Selection.RefreshSelection(Canvas.SelectedElements);
        }
    }
}