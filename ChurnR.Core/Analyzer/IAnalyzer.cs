namespace ChurnR.Core.Analyzer;

public interface IAnalyzer
{
    AnalysisResult Analyze(DateTime? backTo = null);
    void AddInclude(string pattern);
    void AddExcludes(IEnumerable<string> patterns);
}