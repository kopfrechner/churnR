using ChurnR.Core.Analyzer;
using Serilog;

namespace ChurnR.Core.CutoffProcessor;

public class MinimalCutoffProcessor(ILogger logger) : IProcessor
{
    
    public IEnumerable<FileStatistics> Apply(IEnumerable<FileStatistics> input, string? targetValue)
    {
        var minimum = int.TryParse(targetValue, out var parsedMinimum) ? parsedMinimum : 0;
        
        logger.Information("Cutting off where commit count is less than {0}", minimum);
        
        return input.Where(x => x.CommitCount > minimum);
    }
}
