using Nuke.Common;
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
            //ToDo 
            PowerShell($"cd {ClientDirectory}");
            PowerShell("npm install");
        });

    Target SetLocalEnvironmentVariables => _ => _
        .Before(SetupBackEnd)
        .Executes(() =>
        {
            //DoAsk what it means and how to use???
        });

    Target SetupLocal => _ => _
        .OnlyWhenStatic(() => !Dockerize)
        .DependsOn(SetupBackEnd, /*SetupDatabase,*/ SetupFrontEnd);

}

