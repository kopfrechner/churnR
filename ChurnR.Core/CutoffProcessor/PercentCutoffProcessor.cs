using ChurnR.Core.Analyzer;

namespace ChurnR.Core.CutoffProcessor;

public class PercentCutoffProcessor(float percent) : IProcessor
{
    public IEnumerable<FileStatistics> Apply(IEnumerable<FileStatistics> input)
    {
        //quick n dirty.
        var sum = input.Sum(x => x.CommitCount);
        var tt = sum * percent;
        var count = 0;
        var tempsum = 0;
        foreach (var fileStatistics in input)
        {
            tempsum += fileStatistics.CommitCount;
            if (tempsum > tt)
                break;
            count++;
        }
        return input.Take(count);
    }
}
