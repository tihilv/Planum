using Bundle.Uml.Elements;
using Language.Api.Operations;
using Language.Api.Syntax;
using Language.Common.Operations;

namespace Bundle.Uml.Operations;

public class CreateUmlContainerOperation: CreateContainerOperationBase
{
    public static readonly OperationDefinition OperationDefinition = new OperationDefinition("UMLCONTAINER.CREATE", "Create Uml Group", new ParameterDefinition(typeof(string), "Name", true));
    public override OperationDefinition Definition => OperationDefinition;

    public static readonly IOperation Instance = new CreateUmlContainerOperation();

    private CreateUmlContainerOperation()
    {}

    protected override CompositeSyntaxElement CreateContainer(Object[] arguments)
    {
        var name = arguments.FirstOrDefault()?.ToString()??nameof(UmlContainerType.Rectangle);
        return new UmlContainerSyntaxElement(UmlContainerType.Rectangle, name);
    }
}