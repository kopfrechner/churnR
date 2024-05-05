using ChurnR.Core.Support;
using ChurnR.Core.VcsAdapter;
using Moq;
using Serilog;

namespace ChurnR.Core.Test.VcsAdapter;

public class GitAdapterFixture
{
    public Mock<IAdapterDataSource> AdapterDataSourceMock { get; set; } = new();
    public ILogger LoggerMock { get; set; } = Mock.Of<ILogger>();

    public IVcsAdapter CreateGitAdapterForLine(string line)
    {
        AdapterDataSourceMock
            .Setup(x => x.GetDataWithQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns([line]);
        
        return new GitAdapter(LoggerMock, AdapterDataSourceMock.Object);
    }
}