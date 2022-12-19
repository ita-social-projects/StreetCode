using Nuke.Common;
using Nuke.Common.CI;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.build);

    Target build => _ => _
        .DependsOn(BuildLocal,BuildPublic);

    Target publish => _ => _
        .DependsOn(PublishBackEnd,PublishFrontEnd);

    Target test => _ => _
        .DependsOn(BuildPublic, UnitTest, IntegrationTest)
        .Triggers(EndAll);
}