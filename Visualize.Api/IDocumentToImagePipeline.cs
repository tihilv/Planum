using Language.Api.Semantic;
using Visualize.Api.Geometry;

namespace Visualize.Api;

public interface IDocumentToImagePipeline: IDisposable
{
    event EventHandler<EventArgs> Changed;
    ISemanticElement? Select(PointD modelPoint);
    void DrawToImage(ISemanticElement element, IVectorImage image);
}