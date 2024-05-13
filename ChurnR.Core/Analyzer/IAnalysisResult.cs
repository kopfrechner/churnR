namespace ChurnR.Core.Analyzer;

public interface IAnalysisResult
{
    public IOrderedEnumerable<FileStatistics> FileChurn { get; }
}