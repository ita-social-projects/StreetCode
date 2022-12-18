using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Targets;

partial class Build
{
    Target UnitTest => _ => _
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
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(IntegrationTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });
}

