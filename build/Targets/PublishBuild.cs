using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Targets;

partial class Build
{
    Target PushBackEnd => _ => _
        .DependsOn(AddMigration, CommitChanges)
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
