namespace Language.Common.Operations;

public interface ITexted
{
    public string Text { get; set; }
}

public interface IAliased
{
    public string? Alias { get; set; }
}