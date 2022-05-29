using System.Reflection;

namespace Engine.PlantUml;

internal static class PlantUmlResolver
{
    private const string Version = "1.2022.5";

    private static readonly string _directory;
    private static readonly string _filePath;

    public static String FilePath => _filePath;

    static PlantUmlResolver()
    {
        _directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PlantUml");
        var fileName = $"plantuml-{Version}.jar";
        _filePath = Path.Combine(_directory, fileName);
    }

    internal static async Task InitAsync()
    {
        if (!Directory.Exists(_directory))
            Directory.CreateDirectory(_directory);
        if (!File.Exists(_filePath))
            await DownloadAsync();
    }
    
    private static async Task DownloadAsync()
    {
        using (var client = new HttpClient())
        using (var response = await client.GetAsync($"https://github.com/plantuml/plantuml/releases/download/v{Version}/plantuml-{Version}.jar"))
        using (var stream = await response.Content.ReadAsStreamAsync())
        using (var fileStream = new FileStream(_filePath, FileMode.Create))
            await stream.CopyToAsync(fileStream);
    }
}