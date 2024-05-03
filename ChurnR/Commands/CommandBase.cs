using CommandLine;
using Serilog;

namespace ChurnR.Commands;

enum Reporter
{
    Table,
    Xml, 
    Csv
}

abstract class CommandBase
{
    private static readonly ILogger Logger = Log.Logger;
    private static readonly int SUCCESS_CODE = 0;
    private static readonly int ERROR_CODE = 1;
    
    [Option('d', "from-date",
        Required = false,
        HelpText = "Past date to calculate churn from. Absolute in dd-mm-yyyy or number of days back from now.")]
    public string? FromDate { get; set; }

    [Option('c', "churn", 
        Required = false, 
        HelpText = "Minimal churn. Specify either a number for minimum, or float for precent.")]
    public string? MinimalChurnRate { get; set; }

    [Option('t', "top", 
        Required = false, 
        HelpText = "Return this number of top records.")]
    public int? TopRecords { get; set; }

    [Option('r', "report", 
        Required = false, 
        HelpText = "Type of report to output. Use one of: table (default), xml, csv")]
    public Reporter? Report { get; set; }
    
    [Option('i', "input", 
        Required = false, 
        HelpText = "Get input from a file instead of running a versioning system. Must specify correct adapter via -a.")]
    public string? InputFile { get; set; }

    [Option('x', "exclude",
        Required = false,
        HelpText = "Exclude resources matching this regular expression")]
    public IEnumerable<string> ExcludePatterns { get; set; } = new List<string>();

    [Option('n', "include",
        Required = false, 
        HelpText = "Include resources matching this regular expression")]
    public string? IncludePattern { get; set; }
    
    public int Run()
    {
        LogOptions();
        
        return Execute() 
            ? SUCCESS_CODE  
            : ERROR_CODE;
    }

    private void LogOptions()
    {
        LogOption(FromDate, nameof(FromDate));
        LogOption(MinimalChurnRate, nameof(MinimalChurnRate));
        LogOption(TopRecords, nameof(TopRecords));
        LogOption(InputFile, nameof(InputFile));
        if (ExcludePatterns.Any()) LogOption(ExcludePatterns, nameof(ExcludePatterns));
        LogOption(IncludePattern, nameof(IncludePattern));
        LogSpecificOptions();
    }

    protected virtual void LogSpecificOptions() {}
    
    protected static void LogOption<T>(T? property, string propertyName)
    {
        if (property is not null) Logger.Information("{0} {1}", propertyName, property);
    }

    protected abstract bool Execute();
}