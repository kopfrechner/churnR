using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public abstract class VcsAdapterBase : IVcsAdapter
{
    public IAdapterDataSource DataSource { get; set; } = new AdapterDataSource();
    public abstract IEnumerable<string> ChangedResources();
    public abstract IEnumerable<string> ChangedResources(DateTime backTo);
    public abstract IEnumerable<string> ParseImpl(string text);

    public IEnumerable<string> Parse(string text)
    {
        return string.IsNullOrEmpty(text) 
            ? Enumerable.Empty<string>() 
            : ParseImpl(text);
    }
    
}