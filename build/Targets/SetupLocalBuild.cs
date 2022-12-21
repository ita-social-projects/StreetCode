using Nuke.Common;
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
            NpmInstall();
            //ToDo Compile in diferent way than for back-end
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

