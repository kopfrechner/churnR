using System.Text;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class TableReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : BaseReporter(logger, output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileStatistics)
    {
        Logger.Information("Generating table report");
        
        var max = fileStatistics.Max(x => x.FileName.Length);
        var i = fileStatistics.Max(x => x.CommitCount).ToString().Length;
        
        // padding
        var total = max + i + 3; //separators | .. | .. |
        var hline = "+".PadRight(total+3, '-')+"+";
        var sb = new StringBuilder();
        sb.AppendLine(hline);
        foreach (var fileStatistic in fileStatistics)
        {
            sb.Append("| ")
                    .Append(fileStatistic.FileName.PadRight(max))
                .Append(" | ")
                    .Append(fileStatistic.CommitCount.ToString().PadRight(i))
                .AppendLine(" |");
        }
        
        sb.AppendLine(hline);
        Out.Write(sb.ToString());
    }
}