using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class GitAdapter : VcsAdapterBase
{
    public override IEnumerable<string> ChangedResources()
    {
        var text = DataSource.GetDataWithQuery("git log  --name-only --pretty=format:");
        return Parse(text);
    }

    public override IEnumerable<string> ChangedResources(DateTime backTo)
    {
        var text = DataSource.GetDataWithQuery($"git log --after={backTo:yyyy-MM-dd} --name-only --pretty=format:");
        return Parse(text);
    }

    public override IEnumerable<string> ParseImpl(string text)
    {
        return text.SplitLines();
    }
}
