using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace Targets;

enum MsSql_PID : byte
{
    Developer,
    Express,
    Standart,
    Enterprise
}

partial class Build
{
    class DatabaseConfig
    {
        public int Port { get; set; }

        public string Password { get; set; }

        public MsSql_PID Pid { get; set; }
    }
    
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory = RootDirectory / "Streetcode";
    AbsolutePath OutputDirectory => RootDirectory / "Output";
    AbsolutePath UnitTestsDirectory => RootDirectory / "Streetcode" / "Streetcode.XUnitTest";
    AbsolutePath IntegrationTestsDirectory => RootDirectory / "Streetcode" / "Streetcode.XIntegrationTest";
    AbsolutePath ClientDirectory => RootDirectory / "Streetcode" / "Streetcode.Client";
}

