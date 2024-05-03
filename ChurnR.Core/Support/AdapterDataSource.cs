using System.Diagnostics;
using System.Text;

namespace ChurnR.Core.Support;

public class AdapterDataSource : IAdapterDataSource
{
    public string GetDataWithQuery(string program, string args = "")
    {
        var sb = new StringBuilder();
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
            sb.AppendLine(process.StandardOutput.ReadLine());
        }
        
        process.WaitForExit();
        return sb.ToString();
    }
}