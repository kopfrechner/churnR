namespace ChurnR.Core.Analyzer;

public class FileStatistics
{
    public required string FileName { get; init; }
    
    /// <summary>
    /// Number of commits this file was part of
    /// </summary>
    public int CommitCount { get; set; }
    
    /// <summary>
    /// How many lines were added over all commits
    /// </summary>
    public int LinesAdded { get; set; }
    
    /// <summary>
    /// How many lines were deleted over all commits
    /// </summary>
    public int LinesDeleted { get; set; }

    /// <summary>
    /// Sum of <see cref="LinesAdded"/> + <see cref="LinesDeleted"/>
    /// </summary>
    public int TotalLineChurns => LinesAdded + LinesDeleted;
    
    /// <summary>
    /// Sum of <see cref="LinesAdded"/> + <see cref="LinesDeleted"/>
    /// </summary>
    public double AverageLineChurnsPerCommit => (double)TotalLineChurns / CommitCount;
}