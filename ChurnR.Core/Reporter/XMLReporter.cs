using System.Runtime.Serialization;
using System.Xml;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class XmlReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        var xr = new NChurnAnalysisResult
        {
            FileChurns = fileChurns
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

