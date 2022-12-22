using Nuke.Common;
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
        });

    Target SetupSubmodules => _ => _
        .OnlyWhenStatic(() => WithCli)
        .After(CommitChanges)
        .Executes(() =>
        {
            Git("submodule update --init --recursive");
        });

    Target CheckoutBranch => _ => _
        .OnlyWhenStatic(() => Checkout)
        .Executes(() => 
        { 
            Git($"checkout {(NewB ? "-b" : string.Empty)} {BName}"); 
        });

    Target SetupGit => _ => _
        .DependsOn(SetupSubmodules,CommitChanges,CheckoutBranch)
        .Executes(() =>
        {
            Git("pull");
        });
}
