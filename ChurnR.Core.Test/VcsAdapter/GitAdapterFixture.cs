using ChurnR.Core.Support;
using ChurnR.Core.VcsAdapter;
using Moq;

namespace ChurnR.Core.Test.VcsAdapter;

public class GitAdapterFixture
{
    public Mock<IAdapterDataSource> AdapterDataSourceMock { get; set; } = new();

    public IVcsAdapter CreateGitAdapterForLine(string line)
    {
        AdapterDataSourceMock
            .Setup(x => x.GetDataWithQuery(It.IsAny<string>(), It.IsAny<string>()))
            .Returns([line]);
        
        return new GitAdapter(AdapterDataSourceMock.Object);
    } 
}