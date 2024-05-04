using System.Text;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class TableReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        var max = fileChurns.Max(x => x.FileName.Length);
        var i = fileChurns.Max(x => x.CommitCount).ToString().Length;
        
        // padding
        var total = max + i + 3; //separators | .. | .. |
        var hline = "+".PadRight(total+3, '-')+"+";
        var sb = new StringBuilder();
        sb.AppendLine(hline);
        foreach (var fileStatistics in fileChurns)
        {
            sb.Append("| ")
                    .Append(fileStatistics.FileName.PadRight(max))
                .Append(" | ")
                    .Append(fileStatistics.CommitCount.ToString().PadRight(i))
                .AppendLine(" |");
        }
        
        sb.AppendLine(hline);
        Out.Write(sb.ToString());
    }
}