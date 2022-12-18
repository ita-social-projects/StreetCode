using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Targets;

partial class Build
{
    Target PublishBackEnd => _ => _
        //.DependsOn(AddMigration)
        .Executes(() =>
        {
            Git("add .");
            Git($"commit -m \"{Msg}\"");
            Git("push");
        });

    Target PublishFrontEnd => _ => _
        .Executes(() =>
        {
            //ToDo publish front-end
        });
}
