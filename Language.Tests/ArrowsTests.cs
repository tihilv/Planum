using System.Linq;
using Bundle.Uml.Tokenizer;
using Language.Api;
using Language.Common.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class ArrowsTests
{
    [TestMethod]
    public void NoEndSingleLineArrowTest()
    {
        var arrow = ArrowParser.Instance.Parse("--");
        Assert.IsNotNull(arrow);
        Assert.AreEqual(ArrowShape.No, arrow.Value.Start);
        Assert.AreEqual(ArrowShape.No, arrow.Value.End);
        Assert.AreEqual(2, arrow.Value.Length);
        Assert.AreEqual(LineType.Plain, arrow.Value.Type);
        Assert.AreEqual(Direction.Undefined, arrow.Value.Direction);
    }

    [TestMethod]
    public void NoEndBoldLineArrowTest()
    {
        var arrow = ArrowParser.Instance.Parse("===");
        Assert.IsNotNull(arrow);
        Assert.AreEqual(ArrowShape.No, arrow.Value.Start);
        Assert.AreEqual(ArrowShape.No, arrow.Value.End);
        Assert.AreEqual(3, arrow.Value.Length);
        Assert.AreEqual(LineType.Bold, arrow.Value.Type);
        Assert.AreEqual(Direction.Undefined, arrow.Value.Direction);
    }

    [TestMethod]
    public void ArrowEndDashLineArrowTest()
    {
        var arrow = ArrowParser.Instance.Parse("..>");
        Assert.IsNotNull(arrow);
        Assert.AreEqual(ArrowShape.No, arrow.Value.Start);
        Assert.AreEqual(ArrowShape.Arrow, arrow.Value.End);
        Assert.AreEqual(2, arrow.Value.Length);
        Assert.AreEqual(LineType.Dash, arrow.Value.Type);
        Assert.AreEqual(Direction.Undefined, arrow.Value.Direction);
    }

    [TestMethod]
    public void TwoTriangleEndBoldLineArrowTest()
    {
        var arrow = ArrowParser.Instance.Parse("<|=|>");
        Assert.IsNotNull(arrow);
        Assert.AreEqual(ArrowShape.Triangle, arrow.Value.Start);
        Assert.AreEqual(ArrowShape.Triangle, arrow.Value.End);
        Assert.AreEqual(1, arrow.Value.Length);
        Assert.AreEqual(LineType.Bold, arrow.Value.Type);
        Assert.AreEqual(Direction.Undefined, arrow.Value.Direction);
    }

    [TestMethod]
    public void ArrowDirectionTest()
    {
        var arrow = ArrowParser.Instance.Parse(".l.>");
        Assert.IsNotNull(arrow);
        Assert.AreEqual(ArrowShape.No, arrow.Value.Start);
        Assert.AreEqual(ArrowShape.Arrow, arrow.Value.End);
        Assert.AreEqual(2, arrow.Value.Length);
        Assert.AreEqual(LineType.Dash, arrow.Value.Type);
        Assert.AreEqual(Direction.Left, arrow.Value.Direction);
    }

    
    [TestMethod]
    public void NotArrowTest1()
    {
        var arrow = ArrowParser.Instance.Parse("d");
        Assert.IsNull(arrow);
    }

    [TestMethod]
    public void NotArrowTest2()
    {
        var arrow = ArrowParser.Instance.Parse("-q");
        Assert.IsNull(arrow);
    }

    [TestMethod]
    public void NotArrowTest3()
    {
        var arrow = ArrowParser.Instance.Parse("<>");
        Assert.IsNull(arrow);
    }

    [TestMethod]
    public void NotArrowTest4()
    {
        var arrow = ArrowParser.Instance.Parse("<-=>");
        Assert.IsNull(arrow);
    }

    [TestMethod]
    public void NotArrowTest5()
    {
        var arrow = ArrowParser.Instance.Parse("<-do->");
        Assert.IsNull(arrow);
    }
}