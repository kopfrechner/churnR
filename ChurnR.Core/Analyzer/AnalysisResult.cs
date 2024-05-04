namespace ChurnR.Core.Analyzer;

public class AnalysisResult
{
    public IOrderedEnumerable<KeyValuePair<string, int>> FileChurn { get; set; }
}
