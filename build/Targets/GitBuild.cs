using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Targets;

partial class Build
{
    [Parameter("create new branch and checkout to it")]
    readonly string BName = "master";

    [Parameter("commit message")]
    readonly string Msg = "make changes to the project";

    [Parameter("create new branch and checkout to it")]
    readonly bool NewB = true;

    Target CommitChanges => _ => _
        .OnlyWhenDynamic(()=>!GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            Git("add .");
            Git($"commit -m \"{Msg}\"");
            PowerShell($"setx DOCKER_ATOM \"${DockerAtom}\"");
        });

    Target SetupGit => _ => _
        .DependsOn(CommitChanges)
        .Executes(() =>
        {
            Git("pull");
            Git($"checkout {(NewB ? "-b" : "" )} {BName}");
            Git("submodule update --init --recursive");
        });
}
