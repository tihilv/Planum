using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Language.Api;
using Language.Api.Operations;
using Language.Api.Semantic;
using Planum.Ui.Controls;

namespace Planum.Ui.ViewModels;

public class PropertiesViewModel: INotifyPropertyChanged
{
    private readonly IDocumentModel _documentModel;
    private readonly IOperationProcessor _operationProcessor;
    
    public List<ConfigurablePropertyMetadata> PropertyMetadata { get; private set; }

    public PropertiesViewModel(IDocumentModel documentModel, IOperationProcessor operationProcessor)
    {
        _documentModel = documentModel;
        _operationProcessor = operationProcessor;
        PropertyMetadata = new List<ConfigurablePropertyMetadata>();
    }

    public void RefreshSelection(ICollection<ISemanticElement> elements)
    {
        PropertyMetadata = new List<ConfigurablePropertyMetadata>();

        var operations = _operationProcessor.GetOperationsToExecute(elements).OfType<IPropertyOperation>();
        foreach (IPropertyOperation operation in operations)
        {
            PropertyMetadata.Add(new ConfigurablePropertyMetadata(_documentModel, operation, elements));
        }

        OnPropertyChanged(nameof(PropertyMetadata));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}