namespace ChurnR.Core.Processors.Cutoff;

public class MinimalCutoffProcessor(int minimum) : IProcessor
{
    public IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input)
    {
        return input.Where(x => x.Value > minimum);
    }
}
