using System.Text;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class CsvReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : BaseReporter(logger, output, cutOffProcessor)
{
    private const string Sep = ",";
    
    protected override void WriteImpl(IEnumerable<FileStatistics> fileStatistics)
    {
        Logger.Information("Generating CSV report");
        
        var sb = new StringBuilder();

        foreach (var fileStatistic in fileStatistics)
        {
            sb.AppendLine($@"'{fileStatistic.FileName}'
                            {Sep}{fileStatistic.CommitCount}
                            {Sep}{fileStatistic.LinesAdded}
                            {Sep}{fileStatistic.LinesDeleted}
                            {Sep}{fileStatistic.TotalLineChurns}
                            {Sep}{fileStatistic.AverageLineChurnsPerCommit}");
        }
        
        Out.Write(sb.ToString());
    }
}
