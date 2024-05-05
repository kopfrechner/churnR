using System.Diagnostics;
using Serilog;

namespace ChurnR.Core.Support;

public class AdapterDataSource(ILogger logger) : IAdapterDataSource
{
    public IEnumerable<string> GetDataWithQuery(string program, string args, string? executionDirectory)
    {
        if (executionDirectory != null && !Path.Exists(executionDirectory))
        {
            logger.Error("The specified executionDirectory '{0}' does not exist, skipping", executionDirectory);
            yield break;
        }
        
        var process = new Process 
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = program,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = executionDirectory ?? ""
            }
        };
        
        logger.Information("Gather VCS information: {0} {1}", program, args);
        process.Start();

        var processedLines = 0;
        var skippedLines = 0;
        
        while (!process.StandardOutput.EndOfStream)
        {
            var line = process.StandardOutput.ReadLine();
            processedLines++;
            if (string.IsNullOrWhiteSpace(line))
            {
                skippedLines++;
                continue;
            }
            
            // use yield to immediately process this line due to memory management
            yield return line;
        }
        
        process.WaitForExit();
        logger.Information("VCS information gathered successfully: Processed {0} lines, skipped {1}", processedLines, skippedLines);
    }
}