using System.Globalization;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Reporter;
using ChurnR.Options;
using Serilog;

namespace ChurnR;

public class Engine(
    IAnalyzer analyzer,
    IReporter reporter,
    OptionsBase options,
    ILogger logger)
{
    public ExitCode Run()
    {
        // setup analyzer
        if (options.InputFile != null && !File.Exists(options.InputFile))
        {
            logger.Error("Cannot find file {0} to read from.", options.InputFile);
            return ExitCode.Error;
        }
        
        if (options.IncludePattern is not null) analyzer.AddInclude(options.IncludePattern);
        if (options.ExcludePatterns.Any()) analyzer.AddExcludes(options.ExcludePatterns);
        
        // analyze
        var analysisResult =
            options.InputFile != null  
                ? analyzer.Analyze(File.ReadAllText(options.InputFile)) : 
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