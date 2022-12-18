using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace Targets;

partial class Build
{
    readonly string MsSqlImageName = "mcr.microsoft.com/mssql/server:latest";

    Target CreateContainer => _ => _
        .After(Compile)
        .DependsOn(CleanDevelopmentContainers)
        .Executes(() =>
        {
        });
    
    Target CleanDevelopmentContainers => _ => _
        .After(Compile)
        .Executes(() =>
        {
            var runningContainers = DockerPs(s => s
                .SetFilter($"label=atom={DockerAtom}")
                .EnableQuiet());

            if (runningContainers.Any())
                DockerKill(s => s.AddContainers(runningContainers.Select(c => c.Text)));

            var containers = DockerPs(s => s
                .SetFilter($"label=atom={DockerAtom}")
                .EnableQuiet()
                .EnableAll());

            if (containers.Any())
                DockerRm(s => s.AddContainers(containers.Select(c => c.Text)));
        });
    
    Target StartDevelopmentContainers => _ => _
        .DependsOn(CleanDevelopmentContainers)
        .Executes(() =>
        {
            var dbConfig = new DatabaseConfig
            {
                Password = "Sashacool1",
                Port = 49159,
                Pid = MsSql_PID.Developer
            };

            DockerRun(s => s
                .SetName("streetcode_sqlserver")
                .AddLabel($"atom={DockerAtom}")
                .AddEnv("ACCEPT_EULA=Y")
                .AddEnv($"MSSQL_PID={dbConfig.Pid.ToString()}")
                .AddEnv($"MSSQL_SA_PASSWORD={dbConfig.Password}")
                .SetPublish($"{dbConfig.Port}:1433")
                .EnableDetach()
                .SetImage(MsSqlImageName)
            );
        });

}

