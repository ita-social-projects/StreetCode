using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Targets;

partial class Build
{
    Target SetupNuke => _ => _
        //ToDo fix accessibility and remove line below
        .OnlyWhenStatic(() => false)
        .Executes(() =>
        {
            DotNetToolUpdate(_ => _
                .SetPackageName("nuke.globaltool")
                .EnableGlobal()
                .SetVerbosity(DotNetVerbosity.Quiet));
        });
}