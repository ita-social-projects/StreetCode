using Nuke.Common;
using Nuke.Common.Tooling;
using static Nuke.Common.Tools.Npm.NpmTasks;

namespace Targets;

partial class Build
{
    Target SetupBackEnd => _ => _
        .DependsOn(SetLocalEnvironmentVariables,SetupGit);

    Target SetupFrontEnd => _ => _
        .OnlyWhenStatic(() => WithCli)
        .After(SetupBackEnd)
        .Executes(() =>
        {
            NpmInstall(_ => _
                .SetProcessWorkingDirectory(ClientDirectory));
        });

    Target SetLocalEnvironmentVariables => _ => _
        .Before(SetupBackEnd)
        .Executes(() =>
        {
            //DoAsk what it means and how to use???
        });

    Target SetupLocal => _ => _
        .OnlyWhenStatic(() => !Dockerize)
        .DependsOn(SetupBackEnd, UpdateDatabase, SetupFrontEnd);

}

