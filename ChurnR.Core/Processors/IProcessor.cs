namespace ChurnR.Core.Processors;

public interface IProcessor
{
    IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input);
}

