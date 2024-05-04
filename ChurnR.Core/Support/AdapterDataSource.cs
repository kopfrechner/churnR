using System.Diagnostics;

namespace ChurnR.Core.Support;

public class AdapterDataSource : IAdapterDataSource
{
    public IEnumerable<string> GetDataWithQuery(string program, string args = "")
    {
        var process = new Process 
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = program,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            }
        };
        
        process.Start();
        
        while (!process.StandardOutput.EndOfStream)
        {
            var line = process.StandardOutput.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            // use yield to immediately process this line due to memory management
            yield return line;
        }
        
        process.WaitForExit();
    }
}