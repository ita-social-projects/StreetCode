using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Targets;

partial class Build
{
    [Parameter("commit message")]
    readonly string Msg = "make changes to the project";

    [Parameter("update Streetcode_Client supmodule")]
    readonly bool WithCli = true;

    [Parameter("checkout to branch")]
    readonly bool Checkout = false;

    [Parameter("name of branch to checkout")]
    readonly string BName = "master";

    [Parameter("create a new branch")]
    readonly bool NewB = false;

    Target CommitChanges => _ => _
        .OnlyWhenDynamic(() => !GitHasCleanWorkingCopy())
        .DependsOn(SetupNuke)
        .Executes(() =>
        {
            Git($"commit -a -m \"{Msg}\"");
            if (WithCli)
                Git($"submodule foreach 'git commit -a -m \"{Msg}\"'");
        });

    Target UpdateSubmodules => _ => _
        .OnlyWhenStatic(() => WithCli)
        .Executes(() =>
        {
            Git("submodule update --remote --merge");
        });

    Target CheckoutBranch => _ => _
        .OnlyWhenStatic(() => Checkout)
        .Executes(() => 
        { 
            Git($"checkout {(NewB ? "-b" : string.Empty)} {BName}"); 
        });

    Target PullBackEnd => _ => _
        .DependsOn(CommitChanges)
        .Executes(() =>
        {
            Git("pull");
        });

    Target SetupGit => _ => _
        .DependsOn(PullBackEnd, UpdateSubmodules, CheckoutBranch);

    Target PushAll => _ => _
        .DependsOn(AddMigration, PullBackEnd)
        .Executes(() =>
        {
            if (WithCli)
                Git("push --recurse-submodules=on-demand");
            else
                Git("push");
        });
}
