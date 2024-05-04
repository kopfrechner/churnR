using ChurnR.Core.Analyzer;

namespace ChurnR.Core.Reporter;

public interface IReporter
{
    void Write(AnalysisResult result, int topRecords);
}
