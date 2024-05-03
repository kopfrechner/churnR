using CommandLine;

namespace ChurnR.Commands;

[Verb("git", HelpText = "Git Repository")]
class GitCommand : CommandBase 
{
    protected override bool Execute()
    {
        return true;
    }
}