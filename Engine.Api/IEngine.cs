namespace Engine.Api;

public interface IEngine: IDisposable
{
    Task<String> GetPlantAsync(String script);
}