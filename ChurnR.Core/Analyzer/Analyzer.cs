using System.Text.RegularExpressions;
using ChurnR.Core.VcsAdapter;
using Serilog;

namespace ChurnR.Core.Analyzer;

public class Analyzer(ILogger logger, IVcsAdapter adapter) : IAnalyzer
{
    private readonly List<Regex> _includes = [];
    private readonly List<Regex> _excludes = [];

    public AnalysisResult Analyze(DateTime? backTo)
    {
        logger.Information("Collecting changed resources");
        var changedResources = adapter.ChangedResources(backTo);
        logger.Information("Found {0} changed resources", changedResources.Count());
        
        logger.Information("Include/exclude matching files with provided filters");
        if (_includes.Count > 0 || _excludes.Count > 0)
        {
            changedResources = changedResources.Where(x => 
                (_excludes.Count == 0 || _excludes.All(y => !y.IsMatch(x.FullFileName))) && 
                (_includes.Count == 0 || _includes.Any(y => y.IsMatch(x.FullFileName))));
        }
        logger.Information("{0} resources to analyze remain", changedResources.Count());
        
        return new AnalysisResult
        {
            FileChurn = changedResources
                .OrderByDescending(x => x.CommitCount)
                .ThenBy(x => x.FileName)
        };
    }

    public void AddInclude(string pattern)
    {
        _includes.Add(new Regex(pattern, RegexOptions.Compiled));
    }

    public void AddExcludes(IEnumerable<string> patterns)
    {
        foreach (var pattern in patterns)
        {
            _excludes.Add(new Regex(pattern, RegexOptions.Compiled));
        }
    }
}


