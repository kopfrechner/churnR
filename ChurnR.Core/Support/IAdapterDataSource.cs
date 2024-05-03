using System.Diagnostics;
using System.Text;

namespace ChurnR.Core.Support;

public interface IAdapterDataSource
{
    string GetDataWithQuery(string command);
}

public class AdapterDataSource : IAdapterDataSource
{
    public string GetDataWithQuery(string command)
    {
        var sb = new StringBuilder();
        var process = Process.Start(command);
        process.OutputDataReceived += (_, e) => sb.AppendLine(e.Data);
        process.WaitForExit();
        return sb.ToString();
    }
}
