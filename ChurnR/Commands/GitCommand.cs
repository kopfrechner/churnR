using ChurnR.Core.VcsAdapter;
using CommandLine;

namespace ChurnR.Commands;

[Verb("git", HelpText = "Git Repository")]
class GitCommand : CommandBase
{
    protected override IVcsAdapter VcsAdapter => new GitAdapter();
}