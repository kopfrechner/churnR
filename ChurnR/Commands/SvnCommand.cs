using ChurnR.Core.VcsAdapter;
using CommandLine;

namespace ChurnR.Commands;

[Verb("svn", HelpText = "Subversion Repository")]
class SvnCommand : CommandBase 
{
    [Option('p', "env-path", 
        Required = false, 
        HelpText = @"Add to PATH. i.e. for svn.exe you might add ""c:\tools"". Can add multiple with ;.")]
    public string? EnvPath { get; set; }

    protected override IVcsAdapter VcsAdapter => new SvnAdapter();
    
    protected override void LogSpecificOptions()
    {
        LogOption(EnvPath, nameof(EnvPath));
    }
    
    protected override bool Execute()
    {
        Console.WriteLine($"svn {Report}");
        return true;
    }
}