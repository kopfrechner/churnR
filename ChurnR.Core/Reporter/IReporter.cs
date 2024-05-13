using ChurnR.Core.Analyzer;

namespace ChurnR.Core.Reporter;

public interface IReporter
{
    void Write(AnalysisResult analysisResult, string? targetCutOff, int topRecords);
}
