using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.PowerShell;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

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
        .Executes(() =>
        {
            //DoAsk what it means and how to use???
        });

    Target SetupLocal => _ => _
        .OnlyWhenStatic(() => !Dockerize)
        .DependsOn(SetupFrontEnd, UpdateDatabase, SetupBackEnd);

}

