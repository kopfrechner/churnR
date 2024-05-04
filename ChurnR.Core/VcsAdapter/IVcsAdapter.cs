using ChurnR.Core.Analyzer;

namespace ChurnR.Core.VcsAdapter;

public interface IVcsAdapter
{
    IEnumerable<FileStatistics> ChangedResources(DateTime? backTo);
}