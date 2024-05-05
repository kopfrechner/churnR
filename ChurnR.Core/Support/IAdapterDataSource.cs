namespace ChurnR.Core.Support;

public interface IAdapterDataSource
{
    IEnumerable<string> GetDataWithQuery(string program, string args, string? executionDirectory);
}