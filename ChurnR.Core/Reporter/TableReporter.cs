using System.Text;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class TableReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
    {
        var max = fileChurns.Max(x => x.Key.Length);
        var i = fileChurns.FirstOrDefault().Value.ToString().Length;
        
        // padding
        var total = max + i + 3; //separators | .. | .. |
        var hline = "+".PadRight(total+3, '-')+"+";
        var sb = new StringBuilder();
        sb.AppendLine(hline);
        foreach (var kvp in fileChurns)
        {
            sb.Append("| ").Append(kvp.Key.PadRight(max)).Append(" | ").Append(kvp.Value.ToString().PadRight(i)).AppendLine(" |");
        }
        sb.AppendLine(hline);
        output.Write(sb.ToString());
    }
}