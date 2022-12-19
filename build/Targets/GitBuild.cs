using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Targets;

partial class Build
{
    [Parameter("commit message")]
    readonly string Msg = "make changes to the project";

    [Parameter("update Streetcode_Client supmodule")]
    bool WithCli = true;

    [Parameter("checkout to branch")]
    bool Checkouth = false;

    [Parameter("name of branch to checkout")]
    readonly string BName = "master";

    [Parameter("create a new branch")]
    bool NewB = false;

    //[Parameter("allow to update submodules")]
    //public bool WithCli { get => WithCli = true; }

    Target CommitChanges => _ => _
        .OnlyWhenDynamic(()=>!GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            Git("add .");
            Git($"commit -m \"{Msg}\"");
            PowerShell($"setx DOCKER_ATOM \"${DockerAtom}\"");
        });

    Target SetupSubmodules => _ => _
        .OnlyWhenDynamic(() => WithCli)
        .After(CommitChanges)
        .Executes(() =>
        {
            Git("submodule update --init --recursive");
        });

    Target SetupGit => _ => _
        .DependsOn(CommitChanges,SetupSubmodules)
        .Executes(() =>
        {
            Git("pull");
            if(Checkouth)
                Git($"checkout {(NewB ? "-b" : "" )} {BName}");

            WithCli = false;
        });
}
