using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public interface IVcsAdapter
{
    IEnumerable<string> ChangedResources();
    IEnumerable<string> ChangedResources(DateTime backTo);
    IAdapterDataSource DataSource { get; set; }
    IEnumerable<string> Parse(string text);
}