using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class SimpleReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : BaseReporter(logger, output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileStatistics)
    {
        Logger.Information("Generating simple report");
        
        foreach (var fileStatistic in fileStatistics)
        {
            Logger.Information("{0} > {1} commits | +{2} | -{3} | \u00b1 {4} | \u2300 {5} lines/commit", 
                fileStatistic.FileName, 
                fileStatistic.CommitCount,
                fileStatistic.LinesAdded,
                fileStatistic.LinesDeleted,
                fileStatistic.TotalLineChurns,
                fileStatistic.AverageLineChurnsPerCommit);
        }
    }
}
