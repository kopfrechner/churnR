using ChurnR.Core;
using ChurnR.Core.Reporter;
using CommandLine;

namespace ChurnR.Options;

public abstract record OptionsBase
{
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
        HelpText = "Type of report to output. Use one of: table (default), xml, csv, chartjs, simple")]
    public Reporter Reporter { get; set; } = Reporter.Table;

    [Option('x', "exclude",
        Required = false,
        HelpText = "Exclude resources matching a list of regular expressions")]
    public IEnumerable<string> ExcludePatterns { get; set; } = new List<string>();

    [Option('n', "include",
        Required = false, 
        HelpText = "Include resources matching this regular expression")]
    public string? IncludePattern { get; set; }
    
    [Option('o', "output",
        Required = false, 
        HelpText = "When set, writes the report to a specific file, to console otherwise.")]
    public string? Output { get; set; }
    
    [Option('p', "path", 
        Required = false, 
        HelpText = "Set path to your repository otherwise defaults to current directory.")]
    public string? ExecutionDirectory { get; set; }
    
    public abstract Vcs TargetVcs { get; }
}