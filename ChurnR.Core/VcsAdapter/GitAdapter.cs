using System.Text.RegularExpressions;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;
using Serilog;

namespace ChurnR.Core.VcsAdapter;

public class GitAdapter(ILogger logger, IAdapterDataSource dataSource) : VcsAdapterBase(dataSource)
{
    // ff88c2849d7e2dcb55d265eb62620c1b07638ea8 split path and file, not properly working now...
    private readonly Regex _commitShaLineMatcher = 
        new("(^[a-f0-9]{40})\\s(.*)$", RegexOptions.Compiled);
    
    // 1       1       ChurnR/{Engine/ChurnREngine.cs => EngineBlubb/ChurnREngineBlubb.cs}
    private readonly Regex _churnLineMatcherWithFileChanged = 
        new("^([0-9]{1,8})\\s{1,8}([0-9]{1,8})\\s{1,8}(.*)\\{(.*) => (.*)\\}(.*)$", RegexOptions.Compiled);
    
    // 5       1       ChurnR.Core/VcsAdapter/GitAdapter.cs
    private readonly Regex _churnLineMatcher = 
        new("^([0-9]{1,8})\\s{1,8}([0-9]{1,8})\\s{1,8}(.*)$", RegexOptions.Compiled);
    
    public override IEnumerable<FileStatistics> ChangedResources(DateTime? backTo)
    {
        var gitArgument = backTo == null
            ? "log  --format=oneline --numstat"
            : $"log --after={backTo:yyyy-MM-dd} --format=oneline --numstat";
        
        var fileStatistics = new List<FileStatistics>();
        foreach (var line in DataSource.GetDataWithQuery("git", gitArgument))
        {
            if (_commitShaLineMatcher.IsMatch(line))
            {
                logger.Debug("Skipping: {0}", line);
                // skip commit lines
                continue;
            }

            if (!TryChurnWithPathChangeMatch(line, out var currentFilename, out var previousFilename, out var linesAdded, out var linesDeleted))
            {
                if (!TryChurnMatch(line, out currentFilename, out linesAdded, out linesDeleted))
                {
                    logger.Debug("Skipping: {0}", line);
                    continue;
                }
            }
            
            var fileStatistic = fileStatistics.FirstOrDefault(fileStatistic =>
                fileStatistic.HasOrHadFileName(currentFilename, previousFilename)
            );

            if (fileStatistic == null)
            {
                fileStatistic = new FileStatistics
                {
                    FileName = Path.GetFileName(currentFilename),
                    Path = Path.GetDirectoryName(currentFilename) ?? "",
                    HistoricFullFileNames = new HashSet<string>()
                };
                fileStatistics.Add(fileStatistic);
            }
            
            fileStatistic.CommitCount++;
            fileStatistic.LinesAdded += linesAdded;
            fileStatistic.LinesDeleted += linesDeleted;
            fileStatistic.HistoricFullFileNames.Add(currentFilename);
            if (previousFilename != null)
            {
                fileStatistic.HistoricFullFileNames.Add(previousFilename);
            }
        }

        return fileStatistics;
    }
    
    private bool TryChurnMatch(string line, out string filename, out int linesAdded, out int linesDeleted)
    {
        filename = "";
        linesAdded = 0;
        linesDeleted = 0;
        
        var churnMatchResult = _churnLineMatcher.Match(line);
        if (churnMatchResult is { Success: false, Groups.Count: 4 })
        {
            // we can't handle this line
            return false;
        }

        if (!int.TryParse(churnMatchResult.Groups[1].Value, out linesAdded) || 
            !int.TryParse(churnMatchResult.Groups[2].Value, out linesDeleted))
        {
            // we can't parse the data we're interested in...
            return false;
        }

        filename = churnMatchResult.Groups[3].Value;
        
        return true;
    }
    
    private bool TryChurnWithPathChangeMatch(string line, out string currentFilename, out string? previousFilename, out int linesAdded, out int linesDeleted)
    {
        currentFilename = "";
        previousFilename = null;
        linesAdded = 0;
        linesDeleted = 0;
        
        var churnMatchResult = _churnLineMatcherWithFileChanged.Match(line);
        if (churnMatchResult is { Success: false, Groups.Count: 7 })
        {
            // we can't handle this line
            return false;
        }

        if (!int.TryParse(churnMatchResult.Groups[1].Value, out linesAdded) || 
            !int.TryParse(churnMatchResult.Groups[2].Value, out linesDeleted))
        {
            // we can't parse the data we're interested in...
            return false;
        }
        
        // eg.: ChurnR.Core/{Reporters => Reporter}/Reporter.cs
        var prefix = churnMatchResult.Groups[3].Value;
        var optionPrevious = churnMatchResult.Groups[4].Value; // moved from
        var optionCurrent = churnMatchResult.Groups[5].Value; // moved to
        var postfix = churnMatchResult.Groups[6].Value;
        
        currentFilename = $"{prefix}{optionCurrent}{postfix}";
        previousFilename = $"{prefix}{optionPrevious}{postfix}";
        
        return true;
    }
}