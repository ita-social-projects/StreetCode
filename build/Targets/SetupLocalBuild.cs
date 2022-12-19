using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuke.Common;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;


namespace Targets;

partial class Build
{
    Target SetupBackEnd => _ => _
        .DependsOn(SetLocalEnvironmentVariables,Compile);

    Target SetupFrontEnd => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            //ToDo Setup Front-end
            //ToDo Compile in diferent way than for back-end
        });

    Target SetLocalEnvironmentVariables => _ => _
        .Before(Compile)
        .Executes(() =>
        {
            PowerShell($"setx DOCKER_ATOM \"${DockerAtom}\"");
            //DoAsk what it means and how to use???
        });

    Target BuildLocal => _ => _
        .OnlyWhenDynamic(() => !Dockerize)
        .DependsOn(SetupBackEnd, SetupDatabase, SetupFrontEnd);

}

