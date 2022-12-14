using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.All);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    
    AbsolutePath SourceDirectory => RootDirectory;
    
    AbsolutePath Backend_Unit_Tests_Directory => RootDirectory / "StreetCode.Tests";

    Target Compile => _ => _
        .Executes(() =>
        {
            DockerBuild(s => s
                .SetPath(".")
                .SetPath(RootDirectory / "Dockerfile")
                .SetTag("17-alpine"));
        });

    /*
    Target Backend_All => _ => _
        .DependsOn(
            Backend_Clean,
            Backend_Test
        );
    

    Target All => _ => _
        .DependsOn(
            Backend_All);
    
    Target Backend_Clean => _ => _
        .Before(Backend_Restore)
        .Executes(() =>
        {
        });

    Target Backend_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Backend_Compile => _ => _
        .DependsOn(Backend_Restore)
        .Executes(() =>
        {
            DockerCompose();
            
            DockerBuild(s => s
                .SetPath(".")
                .SetPath(String.Empty)
                .SetTag("latest"));
            
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });
    
    Target Backend_Test => _ => _
        .DependsOn(
            Backend_UnitTest
        );

    Target Backend_UnitTest => _ => _
        .DependsOn(Backend_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Backend_Unit_Tests_Directory)
                .SetConfiguration(Configuration)
                .EnableNoBuild());
        });

    /*Target Test => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic()
        .Executes(() =>
        {
            var projects = Solution.GetProjects("*.Test");
    
            foreach (var project in projects)
            {
                DotNetTest(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                );
            }
        });
        */
}
