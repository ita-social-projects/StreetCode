using Nuke.Common;
using System;
using static Nuke.Common.Tools.Git.GitTasks;

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
            Git($"commit -m {Msg}");
        });

    Target SetupGit => _ => _
        .DependsOn(CommitChanges)
        .Executes(() =>
        {
            Git("pull");
            Git($"checkout {(NewB ? "-b" : "" )} {BName}");
            //ToDo update submodules
        });
}
