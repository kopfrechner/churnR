namespace ChurnR.Core.Analyzer;

public class FileStatistics
{
    public required string FileName { get; init; }
    public required string Path { get; init; }
    
    /// <summary>
    /// For example, when the file was renamed or moved to another folder
    /// </summary>
    public required ISet<string> HistoricFullFileNames { get; set; }
    
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

    public bool HasOrHadFileName(string currentFilename, string? previousFilename)
    {
        return HistoricFullFileNames.Any(historicFileName => 
            historicFileName.Equals(currentFilename, StringComparison.InvariantCultureIgnoreCase) ||
            historicFileName.Equals(previousFilename, StringComparison.InvariantCultureIgnoreCase)
        );
    }
}