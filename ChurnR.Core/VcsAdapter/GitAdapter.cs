using System.Text.RegularExpressions;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class GitAdapter(IAdapterDataSource dataSource) : VcsAdapterBase(dataSource)
{
    private readonly Regex _commitShaLineMatcher = new("(^[a-f0-9]{40})\\s(.*)$", RegexOptions.Compiled);
    private readonly Regex _churnLineMatcher = new("^([0-9]{1,8})\\s{1,8}([0-9]{1,8})\\s{1,8}(.*)$", RegexOptions.Compiled);
    
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
                // skip commit lines
                continue;
            }
		
            var churnMatchResult = _churnLineMatcher.Match(line);
            if (churnMatchResult is { Success: false, Groups.Count: 4 })
            {
                // we can't handle this line
                continue;
            }

            if (!int.TryParse(churnMatchResult.Groups[1].Value, out var linesAdded) || 
                !int.TryParse(churnMatchResult.Groups[2].Value, out var linesDeleted))
            {
                // we can't parse the data we're interested in...
                continue;
            }
            
            var fileName = churnMatchResult.Groups[3].Value;
            
            var fileStatistic = fileStatistics.FirstOrDefault(x =>
                x.FileName.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));

            if (fileStatistic == null)
            {
                fileStatistic = new FileStatistics{ FileName = fileName };
                fileStatistics.Add(fileStatistic);
            }

            fileStatistic.CommitCount++;
            fileStatistic.LinesAdded += linesAdded;
            fileStatistic.LinesDeleted += linesDeleted;
        }

        return fileStatistics;
    }
}