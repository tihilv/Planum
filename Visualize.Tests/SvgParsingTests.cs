using Microsoft.VisualStudio.TestTools.UnitTesting;
using SvgVisualizer;

namespace Visualize.Tests;

[TestClass]
public class SvgParsingTests
{
    [TestMethod]
    public void SimpleSvgParsingText()
    {
        var svgModel = new SvgModel();
        var svg = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" contentStyleType=\"text/css\" height=\"94px\" preserveAspectRatio=\"none\" style=\"width:144px;height:94px;background:#FFFFFF;\" version=\"1.1\" viewBox=\"0 0 144 94\" width=\"144px\" zoomAndPan=\"magnify\"><defs/><g><line style=\"stroke:#181818;stroke-width:0.5;stroke-dasharray:5.0,5.0;\" x1=\"71\" x2=\"71\" y1=\"37.6094\" y2=\"57.6094\"/><rect fill=\"#E2E2F0\" height=\"31.6094\" rx=\"2.5\" ry=\"2.5\" style=\"stroke:#181818;stroke-width:0.5;\" width=\"133\" x=\"5\" y=\"5\"/><text fill=\"#000000\" font-family=\"sans-serif\" font-size=\"14\" lengthAdjust=\"spacing\" textLength=\"119\" x=\"12\" y=\"26.5332\">Not so famous Bob</text><rect fill=\"#E2E2F0\" height=\"31.6094\" rx=\"2.5\" ry=\"2.5\" style=\"stroke:#181818;stroke-width:0.5;\" width=\"133\" x=\"5\" y=\"56.6094\"/><text fill=\"#000000\" font-family=\"sans-serif\" font-size=\"14\" lengthAdjust=\"spacing\" textLength=\"119\" x=\"12\" y=\"78.1426\">Not so famous Bob</text></g></svg>";
        svgModel.UpdateModel(svg);

    }
}