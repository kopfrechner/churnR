using ChurnR;
using ChurnR.Extensions;
using ChurnR.Options;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var parserResult = Parser.Default.ParseArguments<GitOptions, SvnOptions>(args);

parserResult.WithNotParsed(_ =>
{
    var helpText = HelpText.AutoBuild(parserResult, h => HelpText.DefaultParsingErrorsHandler(parserResult, h), e => e);
    Console.Error.Write(helpText);
    Exit(ExitCode.Parameters);
});


var services = new ServiceCollection(); 

parserResult.WithParsed<GitOptions>(gitOptions => services.AddChurnR(gitOptions));
parserResult.WithParsed<SvnOptions>(svnOptions => services.AddChurnR(svnOptions));

await using var provider = services.BuildServiceProvider();
using var serviceScope = provider.CreateScope();
var engine = serviceScope.ServiceProvider.GetRequiredService<Engine>();

Exit(engine.Run());


void Exit(ExitCode exitCode, string? message = null)
{
    if (!string.IsNullOrWhiteSpace(message))
    {
        Log.Logger.Fatal(message);
    }

    Environment.Exit((int)exitCode);
}
