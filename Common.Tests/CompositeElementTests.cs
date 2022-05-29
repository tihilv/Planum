using System;
using System.Linq;
using Language.Api.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests;

[TestClass]
public class CompositeElementTests
{
    [TestMethod]
    public void AddLastTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).Last();
        composite.Make(element2).Last();
        composite.Make(element3).Last();
        composite.Make(element1).Last();

        var elements = composite.Children.ToArray();
        Assert.AreEqual(3, elements.Length);
        Assert.AreEqual(element2, elements[0]);
        Assert.AreEqual(element3, elements[1]);
        Assert.AreEqual(element1, elements[2]);
        Assert.AreEqual(composite, element1.Parent);
    }
    
    [TestMethod]
    public void AddFirstTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).First();
        composite.Make(element2).First();
        composite.Make(element3).First();
        composite.Make(element1).First();

        var elements = composite.Children.ToArray();
        Assert.AreEqual(3, elements.Length);
        Assert.AreEqual(element1, elements[0]);
        Assert.AreEqual(element3, elements[1]);
        Assert.AreEqual(element2, elements[2]);
        Assert.AreEqual(composite, element1.Parent);
    }
    
    [TestMethod]
    public void AddAfterTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).First();
        try
        {
            composite.Make(element2).After(element3);
            Assert.Fail("Should be an exception thrown");
        }
        catch (InvalidOperationException)
        {
            // ok
        }
        
        composite.Make(element2).After(element1);
        composite.Make(element3).After(element1);

        var elements = composite.Children.ToArray();
        Assert.AreEqual(3, elements.Length);
        Assert.AreEqual(element1, elements[0]);
        Assert.AreEqual(element3, elements[1]);
        Assert.AreEqual(element2, elements[2]);
        Assert.AreEqual(composite, element1.Parent);
    }
    
    [TestMethod]
    public void AddBeforeTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).First();
        try
        {
            composite.Make(element2).Before(element3);
            Assert.Fail("Should be an exception thrown");
        }
        catch (InvalidOperationException)
        {
            // ok
        }
        
        composite.Make(element2).Before(element1);
        composite.Make(element3).Before(element1);

        var elements = composite.Children.ToArray();
        Assert.AreEqual(3, elements.Length);
        Assert.AreEqual(element2, elements[0]);
        Assert.AreEqual(element3, elements[1]);
        Assert.AreEqual(element1, elements[2]);
        Assert.AreEqual(composite, element1.Parent);
    }
    
    [TestMethod]
    public void DeletedTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        var element4 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).Last();
        composite.Make(element2).Last();
        composite.Make(element3).Last();
        try
        {
            composite.Make(element4).Deleted();
            Assert.Fail("Should be an exception thrown");
        }
        catch (InvalidOperationException)
        {
            // ok
        }
        
        composite.Make(element1).Deleted();
        composite.Make(element3).Deleted();

        var elements = composite.Children.ToArray();
        Assert.AreEqual(1, elements.Length);
        Assert.AreEqual(element2, elements[0]);
        Assert.IsNull(element1.Parent);
        Assert.IsNotNull(element2.Parent);
    }
    
    [TestMethod]
    public void ReplacingTest()
    {
        var element1 = new MockSyntaxElement();
        var element2 = new MockSyntaxElement();
        var element3 = new MockSyntaxElement();
        var element4 = new MockSyntaxElement();
        
        var composite = new RootSyntaxElement();
        composite.Make(element1).Last();
        composite.Make(element2).Last();
        composite.Make(element3).Last();
        try
        {
            composite.Make(element1).Replacing(element4);
            Assert.Fail("Should be an exception thrown");
        }
        catch (InvalidOperationException)
        {
            // ok
        }
        
        composite.Make(element4).Replacing(element2);

        var elements = composite.Children.ToArray();
        Assert.AreEqual(3, elements.Length);
        Assert.AreEqual(element1, elements[0]);
        Assert.AreEqual(element4, elements[1]);
        Assert.AreEqual(element3, elements[2]);
        Assert.AreEqual(composite, element4.Parent);
    }
    
    private class MockSyntaxElement: SyntaxElement
    {
        
    }
}