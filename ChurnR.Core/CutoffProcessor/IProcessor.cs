using ChurnR.Core.Analyzer;

namespace ChurnR.Core.CutoffProcessor;

public interface IProcessor
{
    IEnumerable<FileStatistics> Apply(IEnumerable<FileStatistics> input);
}

