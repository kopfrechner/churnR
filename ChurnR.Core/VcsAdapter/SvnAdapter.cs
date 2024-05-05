using System.Text.RegularExpressions;
using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class SvnAdapter(IAdapterDataSource dataSource) : VcsAdapterBase(dataSource)
{
    private readonly Regex _matcher = new(@"\W*[A,M]\W+(\/.*)\b",RegexOptions.Compiled);
    
    public override IEnumerable<FileStatistics> ChangedResources(DateTime? backTo, string? executionDirectory)
    {
        var svnArgument = backTo == null
            ?  "log --verbose"
            : $"log --revision {{{backTo:yyyy-MM-dd}}}:{{{DateTime.Now:yyyy-MM-dd}}} --verbose";
        
        var fileStatistics = new List<FileStatistics>();
        foreach (var line in DataSource.GetDataWithQuery("svn", svnArgument, executionDirectory))
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
                fileStatistic = new FileStatistics
                {
                    FileName = Path.GetFileName(file),
                    Path = Path.GetDirectoryName(file) ?? "",
                    HistoricFullFileNames = new HashSet<string> { file }
                };
                fileStatistics.Add(fileStatistic);
            }

            fileStatistic.CommitCount++;
        }

        return fileStatistics;
    }
}