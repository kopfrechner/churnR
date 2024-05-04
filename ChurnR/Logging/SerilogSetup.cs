using Microsoft.Extensions.Configuration;
using Serilog;

namespace ChurnR.Logging;

public static class SerilogSetup
{
    public static ILogger Setup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}