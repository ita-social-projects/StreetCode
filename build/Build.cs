using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using System;
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;

enum MsSql_PID : byte
{
    Developer,
    Express,
    Standart,
    Enterprise
}

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    class DatabaseConfig
    {
        public int Port { get; set; }

        public string Password { get; set; }

        public MsSql_PID Pid { get; set; }
    }

    public static int Main() => Execute<Build>(x => x.TestAll);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory = RootDirectory / "Streetcode";
    AbsolutePath OutputDirectory => RootDirectory / "Output";
    AbsolutePath UnitTestsDirectory => RootDirectory / "Streetcode" / "Streetcode.XUnitTest";
    AbsolutePath IntegrationTestsDirectory => RootDirectory / "Streetcode" / "Streetcode.IntegrationTest";

    [Parameter("docker atom")] const string DockerAtom = "Streetcode";

    Target Clean => _ => _
        .DependsOn()
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.Quiet)
            );
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        //.Triggers(StartDevelopmentContainers)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
            );
        });

    Target CleanDevelopmentContainers => _ => _
     .After(Compile)
     .Executes(() =>
     {
         var runningContainers = DockerPs(s => s
             .SetFilter($"label=atom={DockerAtom}")
             .EnableQuiet());

         if (runningContainers.Any())
             DockerKill(s => s.AddContainers(runningContainers.Select(c => c.Text)));

         var containers = DockerPs(s => s
             .SetFilter($"label=atom={DockerAtom}")
             .EnableQuiet()
             .EnableAll());

         if (containers.Any())
             DockerRm(s => s.AddContainers(containers.Select(c => c.Text)));
     });

    private readonly string MsSqlImageName = "mcr.microsoft.com/mssql/server:latest";

    Target StartDevelopmentContainers => _ => _
        .DependsOn(CleanDevelopmentContainers)
        .Executes(() =>
        {
            var dbConfig = new DatabaseConfig
            {
                Password = "Sashacool1",
                Port = 49159,
                Pid = MsSql_PID.Developer
            };

            DockerRun(s => s
                .SetName("streetcode_sqlserver")
                .AddLabel($"atom={DockerAtom}")
                .AddEnv("ACCEPT_EULA=Y")
                .AddEnv($"MSSQL_PID={dbConfig.Pid.ToString()}")
                .AddEnv($"MSSQL_SA_PASSWORD={dbConfig.Password}")
                .SetPublish($"{dbConfig.Port}:1433")
                .EnableDetach()
                .SetImage(MsSqlImageName)
            );
        });

    Target UnitTest => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(UnitTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    Target IntegrationTest => _ => _
        .DependsOn(Compile,SetupLocal)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(IntegrationTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    Target TestAll => _ => _
        .DependsOn(UnitTest, IntegrationTest)
        .Triggers(EndAll);

     [Parameter("Specifies migration name during its creation")]
     readonly string MigrName = "New Migration Added";
     
     Target AddMigration => _ => _
         .DependsOn(Compile)
         .Executes(() =>
         {
             EntityFrameworkMigrationsAdd(_ => _
                 .SetProcessWorkingDirectory(SourceDirectory)
                 .SetName(MigrName)
                 .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                 .SetStartupProject(@"Streetcode\Streetcode.WebApi.csproj")  
                 .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")            
                 .SetConfiguration(Configuration)
                 .SetOutputDirectory(@"Persistence\Migrations"));
         });

     [Parameter("Specifies migration name during db update")]
     [CanBeNull] 
     readonly string UpdMigrName = default;

     [Parameter("Specifies whether a migration rollback should be committed")]
     readonly bool RollbackMigration = false;
     
     Target MigrateDatabase => _ => _
         .DependsOn(AddMigration)
         .Executes(() =>
         {
             EntityFrameworkDatabaseUpdate(_ => _
                 .SetProcessWorkingDirectory(SourceDirectory)
                 .SetMigration(RollbackMigration ? "0" : (UpdMigrName ?? String.Empty))
                 .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                 .SetStartupProject(@"Streetcode\Streetcode.WebApi.csproj")  
                 .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")            
                 .SetConfiguration(Configuration)
             );
         });

     Target DropDatabase => _ => _
         .Executes(() =>
         {
             EntityFrameworkDatabaseDrop(_ => _
                 .SetProcessWorkingDirectory(SourceDirectory)
                 .EnableForce()
                 .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                 .SetStartupProject(@"Streetcode\Streetcode.WebApi.csproj")
                 .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                 .SetConfiguration(Configuration)
             );
         });

    Target SetLocalEnvironmentVariables => _ => _
        .DependsOn(Compile);

    Target SetupLocalDockerContainer => _ => _
        .DependsOn(StartDevelopmentContainers);

    Target CreateLocalDatabase => _ => _;

    Target UpdateLocalDatabase => _ => _
        .DependsOn(MigrateDatabase);

    Target SetupLocal => _ => _
        .DependsOn(SetLocalEnvironmentVariables, SetupLocalDockerContainer, CreateLocalDatabase, UpdateLocalDatabase);

    Target EndAll => _ => _.DependsOn(DropDatabase,CleanDevelopmentContainers);
}
