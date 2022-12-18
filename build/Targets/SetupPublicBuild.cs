using ICSharpCode.SharpZipLib;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using System.Linq;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace Targets;

partial class Build
{
    [Parameter("docker atom")] readonly string DockerAtom = "Streetcode";

    Target SetPublicEnvironmentVariables => _ => _
        .Executes(() =>
        {
            //ToAsk ??? how to use and what it means
        });

    Target SetupDocker => _ => _
        .DependsOn(SetPublicEnvironmentVariables)
        .Executes(() =>
        {
            //ToDo setup docker by docker-compose file
        });

    Target CleanDocker => _ => _
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

            var volumes = DockerVolumeLs(v=>v
                .SetFilter($"label=atom={DockerAtom}")
                .EnableQuiet());

            if (volumes.Any())
                DockerVolumeRm(s => s.AddVolumes(volumes.Select(v => v.Text)));
        });
}
