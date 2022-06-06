using Language.Api.Operations;
using Language.Common.Primitives;

namespace Language.Common.Operations;

public static class DefaultOperationDefinitions
{
    public static readonly OperationDefinition ChangeText = new OperationDefinition("TEXT.CHANGE", "Text", new ParameterDefinition(typeof(string), "New text"));
    public static readonly OperationDefinition ChangeAlias = new OperationDefinition("ALIAS.CHANGE", "Id", new ParameterDefinition(typeof(string), "New id"));
    public static readonly OperationDefinition ChangeArrow = new OperationDefinition("ARROW.CHANGE", "Arrow", new ParameterDefinition(typeof(Arrow), "New arrow"));
    public static readonly OperationDefinition Ungroup = new OperationDefinition("GROUP.UNGROP", "Ungroup");
    public static readonly OperationDefinition AddToGroup = new OperationDefinition("GROUP.ADDTO", "Add to group");
    public static readonly OperationDefinition ExcludeFromGroup = new OperationDefinition("GROUP.EXCLUDEFROM", "Exclude from group");
    public static readonly OperationDefinition MoveElementNext = new OperationDefinition("NEXT.MOVE", "Move element next");
    public static readonly OperationDefinition MoveElementPrev = new OperationDefinition("PREV.MOVE", "Move element back");
}