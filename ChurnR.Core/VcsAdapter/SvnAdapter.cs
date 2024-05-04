using System.Text.RegularExpressions;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class SvnAdapter : VcsAdapterBase
{
    private readonly Regex _matcher = new(@"\W*[A,M]\W+(\/.*)\b",RegexOptions.Compiled);
    
    public override IEnumerable<FileStatistics> ChangedResources(DateTime? backTo)
    {
        var svnArgument = backTo == null
            ?  "log --verbose"
            : $"log --revision {{{backTo:yyyy-MM-dd}}}:{{{DateTime.Now:yyyy-MM-dd}}} --verbose";
        
        var fileStatistics = new List<FileStatistics>();
        foreach (var line in DataSource.GetDataWithQuery("svn", svnArgument))
        {
            var matchingResult = _matcher.Match(line);
            if (!matchingResult.Success || matchingResult.Groups.Count != 2)
            {
                continue;
            }

            var file = matchingResult.Groups[1].Value;

            var fileStatistic = fileStatistics.FirstOrDefault(x =>
                x.FileName.Equals(file, StringComparison.InvariantCultureIgnoreCase));

            if (fileStatistic == null)
            {
                fileStatistic = new FileStatistics{ FileName = file };
                fileStatistics.Add(fileStatistic);
            }

            fileStatistic.CommitCount++;
        }

        return fileStatistics;
    }
}