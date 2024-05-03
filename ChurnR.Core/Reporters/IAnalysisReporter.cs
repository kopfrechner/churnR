using ChurnR.Core.Analyzers;
using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public interface IAnalysisReporter
{
    void Write(AnalysisResult result, IProcessor cutoffPolicy, int topRecords);
}
