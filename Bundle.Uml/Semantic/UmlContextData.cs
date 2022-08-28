using Bundle.Uml.Elements;
using Language.Api.Semantic;

namespace Bundle.Uml.Semantic;

internal class UmlContextData
{
    private readonly Dictionary<string, UmlFigureSemanticElement> _figures;
    private readonly HashSet<UmlFigureSemanticElement> _elementsToDelete;

    public UmlContextData()
    {
        _figures = new Dictionary<String, UmlFigureSemanticElement>();
        _elementsToDelete = new HashSet<UmlFigureSemanticElement>();
    }

    public UmlFigureSemanticElement? Register(UmlSyntaxElement uml, UmlFigure figure)
    {
        UmlFigureSemanticElement? result = null;
        UmlFigureSemanticElement? semanticElementByText = null;
        if (figure.Text != null)
        {
            if (!_figures.TryGetValue(figure.Text, out semanticElementByText))
            {
                semanticElementByText = new UmlFigureSemanticElement(figure.Text);
                _figures.Add(figure.Text, semanticElementByText);
                result = semanticElementByText;
            }

            semanticElementByText.Register(uml, figure);
        }

        if (figure.Alias != null)
        {
            if (_figures.TryGetValue(figure.Alias, out var semanticElementForAlias))
            {
                semanticElementForAlias.RegisterAlias(figure.Text, figure.Alias);

                if (semanticElementByText != null)
                {
                    foreach (var usage in semanticElementByText.Usages)
                        semanticElementForAlias.Register(usage.SyntaxElement, usage.Figure);

                    _elementsToDelete.Add(semanticElementByText);
                    _figures[figure.Text] = semanticElementForAlias;
                }
                else
                    semanticElementForAlias.Register(uml, figure);
            }
            else if (semanticElementByText != null)
            {
                semanticElementByText.RegisterAlias(figure.Text, figure.Alias);
                _figures.Add(figure.Alias, semanticElementByText);
            }
        }

        return result;
    }

    public bool IsDeleted(ISemanticElement semanticElement)
    {
        return _elementsToDelete.Contains(semanticElement);
    }

    public UmlFigureSemanticElement GetSemanticElement(UmlFigure figure)
    {
        return _figures[figure.Text ?? figure.Alias];
    }
}