using ChurnR.Core.Analyzer;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public abstract class VcsAdapterBase(IAdapterDataSource dataSource) : IVcsAdapter
{
    public IAdapterDataSource DataSource { get; } = dataSource;
    public abstract IEnumerable<FileStatistics> ChangedResources(DateTime? backTo);
}