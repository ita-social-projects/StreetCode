using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Targets;

partial class Build
{
    Target UnitTest => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(UnitTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    Target IntegrationTest => _ => _
        .DependsOn(Compile, SetupLocal)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(IntegrationTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    Target TestAll => _ => _
        .DependsOn(UnitTest, IntegrationTest)
        .Triggers(EndAll);
}

