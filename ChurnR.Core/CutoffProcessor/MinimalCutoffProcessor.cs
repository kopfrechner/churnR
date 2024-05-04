using ChurnR.Core.Analyzer;

namespace ChurnR.Core.CutoffProcessor;

public class MinimalCutoffProcessor(int minimum) : IProcessor
{
    public IEnumerable<FileStatistics> Apply(IEnumerable<FileStatistics> input)
    {
        return input.Where(x => x.CommitCount > minimum);
    }
}
