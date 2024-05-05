using ChurnR.Core.Support;
using FluentAssertions;

namespace ChurnR.Core.Test.Support;

public class PathSanitizerTests
{
    [Theory]
    [InlineData("a/c\\d/c", "a/c/d/c")]
    [InlineData("a\\c\\d/c", "a/c/d/c")]
    [InlineData("aa\\\\d/c", "a/a/d/c")]
    public void can_sanitise_slashes(string input, string expectedSanitizedInput)
    {
        var sanitizedInput = PathSanitizer.ToForwardSlashes(input);

        sanitizedInput.Should().Be(expectedSanitizedInput);
    }
}