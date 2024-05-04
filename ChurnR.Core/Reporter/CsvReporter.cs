using System.Text;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class CsvReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    private const string Sep = ",";
    
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        var sb = new StringBuilder();

        foreach (var fileStatistics in fileChurns)
        {
            sb.AppendLine($"\"{fileStatistics.FileName}\"{Sep}{fileStatistics.CommitCount}");
        }
        
        Out.Write(sb.ToString());
    }
}
