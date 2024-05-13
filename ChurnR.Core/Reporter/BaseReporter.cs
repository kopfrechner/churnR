using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public abstract class BaseReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : IReporter
{
    protected readonly TextWriter Out = output;
    protected ILogger Logger => logger;

    public void Write(IAnalysisResult analysisResult, string? targetCutOff, int topRecords)
    {
        if (!analysisResult.FileChurn.Any())
        {
            Logger.Information("Skipping report since no churn was detected");
            return;
        }
        
        var fileChurns = cutOffProcessor.Apply(analysisResult.FileChurn, targetCutOff).Take(topRecords);

        WriteImpl(fileChurns);
        
        Out.Flush();
        Logger.Information("Wrote report file or console");
    }
    
    protected abstract void WriteImpl(IEnumerable<FileStatistics> fileStatistics);
}
