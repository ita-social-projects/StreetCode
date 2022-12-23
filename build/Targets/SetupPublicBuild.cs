using Nuke.Common;
using Nuke.Common.Tools.Docker;
using System.Linq;
using Nuke.Common.Tooling;
using static Utils.DockerComposeTasks;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Targets;

partial class Build
{
    [Parameter("docker atom")]
    readonly string DockerAtom = "Streetcode";

    Target SetPublicEnvironmentVariables => _ => _
        .Executes(() =>
        {
            PowerShell($"setx DOCKER_ATOM \"{DockerAtom}\"");
        });

    Target SetupDocker => _ => _
        .DependsOn(SetPublicEnvironmentVariables)
        .Executes(() =>
        {
            DockerComposeBuild(b => b
                .SetProcessWorkingDirectory(RootDirectory)
                .EnableNoCache()
                .EnableQuiet());

            DockerComposeUp(u => u
                .SetProcessWorkingDirectory(RootDirectory)
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
        .After(CleanContainers)
        .Executes(() =>
        {
            var volumes = DockerVolumeLs(v=>v
                .SetFilter($"label=atom={DockerAtom}")
                .EnableQuiet());

            if (volumes.Any())
                DockerVolumeRm(s => s.AddVolumes(volumes.Select(v => v.Text)));
        });

    [Parameter("Build in Docker")]
    bool Dockerize = false;

    Target SetupPublic => _ => _
        .OnlyWhenStatic(() => Dockerize)
        .DependsOn(UpdateDatabase, SetupDocker)
        .Executes(() =>
        {
            Dockerize = false;
        });

    Target CleanDocker => _ => _
        .Triggers(CleanVolumes, CleanImages, CleanContainers)
        .Executes(() =>
        {
            DockerComposeDown(u => u
                .SetProcessWorkingDirectory(RootDirectory));
        });
}
