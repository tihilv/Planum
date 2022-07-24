namespace Visualize.Api.Primitives;

public class LinkPrimitive: CompositePrimitiveBase
{
    private readonly string _url;

    public String Url => _url;

    public LinkPrimitive(String url)
    {
        _url = url;
    }
}