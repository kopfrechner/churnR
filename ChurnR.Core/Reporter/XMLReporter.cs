using System.Runtime.Serialization;
using System.Xml;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using Serilog;

namespace ChurnR.Core.Reporter;

public class XmlReporter(ILogger logger, TextWriter output, IProcessor cutOffProcessor) : BaseReporter(logger, output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileStatistics)
    {
        Logger.Information("Generating XML report");
        
        var xr = new NChurnAnalysisResult
        {
            FileChurns = fileStatistics
                .Select(x => new FileChurn { File = x.FileName, Value = x.CommitCount })
                .ToList()
        };

        var ds = new DataContractSerializer(typeof(NChurnAnalysisResult));

        var settings = new XmlWriterSettings { Indent = true };

        using var xw = XmlWriter.Create(Out, settings);
        ds.WriteObject(xw, xr);
    }
}
[DataContract(Namespace="")]
public class NChurnAnalysisResult
{
    [DataMember]
    public List<FileChurn> FileChurns = new();
}

[DataContract(Namespace="")]
public class FileChurn
{
    [DataMember]
    public string File = "";
    [DataMember]
    public int Value;
}

