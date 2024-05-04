using ChurnR.Core.Analyzers;
using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public abstract class BaseReporter(TextWriter output, IProcessor cutOffProcessor) : IReporter
{
    protected readonly TextWriter Out = output;

    public void Write(AnalysisResult r, int topRecords)
    {
        if (r.FileChurn.Any() == false)
            return;

        var fileChurns = cutOffProcessor.Apply(r.FileChurn).Take(topRecords);

        WriteImpl(fileChurns);
        
        Out.Flush();
    }
    
    protected abstract void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns);
}
