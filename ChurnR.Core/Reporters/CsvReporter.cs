﻿using System.Text;
using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public class CsvReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    private const string Sep = ",";
    
    protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
    {
        var sb = new StringBuilder();

        foreach (var kvp in fileChurns)
        {
            sb.AppendLine($"\"{kvp.Key}\"{Sep}{kvp.Value}");
        }
        
        Out.Write(sb.ToString());
    }
}
