using ChurnR.Core.Analyzer;

namespace ChurnR.Core.Reporter;

public interface IReporter
{
    void Write(AnalysisResult r, string? targetCutOff, int topRecords);
}
