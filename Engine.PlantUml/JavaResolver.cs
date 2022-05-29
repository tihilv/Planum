namespace Engine.PlantUml;

internal static class JavaResolver
{
    private static string _javaExecutable;

    public static String FilePath => _javaExecutable;

    internal static async Task InitAsync()
    {
        var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (javaHome == null)
        {
            // Download JDK: https://docs.microsoft.com/en-us/java/openjdk/download
            throw new NotImplementedException("JAVA_HOME is not set.");
        }
        else
        {
            _javaExecutable = Path.Combine(javaHome, "bin", "java");
        }
    }
}