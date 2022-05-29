namespace Engine.PlantUml;

internal class PlantUmlErrorReport
{
    public string Status;
    public int LineNumber;
    public string Label;
    public bool HasError => LineNumber != 0 || !string.IsNullOrEmpty(Status) || !string.IsNullOrEmpty(Label);
}