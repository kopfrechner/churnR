namespace ChurnR.Core.Support;

public interface IAdapterDataSource
{
    string GetDataWithQuery(string program, string args);
}