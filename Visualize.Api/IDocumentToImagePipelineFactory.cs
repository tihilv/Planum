using Language.Api;

namespace Visualize.Api;

public interface IDocumentToImagePipelineFactory
{
    IDocumentToImagePipeline Create(IDocumentModel documentModel, IVectorImage vectorImage);
}