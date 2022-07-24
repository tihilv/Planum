using Engine.PlantUml;
using Language.Api;
using Visualize.Api;

namespace SvgVisualizer;

public class DocumentToImageSvgPipelineFactory: IDocumentToImagePipelineFactory
{
    public IDocumentToImagePipeline Create(IDocumentModel documentModel, IVectorImage vectorImage)
    {
        return new DocumentToImageSvgPipeline(documentModel, vectorImage, new PlantUmlEngine());
    }
}