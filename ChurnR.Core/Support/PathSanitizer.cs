namespace ChurnR.Core.Support;

public static class PathSanitizer
{
    public static string? ToForwardSlashes(string? path)
    {
        return path?.Replace("\\", "/").Replace("\\\\", "/");
    }
}