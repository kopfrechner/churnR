namespace ChurnR.Core.Processors.Cutoff;

public class PercentCutoffProcessor(float percent) : IProcessor
{
    public IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input)
    {
        //quick n dirty.
        var sum = input.Sum(x => x.Value);
        var tt = sum * percent;
        var count = 0;
        var tempsum = 0;
        foreach (var keyValuePair in input)
        {
            tempsum += keyValuePair.Value;
            if (tempsum > tt)
                break;
            count++;
        }
        return input.Take(count);
    }
}
