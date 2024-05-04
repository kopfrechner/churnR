using ChurnR.Core;
using CommandLine;

namespace ChurnR.Options;

[Verb("git", HelpText = "Git Repository")]
public record GitOptions : OptionsBase
{
    public override Vcs TargetVcs => Vcs.Git;
}