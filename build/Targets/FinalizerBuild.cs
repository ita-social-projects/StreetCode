using Nuke.Common;

namespace Targets;

partial class Build
{
    Target EndAll => _ => _
        .DependsOn(DropDatabase, CleanDocker);
}

