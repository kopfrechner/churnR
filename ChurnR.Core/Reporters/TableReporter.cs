using System.Text;
using ChurnR.Core.Analyzers;
using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public class TableReporter(TextWriter output) : IAnalysisReporter
{
    public void Write(AnalysisResult r, IProcessor cutoffPolicy , int top)
    {
        if (r.FileChurn.Any() == false)
            return;

        var max = r.FileChurn.Max(x => x.Key.Length);
        var i = r.FileChurn.FirstOrDefault().Value.ToString().Length;
        
        //padding
        
        var total = max + i + 3; //separators | .. | .. |
        var hline = "+".PadRight(total+3, '-')+"+";
        var sb = new StringBuilder();
        sb.AppendLine(hline);
        foreach (var kvp in cutoffPolicy.Apply(r.FileChurn).Take(top))
        {
            sb.Append("| ").Append(kvp.Key.PadRight(max)).Append(" | ").Append(kvp.Value.ToString().PadRight(i)).AppendLine(" |");
        }
        sb.AppendLine(hline);
        output.Write(sb.ToString());
        output.Flush();
    }
}