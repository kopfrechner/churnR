namespace ChurnR.Core.CutoffProcessor;

public interface IProcessor
{
    IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input);
}

