using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Engine.Api.Exceptions;
using Engine.PlantUml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests;

[TestClass]
public class PlantUmlEngineTests
{
    [TestMethod]
    public async Task InitTestAsync()
    {
        var javaBefore = Process.GetProcessesByName("java");
        var engine = new PlantUmlEngine();
        await engine.InitAsync();
        var javaAfterStart = Process.GetProcessesByName("java");
        Assert.AreEqual(javaBefore.Length + 1, javaAfterStart.Length);
        engine.Dispose();
        var javaAfterDispose = Process.GetProcessesByName("java");
        Assert.AreEqual(javaBefore.Length, javaAfterDispose.Length);
    }

    [TestMethod]
    public async Task SimpleTestAsync()
    {
        using (var engine = new PlantUmlEngine())
        {
            await engine.InitAsync();

            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("participant \"Famous Bob\" as Bob");
            sb.AppendLine("@enduml");

            var result = await engine.GetPlantAsync(sb.ToString());
            Assert.IsNotNull(result);
        }
    }
    
    [TestMethod]
    public async Task TttTestAsync()
    {
        using (var engine = new PlantUmlEngine())
        {
            await engine.InitAsync();
            
            var t = @"@startuml
!pragma svek_trace on
participant Participant as Foo [[l1]] 
actor       Actor       as Foo1
boundary    Boundary    as Foo2
control     Control     as Foo3
entity      Entity      as Foo4
database    Database    as Foo5
collections Collections as Foo6
queue       Queue       as Foo7
Foo -> Foo1 : To actor 
Foo -> Foo2 : To boundary
Foo -> Foo3 : To control
Foo -> Foo4 : To entity
Foo -> Foo5 : To database
Foo -> Foo6 : To collections
Foo -> Foo7: To queue
@enduml
";
            
            var result = await engine.GetPlantAsync(t);
            Assert.IsNotNull(result);
        }
    }
    
    [TestMethod]
    public async Task SequentialRequestsTestAsync()
    {
        using (var engine = new PlantUmlEngine())
        {
            await engine.InitAsync();

            var sb1 = new StringBuilder();
            sb1.AppendLine("@startuml");
            sb1.AppendLine("participant \"Famous Bob\" as Bob");
            sb1.AppendLine("@enduml");

            var result1 = await engine.GetPlantAsync(sb1.ToString());
            Assert.IsNotNull(result1);

            var sb2 = new StringBuilder();
            sb2.AppendLine("@startuml");
            sb2.AppendLine("participant \"Not so famous Bob\" as Bob");
            sb2.AppendLine("@enduml");

            var result2 = await engine.GetPlantAsync(sb2.ToString());
            Assert.IsNotNull(result2);
            
            Assert.AreNotEqual(result1, result2);
        }
    }
    
    [TestMethod]
    public async Task WrongScriptTestAsync()
    {
        using (var engine = new PlantUmlEngine())
        {
            await engine.InitAsync();

            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("participant \"Famous Bob\" aass Bob");
            sb.AppendLine("@enduml");

            try
            {
                var result = await engine.GetPlantAsync(sb.ToString());
                Assert.Fail("Should be an exception");
            }
            catch (EngineException exception)
            {
                Assert.AreEqual(2, exception.LineNumber);
            }
        }
    }
    
    [TestMethod]
    public async Task ValidScriptAfterWrongScriptTestAsync()
    {
        using (var engine = new PlantUmlEngine())
        {
            await engine.InitAsync();

            var sb1 = new StringBuilder();
            sb1.AppendLine("@startuml");
            sb1.AppendLine("participant \"Famous Bob\" aass Bob");
            sb1.AppendLine("@enduml");

            try
            {
                await engine.GetPlantAsync(sb1.ToString());
                Assert.Fail("Should be an exception");
            }
            catch (EngineException exception)
            {
            }
            
            var sb2 = new StringBuilder();
            sb2.AppendLine("@startuml");
            sb2.AppendLine("participant \"Not so famous Bob\" as Bob");
            sb2.AppendLine("@enduml");

            var result2 = await engine.GetPlantAsync(sb2.ToString());
            Assert.IsNotNull(result2);
        }
    }
}