namespace ChurnR.Core.Analyzer;

public interface IAnalyzer
{
    AnalysisResult Analyze(string input);
    AnalysisResult Analyze(DateTime backTo);
    AnalysisResult Analyze();
    void AddInclude(string pattern);
    void AddExcludes(IEnumerable<string> patterns);
}