using Nuke.Common;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Build.Targets;

class InstallNukeBuild
{
    bool chechIfInstalled = default;

    Target InstallNuke => _ => _
        .OnlyWhenDynamic(() => chechIfInstalled)
        .Executes(() =>
        {
            //dotnet tool install Nuke.GlobalTool --global
            DotNet("tool install Nuke.GlobalTool --global");
        });
}