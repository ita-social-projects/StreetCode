using Nuke.Common;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Targets;

partial class Build
{
    Target RemoveEnvVariables => _ => _
        .Executes(() =>
        {
            PowerShell(@"reg delete HKEY_CURRENT_USER\Environment /v DOCKER_ATOM /f");
        });
    
    Target EndAll => _ => _
        .DependsOn(DropDatabase, CleanDocker, RemoveEnvVariables);
}

