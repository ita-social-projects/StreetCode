using Nuke.Common;
using Nuke.Common.Tooling;
using System;
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
        .Executes(() =>
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment != "Local")
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local", EnvironmentVariableTarget.Machine);
            }
        });

    Target SetupLocal => _ => _
        .OnlyWhenStatic(() => !Dockerize)
        .DependsOn(SetupFrontEnd, UpdateDatabase, SetupBackEnd);

}

