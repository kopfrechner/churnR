using System.Globalization;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Reporter;
using ChurnR.Options;

namespace ChurnR;

public class Engine(
    IAnalyzer analyzer,
    IReporter reporter,
    OptionsBase options)
{
    public ExitCode Run()
    {
        if (options.IncludePattern is not null) analyzer.AddInclude(options.IncludePattern);
        if (options.ExcludePatterns.Any()) analyzer.AddExcludes(options.ExcludePatterns);
        
        // analyze
        var analysisResult = 
            TryGetCalculateStartDate(options.FromDate, out var startDate)
                ? analyzer.Analyze(startDate)
                : analyzer.Analyze();
        
        // report
        reporter.Write(analysisResult, options.TopRecords ?? int.MaxValue);
        
        return ExitCode.Ok;
    }

    private bool TryGetCalculateStartDate(string? dateString, out DateTime startDate)
    {
        if (int.TryParse(dateString, out var daysBack))
        {
            startDate = DateTime.Now.Subtract(TimeSpan.FromDays(daysBack));
            return true;
        }

        return DateTime.TryParseExact(dateString, "dd-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
    }
}