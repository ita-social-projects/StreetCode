using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Targets;

partial class Build
{
    [Parameter("commit message")]
    readonly string Msg = "make changes to the project";

    [Parameter("update Streetcode_Client submodule")]
    readonly bool WithCli = true;

    [Parameter("checkout to branch")]
    readonly bool Checkout = false;

    [Parameter("name of branch to checkout")]
    readonly string BName = "master";

    [Parameter("create a new branch")]
    readonly bool NewB = false;

    Target CommitMainRepo => _ => _
        .OnlyWhenDynamic(() => !GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            Git($"commit -a -m \"{Msg}\"");
        });

    Target CommitSubmodules => _ => _
        .DependsOn(UpdateSubmodules)
        .OnlyWhenDynamic(() => !GitHasCleanWorkingCopy(ClientDirectory) && WithCli)
        .Executes(() =>
        {
            //command below can`t work with spaces, we replace them by Unicode Character “⠀” (U+2800)
            var joinedMessage = string.Join("⠀", Msg.Split(" "));
            PowerShell($"git submodule foreach 'git add .; git commit -m \"{joinedMessage}\"'");
        });

    Target UpdateSubmodules => _ => _
        .OnlyWhenStatic(() => WithCli)
        .Executes(() =>
        {
            Git("submodule update --remote");
        });

    Target CheckoutBranch => _ => _
        .OnlyWhenStatic(() => Checkout)
        .Executes(() => 
        { 
            Git($"checkout {(NewB ? "-b" : string.Empty)} {BName}"); 
        });

    Target PullBackEnd => _ => _
        .DependsOn(CommitMainRepo)
        .Executes(() =>
        {
            Git("pull");
        });

    Target SetupGit => _ => _
        .DependsOn(CheckoutBranch, CommitSubmodules, PullBackEnd);

    Target PushAll => _ => _
        .DependsOn(AddMigration, PullBackEnd)
        .Executes(() =>
        {
            Git(WithCli ? "push --recurse-submodules=on-demand" : "push");
        });
}