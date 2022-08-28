using System.Diagnostics;
using System.Text;
using Engine.Api;
using Engine.Api.Exceptions;

namespace Engine.PlantUml;

public class PlantUmlEngine: IEngine
{
    private Process? _runningProcess;

    private readonly string _delimiter;
    private readonly SemaphoreSlim _semaphore;
    
    public PlantUmlEngine()
    {
        _semaphore = new SemaphoreSlim(1, 1);
        _delimiter = $"delimiter_{Guid.NewGuid().ToString()}";
    }

    public async Task InitAsync()
    {
        await PlantUmlResolver.InitAsync();
        await JavaResolver.InitAsync();

        RunPlantUmlAsync();
    }

    private void RunPlantUmlAsync()
    {
        Process process = new Process();

        var startInfo = process.StartInfo;
        startInfo.Environment.Add("JAVA_TOOL_OPTIONS", "-XX:+SuppressFatalErrorMessage");
        startInfo.FileName = JavaResolver.FilePath;
        startInfo.Arguments = $"-jar \"{PlantUmlResolver.FilePath}\" -pipe -pipedelimitor {_delimiter} -nometadata -stdrpt:1 -tsvg";
        startInfo.ErrorDialog = false;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        
        process.Start();

        _runningProcess = process;
    }

    private PlantUmlErrorReport? _errorReport;
    public async Task<String> GetPlantAsync(String script)
    {
        await _semaphore.WaitAsync();

        if (_runningProcess == null)
            await InitAsync();


        if (_runningProcess == null)
            throw new EngineException("Unable to run the PlantUml engine.", -1);
        
        var sb = new StringBuilder();
        
        try
        {
            _errorReport = new PlantUmlErrorReport();
            _runningProcess.ErrorDataReceived += ErrorReceived;
            _runningProcess.BeginErrorReadLine();
            
            await _runningProcess.StandardInput.WriteLineAsync(script);
            bool finished = false;
            do
            {
                var lastString = await _runningProcess.StandardOutput.ReadLineAsync();
                var index = lastString?.IndexOf(_delimiter);
                if (index >= 0)
                {
                    finished = true;
                    sb.Append(lastString!.Substring(0, index.Value));
                }
                else
                    sb.AppendLine(lastString);

            } while (!finished);
        }
        finally
        {
            _runningProcess.CancelErrorRead();
            _runningProcess.ErrorDataReceived -= ErrorReceived;

            _semaphore.Release();
            
            if (_errorReport?.HasError == true)
                throw new EngineException($"{_errorReport.Status}: {_errorReport.Label}", _errorReport.LineNumber);
        }

        return sb.ToString();
    }

    private void ErrorReceived(Object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null && _errorReport != null)
        {
            var split = e.Data.Split("=");
            if (split.Length == 2)
            {
                if (split[0].Equals("status", StringComparison.InvariantCultureIgnoreCase))
                    _errorReport.Status = split[1];


                if (split[0].Equals("lineNumber", StringComparison.InvariantCultureIgnoreCase))
                    _errorReport.LineNumber = int.Parse(split[1]);

                if (split[0].Equals("label", StringComparison.InvariantCultureIgnoreCase))
                    _errorReport.Label = split[1];
            }
        }
    }

    public void Dispose()
    {
        _runningProcess?.Kill();
        _runningProcess?.Dispose();
        _semaphore.Dispose();
    }
}