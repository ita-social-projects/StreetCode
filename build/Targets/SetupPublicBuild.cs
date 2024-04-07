using Nuke.Common;
using Nuke.Common.Tools.Docker;
using System.Linq;
using Nuke.Common.Tooling;
using static Utils.DockerComposeTasks;
using static Nuke.Common.Tools.Docker.DockerTasks;
using System.IO;

namespace Targets;

partial class Build
{
    private const string DOCKER_COMPOSE_FOLDER_NAME = "Docker";

    Target SetupDocker => _ => _
        .Executes(() =>
        {
            DockerComposeBuild(b => b
                .SetProcessWorkingDirectory(Path.Combine(RootDirectory, DOCKER_COMPOSE_FOLDER_NAME))
                .EnableNoCache()
                .EnableQuiet());

            DockerComposeUp(u => u
                .SetProcessWorkingDirectory(Path.Combine(RootDirectory, DOCKER_COMPOSE_FOLDER_NAME))
                .EnableDetach());
        });

    Target CleanImages => _ => _
        .After(CleanContainers)
        .Executes(() =>
        {
            var images = DockerImageLs(i => i);

            if (images.Any())
                DockerImageRm(r => r.AddImages(images
                    .Where((_, idx) => (idx & 1) == 1)
                    .Select(i => i.Text.Split(" ")[0])));
        });

    Target CleanContainers => _ => _
        .Executes(() =>
        {
            var runningContainers = DockerPs(s => s
                .EnableQuiet());

            if (runningContainers.Any())
                DockerKill(s => s.AddContainers(runningContainers.Select(c => c.Text)));

            var containers = DockerPs(s => s
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
