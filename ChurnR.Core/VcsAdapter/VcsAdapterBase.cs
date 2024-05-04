using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public abstract class VcsAdapterBase : IVcsAdapter
{
    public IAdapterDataSource DataSource { get; set; } = new AdapterDataSource();
    public abstract IEnumerable<FileStatistics> ChangedResources(DateTime? backTo);
}