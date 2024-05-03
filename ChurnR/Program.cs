using ChurnR.Commands;
using CommandLine;
using Serilog;

Log.Logger = new LoggerConfiguration()
    // add console as logging target
    .WriteTo.Console()
    // set default minimum level
    .MinimumLevel.Debug()
    .CreateLogger();


return Parser.Default.ParseArguments<GitCommand, SvnCommand>(args)
    .MapResult(
        (GitCommand opts) => opts.Run(),
        (SvnCommand opts) => opts.Run(),
        errs => 1);