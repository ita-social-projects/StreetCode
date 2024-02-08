using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
[GitHubActions(
    "windows-latest",
    GitHubActionsImage.WindowsLatest,
    FetchDepth = 0)]
[GitHubActions(
    "macos-latest",
    GitHubActionsImage.MacOsLatest,
    FetchDepth = 0)]
[GitHubActions(
    "ubuntu-latest",
    GitHubActionsImage.UbuntuLatest,
    FetchDepth = 0)]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Setup);

    Target Run => _ => _
        .DependsOn(CompileAPI, CompileCli);

    Target Setup => _ => _
        .DependsOn(SetupPublic, SetupLocal);

    Target Push => _ => _
        .DependsOn(PushAll);

    Target Test => _ => _
        .DependsOn(UnitTest, IntegrationTest)
        .Triggers(EndAll);
}