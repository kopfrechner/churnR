using ChurnR.Core.Analyzer;

namespace ChurnR.Core.Reporter;

public interface IReporter
{
    void Write(IAnalysisResult analysisResult, string? targetCutOff, int topRecords);
}
