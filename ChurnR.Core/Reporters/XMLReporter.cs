using System.Runtime.Serialization;
using System.Xml;

namespace ChurnR.Core.Reporters;

public class XmlReporter(TextWriter output) : BaseAnalysisReporter(output)
{
    protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
    {
        var xr = new NChurnAnalysisResult
        {
            FileChurns = fileChurns
                .Select(x => new FileChurn { File = x.Key, Value = x.Value })
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

