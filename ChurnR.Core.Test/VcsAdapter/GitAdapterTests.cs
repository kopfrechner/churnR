using ChurnR.Core.Analyzer;
using FluentAssertions;

namespace ChurnR.Core.Test.VcsAdapter;

public class GitAdapterTests(GitAdapterFixture gitAdapterFixture) : IClassFixture<GitAdapterFixture>
{
    [Theory]
    [InlineData("ff88c2849d7e2dcb55d265eb62620c1b07638ea8 split path and file, not properly working now...")]
    [InlineData("just a random line that should be able to be parsed and thus skipped")]
    public void should_skip_these_lines(string line)
    {
        // Arrange
        var gitAdapter = gitAdapterFixture.CreateGitAdapterForLine(line);
        
        // Act
        var fileStatistics = gitAdapter.ChangedResources(null);
        
        // Assert
        fileStatistics.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("1       2       A/{B.cs => B/C.cs}", 1, 2, "A/B/C.cs", "A/B.cs")]
    [InlineData("2       3       A/{B => C}/D.cs", 2, 3, "A/C/D.cs", "A/B/D.cs")]
    [InlineData("4       5       {B => C}/D.cs", 4, 5, "C/D.cs", "B/D.cs")]
    [InlineData("5       1       A/B.cs", 5, 1, "A/B.cs")]
    [InlineData("1234567 7654321 A/D.cs", 1234567, 7654321, "A/D.cs")]
    public void can_skip_commit_lines(
        string line, 
        int expectedLinesAdded, 
        int expectedLinesDeleted, 
        params string[] expectedHistoryFileNames)
    {
        // Arrange
        var gitAdapter = gitAdapterFixture.CreateGitAdapterForLine(line);

        // Act
        var fileStatistics = gitAdapter.ChangedResources(null).ToList();

        // Assert
        fileStatistics.Count.Should().Be(1);
        fileStatistics.First().Should().BeEquivalentTo(
            new FileStatistics
            {
                LinesAdded = expectedLinesAdded,
                LinesDeleted = expectedLinesDeleted,
                HistoricFullFileNames = expectedHistoryFileNames.ToHashSet(),
                FileName = Path.GetFileName(expectedHistoryFileNames.First()),
                Path = Path.GetDirectoryName(expectedHistoryFileNames.First()) ?? "",
                CommitCount = 1
            }
        );
    }
}
