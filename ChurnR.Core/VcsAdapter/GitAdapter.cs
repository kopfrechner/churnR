using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class GitAdapter(IAdapterDataSource dataSource) : VcsAdapterBase(dataSource)
{
    public override IEnumerable<FileStatistics> ChangedResources(DateTime? backTo)
    {
        var gitArgument = backTo == null
            ? "log  --name-only --pretty=format:"
            : $"log --after={backTo:yyyy-MM-dd} --name-only --pretty=format:";

        var fileStatistics = new List<FileStatistics>();
        foreach (var line in DataSource.GetDataWithQuery("git", gitArgument))
        {
            var fileStatistic = fileStatistics.FirstOrDefault(x =>
                x.FileName.Equals(line, StringComparison.InvariantCultureIgnoreCase));

            if (fileStatistic == null)
            {
                fileStatistic = new FileStatistics{ FileName = line };
                fileStatistics.Add(fileStatistic);
            }

            fileStatistic.CommitCount++;
        }

        return fileStatistics;
    }
}
