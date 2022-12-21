using Nuke.Common;
using Nuke.Common.CI;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Setup);

    Target Run => _ => _
        .DependsOn(CompileAPI, CompileCli);

    Target Setup => _ => _
        .DependsOn(SetupPublic, SetupLocal);

    Target Publish => _ => _
        .DependsOn(PublishFrontEnd, PublishBackEnd);

    Target Test => _ => _
        .DependsOn(SetupNuke)
        .DependsOn(UnitTest, IntegrationTest)
        .Triggers(EndAll);
}