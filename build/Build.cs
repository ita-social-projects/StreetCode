using Nuke.Common;
using Nuke.Common.CI;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.BuildLocal);

    Target BuildLocal => _ => _
        .DependsOn(SetupBackEnd,SetupDatabase,SetupFrontEnd);

    Target BuildPublic => _ => _
        .DependsOn(SetupDocker,SetupDatabase);

    Target Publish => _ => _
        .DependsOn(PublishBackEnd,PublishFrontEnd);

    Target Test => _ => _
        .DependsOn(BuildPublic, UnitTest, IntegrationTest)
        .Triggers(EndAll);
}