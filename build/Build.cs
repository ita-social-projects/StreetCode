using Nuke.Common;
using Nuke.Common.CI;

namespace Targets;

[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.TestAll);
}