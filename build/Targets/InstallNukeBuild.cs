using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Targets;

partial class Build
{
    Target SetupNuke => _ => _
        .Executes(() =>
        {
            try
            {
                DotNetToolInstall(_ => _
                    .SetPackageName("nuke.globaltool")
                    .EnableGlobal()
                    .SetVerbosity(DotNetVerbosity.Quiet));
            }
            catch
            {
                DotNetToolUpdate(_ => _
                    .SetPackageName("nuke.globaltool")
                    .EnableGlobal()
                    .SetVerbosity(DotNetVerbosity.Quiet));
            }
        });
}