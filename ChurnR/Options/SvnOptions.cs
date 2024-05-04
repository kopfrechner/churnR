using ChurnR.Core;
using CommandLine;

namespace ChurnR.Options;

[Verb("svn", HelpText = "Subversion Repository")]
record SvnOptions : OptionsBase 
{
    [Option('p', "env-path", 
        Required = false, 
        HelpText = @"Add to PATH. i.e. for svn.exe you might add ""c:\tools"". Can add multiple with ;.")]
    public string? EnvironmentPath { get; set; }

    public override Vcs TargetVcs => Vcs.Svn;
}