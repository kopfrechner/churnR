using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class SimpleReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    private static readonly ILogger Logger = Log.Logger;
    
    protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
    {
        foreach (var kvp in fileChurns)
        {
            Logger.Information("{0} > {1}", kvp.Key, kvp.Value);
        }
    }
}
