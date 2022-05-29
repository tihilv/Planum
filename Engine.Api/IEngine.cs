namespace Engine.Api;

public interface IEngine: IDisposable
{
    Task<string> GetPlantAsync(string script);
}