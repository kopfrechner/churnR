namespace ChurnR.Core.Analyzer;

public interface IAnalyzer
{
    AnalysisResult Analyze(string? executionDirectory = null, DateTime? backTo = null);
    void AddInclude(string pattern);
    void AddExcludes(IEnumerable<string> patterns);
}