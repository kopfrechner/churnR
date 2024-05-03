namespace ChurnR.Core.Support;

static class SystemExts
{
    public static IEnumerable<string> SplitLines(this string text)
    {
        return text.Split(["\r\n", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries);
    }
}

