namespace Visualize.Api;

public interface IDocumentToImagePipeline: IDisposable
{
    event EventHandler<EventArgs> Changed;
}