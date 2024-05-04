using System.Text.RegularExpressions;
using ChurnR.Core.VcsAdapter;

namespace ChurnR.Core.Analyzer;

public class Analyzer(IVcsAdapter adapter) : IAnalyzer
{
    private readonly List<Regex> _includes = [];
    private readonly List<Regex> _excludes = [];

    public AnalysisResult Analyze(string input)
    {
        var changedResources = adapter.Parse(input);
        return AnalyzeChangedResources(changedResources);
    }

    public AnalysisResult Analyze(DateTime backTo)
    {
        var changedResources = GetChangedResources(backTo);
        return AnalyzeChangedResources(changedResources);
    }
    public AnalysisResult Analyze()
    {
        var changedResources = GetChangedResources();
        return AnalyzeChangedResources(changedResources);
    }

    private AnalysisResult AnalyzeChangedResources(IEnumerable<string> changedResources)
    {
        var fileChurns = new Dictionary<string, FileStatistics>();

        foreach (var file in ApplyExcludeIncludes(changedResources))
        {
            fileChurns.TryAdd(file, new FileStatistics { FileName = file });
            fileChurns[file].CommitCount++;
        }

        return new AnalysisResult
        {
            FileChurn = fileChurns.Values
                .OrderByDescending(x => x.CommitCount)
                .ThenBy(x => x.FileName)
        };
    }

    private IEnumerable<string> ApplyExcludeIncludes(IEnumerable<string> changedResources)
    {
        if (_includes.Count == 0 && _excludes.Count == 0)
            return changedResources;

        return changedResources.Where(x => (_excludes.Count == 0 || _excludes.All(y => !y.IsMatch(x)) )
                                           &&  (_includes.Count == 0 || _includes.Any(y => y.IsMatch(x))));
                                
    }

    private IEnumerable<string> GetChangedResources()
    {
        return adapter.ChangedResources();
    }

    private IEnumerable<string> GetChangedResources(DateTime backTo)
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


