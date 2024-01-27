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

    Target Push => _ => _
        .DependsOn(PushAll);

    Target Test => _ => _
        .DependsOn(UnitTest, IntegrationTest)
        .Triggers(EndAll);
}