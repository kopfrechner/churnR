using ChurnR.Core.Analyzers;
using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public abstract class BaseAnalysisReporter(TextWriter output) : IAnalysisReporter
{
    protected readonly TextWriter Out = output;

    public void Write(AnalysisResult r, IProcessor cutoffPolicy, int topRecords)
    {
        if (r.FileChurn.Any() == false)
            return;

        var fileChurns = cutoffPolicy.Apply(r.FileChurn).Take(topRecords);

        WriteImpl(fileChurns);
        
        Out.Flush();
    }
    
    protected abstract void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns);
}
