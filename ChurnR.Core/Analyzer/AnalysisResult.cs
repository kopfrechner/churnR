namespace ChurnR.Core.Analyzer;

public class AnalysisResult : IAnalysisResult
{
    public required IOrderedEnumerable<FileStatistics> FileChurn { get; set; }
}