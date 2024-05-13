using ChurnR;
using ChurnR.Extensions;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var services = new ServiceCollection();

var parserResult = Parser.Default.ParseArguments<Options>(args);
parserResult.WithNotParsed(_ => Exit(ExitCode.Parameters));
parserResult.WithParsed(options => services.AddChurnR(options));

await using var provider = services.BuildServiceProvider();
using var serviceScope = provider.CreateScope();

var engine = serviceScope.ServiceProvider.GetRequiredService<Engine>();
var result = engine.Run();

Exit(result);
return;

void Exit(ExitCode exitCode, string? message = null)
{
    if (!string.IsNullOrWhiteSpace(message))
    {
        Log.Logger.Fatal(message);
    }

    Environment.Exit((int)exitCode);
}
