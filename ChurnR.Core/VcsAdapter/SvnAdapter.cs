using System.Text.RegularExpressions;
using ChurnR.Core.Support;

namespace ChurnR.Core.VcsAdapter;

public class SvnAdapter : VcsAdapterBase
{
    private readonly Regex _matcher = new(@"\W*[A,M]\W+(\/.*)\b",RegexOptions.Compiled);
    public override IEnumerable<string> ChangedResources()
    {
        var text = DataSource.GetDataWithQuery("svn", "log --verbose");
        return Parse(text);
    }

    public override IEnumerable<string> ChangedResources(DateTime backTo)
    {
        var text = DataSource.GetDataWithQuery("svn", $"log --revision {{{backTo:yyyy-MM-dd}}}:{{{DateTime.Now:yyyy-MM-dd}}} --verbose");
        return Parse(text);
    }

    public override IEnumerable<string> ParseImpl(string text)
    {
        var strings = text.SplitLines();
        return from s in strings
               select _matcher.Match(s)
                   into match
                   where match.Success && match.Groups.Count == 2
                   select match.Groups[1].Value;
    }
}