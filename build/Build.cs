using Nuke.Common;
using Nuke.Common.CI;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.SetupBuild);

    Target SetupBuild => _ => _
        .DependsOn(BuildLocal,BuildPublic);

    Target Publish => _ => _
        .DependsOn(PublishBackEnd,PublishFrontEnd);

    Target TestAll => _ => _
        .DependsOn(BuildPublic, UnitTest, IntegrationTest)
        .Triggers(EndAll);
}