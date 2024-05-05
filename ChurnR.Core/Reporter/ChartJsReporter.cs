using System.Globalization;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class ChartJsReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : BaseReporter(logger, output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileStatistics)
    {
        var htmlTemplate = File.ReadAllText("Reporter/Templates/ChurnR_ChartJs_Report_Template.html");
        
        Logger.Information("Generating ChartJs report");
        var churns = fileStatistics
            .Select(file => new
            {
                UniqueFileName = fileStatistics.Count(x => x.FileName.Equals(file.FileName, StringComparison.InvariantCultureIgnoreCase)) == 1
                    ? file.FileName
                    : file.FullFileName,
                file.CommitCount,
                file.TotalLineChurns,
                file.AverageLineChurnsPerCommit,
                FileNameCount = file.HistoricFullFileNames.Count
            }).ToList();
        
        var commitsPerFile = string.Join(",", churns.OrderByDescending(x => x.CommitCount).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.CommitCount}" + "}"));
        var totalLineChurnsPerFile = string.Join(",", churns.OrderByDescending(x => x.TotalLineChurns).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.TotalLineChurns}" + "}"));
        var averageChurnPerCommitPerFile = string.Join(",", churns.OrderByDescending(x => x.AverageLineChurnsPerCommit).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.AverageLineChurnsPerCommit.ToString(CultureInfo.InvariantCulture)}" + "}"));
        var renamesOrMovesPerFile = string.Join(",", churns.OrderByDescending(x => x.FileNameCount).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.FileNameCount}" + "}"));

        htmlTemplate = htmlTemplate
            .Replace("{commitsPerFile}", commitsPerFile)
            .Replace("{totalLineChurnsPerFile}", totalLineChurnsPerFile)
            .Replace("{averageChurnPerCommitPerFile}", averageChurnPerCommitPerFile)
            .Replace("{renamesOrMovesPerFile}", renamesOrMovesPerFile);

        Logger.Debug(htmlTemplate);
        
        Out.Write(htmlTemplate);
    }
}

