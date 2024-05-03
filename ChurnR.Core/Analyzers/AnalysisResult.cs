namespace ChurnR.Core.Analyzers;

public class AnalysisResult
{
    public IOrderedEnumerable<KeyValuePair<string, int>> FileChurn { get; set; }
}
