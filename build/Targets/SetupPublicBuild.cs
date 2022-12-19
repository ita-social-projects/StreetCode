using Nuke.Common;
using Nuke.Common.Tools.Docker;
using System.Linq;
using Nuke.Common.Tooling;
using static Utils.DockerComposeTasks;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace Targets;

partial class Build
{
    [Parameter("docker atom")]
    readonly string DockerAtom = "Streetcode";

    Target SetPublicEnvironmentVariables => _ => _
        .Executes(() =>
        {
            //ToAsk ??? how to use and what it means
        });

    Target SetupDocker => _ => _
        .DependsOn(SetPublicEnvironmentVariables)
        .Executes(() =>
        {
            DockerComposeBuild(b => b
                .SetProcessWorkingDirectory(SourceDirectory)
                .EnableNoCache()
                .EnableQuiet());

            DockerComposeUp(u => u
                .SetProcessWorkingDirectory(SourceDirectory)
                .EnableDetach());
        });

    Target CleanImages => _ => _
        .After(CleanContainers)
        .Executes(() =>
        {
            var images = DockerImageLs(i => i
                .SetFilter($"label=atom={DockerAtom}")
            );

            if (images.Any())
                DockerImageRm(r => r.AddImages(images
                    .Where((_, idx) => (idx & 1) == 1)
                    .Select(i => i.Text.Split(" ")[0])));
        });

    Target CleanContainers => _ => _
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

    Target CleanVolumes => _ => _
        .Executes(() =>
        {
            var volumes = DockerVolumeLs(v=>v
                .SetFilter($"label=atom={DockerAtom}")
                .EnableQuiet());

            if (volumes.Any())
                DockerVolumeRm(s => s.AddVolumes(volumes.Select(v => v.Text)));
        });

    Target CleanDocker => _ => _
        .Triggers(CleanContainers, CleanVolumes, CleanImages)
        .Executes(() =>
        {
            DockerComposeDown(u => u
                .SetProcessWorkingDirectory(SourceDirectory));
        });
}
