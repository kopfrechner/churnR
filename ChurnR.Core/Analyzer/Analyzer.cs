using System.Text.RegularExpressions;
using ChurnR.Core.VcsAdapter;

namespace ChurnR.Core.Analyzer;

public class Analyzer(IVcsAdapter adapter) : IAnalyzer
{
    private readonly List<Regex> _includes = [];
    private readonly List<Regex> _excludes = [];

    public AnalysisResult Analyze(DateTime? backTo)
    {
        var changedResources = GetChangedResources(backTo);

        // filter include and excludes
        if (_includes.Count > 0 || _excludes.Count > 0)
        {
            changedResources = changedResources.Where(x => 
                (_excludes.Count == 0 || _excludes.All(y => !y.IsMatch(x.FullFileName))) && 
                (_includes.Count == 0 || _includes.Any(y => y.IsMatch(x.FullFileName))));
        }
        
        return new AnalysisResult
        {
            FileChurn = changedResources
                .OrderByDescending(x => x.CommitCount)
                .ThenBy(x => x.FileName)
        };
    }

    private IEnumerable<FileStatistics> GetChangedResources(DateTime? backTo)
    {
        return adapter.ChangedResources(backTo);
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


