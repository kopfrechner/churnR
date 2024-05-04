using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public interface IVcsAdapter
{
    IEnumerable<FileStatistics> ChangedResources(DateTime? backTo);
    IAdapterDataSource DataSource { get; set; }
}