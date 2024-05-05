using System.Diagnostics.CodeAnalysis;
using ChurnR.Core.Support;

namespace ChurnR.Core.Analyzer;

public class FileStatistics
{
    public required string FileName { get; init; }

    private readonly string _path;
    
    public required string Path
    {
        get => _path;
        [MemberNotNull(nameof(_path))] init => _path = PathSanitizer.ToForwardSlashes(value)!;
    }

    public string FullFileName => PathSanitizer.ToForwardSlashes(System.IO.Path.Combine(Path, FileName))!; 
    
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
        var cleanCurrentFilename = PathSanitizer.ToForwardSlashes(currentFilename);
        var cleanPreviousFilename = PathSanitizer.ToForwardSlashes(previousFilename);
        
        return HistoricFullFileNames
            .Select(PathSanitizer.ToForwardSlashes)
            .Any(historicFileName => 
            historicFileName!.Equals(cleanCurrentFilename, StringComparison.InvariantCultureIgnoreCase) ||
            historicFileName!.Equals(cleanPreviousFilename, StringComparison.InvariantCultureIgnoreCase)
        );
    }
}