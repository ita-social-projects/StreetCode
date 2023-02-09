using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

namespace Targets;

partial class Build
{
    Target CleanTests => _ => _
    .Executes(() =>
    {
        UnitTestsDirectory
            .GlobDirectories("**/bin", "**/obj")
            .ForEach(DeleteDirectory);
        IntegrationTestsDirectory
            .GlobDirectories("**/bin", "**/obj")
            .ForEach(DeleteDirectory);
    });

    Target CleanNukeTemp => _ => _
    .Executes(() =>
    {
        NukeDirectory
            .GlobFiles("**/execution-plan.html")
            .ForEach(DeleteFile);
    });

    Target CleanSubmodule => _ => _
    .Executes(() =>
    {
        ClientDirectory
            .GlobDirectories("**/node-modules")
            .ForEach(DeleteDirectory);
    });

    Target Clean => _ => _
        .DependsOn(CleanTests, CleanNukeTemp, CleanSubmodule)
        .Executes(() =>
        {
            SourceDirectory
                .GlobDirectories("**/bin", "**/obj")
                .ForEach(DeleteDirectory);

            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.Quiet)
            );
        });

    Target CompileAPI => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
            );
        });

    Target CompileCli => _ => _
        .OnlyWhenStatic(() => WithCli)
        .Executes(() =>
        {
            NpmRun(_ => _
                .SetProcessWorkingDirectory(ClientDirectory));
        });
}