namespace ChurnR.Core.Analyzer;

public class AnalysisResult
{
    public required IOrderedEnumerable<FileStatistics> FileChurn { get; set; }
}