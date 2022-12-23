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

    Target UpdateSubmodules => _ => _
        .OnlyWhenStatic(() => WithCli)
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

    Target PullBackEnd => _ => _
        .After(CommitChanges)
        .Executes(() =>
        {
            Git("pull");
        });

    Target SetupGit => _ => _
        .DependsOn(UpdateSubmodules, CommitChanges, PullBackEnd, CheckoutBranch);

    Target PushBackEnd => _ => _
        .DependsOn(AddMigration, CommitChanges, PullBackEnd)
        .After(PushFrontEnd)
        .Executes(() =>
        {
            Git("push");
        });

    Target PushFrontEnd => _ => _
        .OnlyWhenStatic(() => WithCli)
        .Executes(() =>
        {
            //ToDo publish front-end
        });
}
