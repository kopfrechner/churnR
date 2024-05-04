using System.Globalization;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Reporter;
using ChurnR.Options;
using Serilog;

namespace ChurnR;

public class Engine(
    ILogger logger,
    IAnalyzer analyzer,
    IReporter reporter,
    OptionsBase options)
{
    public ExitCode Run()
    {
        logger.Information("Engine started with {0}", options);
        
        if (options.IncludePattern is not null) analyzer.AddInclude(options.IncludePattern);
        if (options.ExcludePatterns.Any()) analyzer.AddExcludes(options.ExcludePatterns);
        
        // analyze
        var analysisResult = 
            TryGetCalculateStartDate(options.FromDate, out var startDate)
                ? analyzer.Analyze(startDate)
                : analyzer.Analyze();
        
        // report
        reporter.Write(analysisResult, options.MinimalChurnRate, options.TopRecords ?? int.MaxValue);
        
        logger.Information("Engine successfully processed repository");
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