using ChurnR.Core.Analyzer;
using Serilog;

namespace ChurnR.Core.CutoffProcessor;

public class PercentCutoffProcessor(ILogger logger) : IProcessor
{
    public IEnumerable<FileStatistics> Apply(IEnumerable<FileStatistics> input, string? targetValue)
    {
        var percent = float.TryParse(targetValue, out var parsedPercent) ? parsedPercent : 50.0; 
        
        logger.Information("Taking the files included in the {0:F2}% total commit churns", percent);
        
        // quick n dirty = hell yeah!
        var sum = input.Sum(x => x.CommitCount);
        var threshold = sum * percent;
        var count = 0;
        var tempsum = 0;
        foreach (var fileStatistics in input)
        {
            tempsum += fileStatistics.CommitCount;
            if (tempsum > threshold)
                break;
            count++;
        }
        return input.Take(count);
    }
}
