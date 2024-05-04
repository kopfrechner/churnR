namespace ChurnR.Core.Analyzer;

public class FileStatistics
{
    public required string FileName { get; init; }
    public int CommitCount { get; set; }
}