using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Targets;

partial class Build
{
    Target PublishBackEnd => _ => _
        .DependsOn(AddMigration, CommitChanges)
        .After(PublishFrontEnd)
        .Executes(() =>
        {
            Git("push");
        });

    Target PublishFrontEnd => _ => _
        .OnlyWhenDynamic(() => WithCli)
        .Executes(() =>
        {
            //ToDo publish front-end
        });
}
