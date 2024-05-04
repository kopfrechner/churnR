using ChurnR.Core.Analyzers;

namespace ChurnR.Core.Reporters;

public interface IReporter
{
    void Write(AnalysisResult result, int topRecords);
}
