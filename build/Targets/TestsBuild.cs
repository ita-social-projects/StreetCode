using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;
using System;
using System.Threading;
using Utils;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;

namespace Targets;

partial class Build
{
    [Parameter("enable unit tests")]
    readonly bool UTest = true;

    [Parameter("enable integration tests")]
    readonly bool ITest = true;

    Target UnitTest => _ => _
        .OnlyWhenStatic(() => UTest)
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
        .OnlyWhenStatic(() => ITest)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(IntegrationTestsDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    Target SetupIntegrationTestsEnvironment => _ => _
        .DependsOn(UpdateDatabase, CreateDatabaseForIntegrationTests);

    Target SetupIntegrationTestsEnvironmentVariables => _ => _
        .Before(SetupDocker)
        .Executes(() =>
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests", EnvironmentVariableTarget.Process);
        });
    Target CreateDatabaseForIntegrationTests => _ => _
        .DependsOn(SetupDocker, SetupIntegrationTestsEnvironmentVariables)
        .Before(UpdateDatabase)
        .Executes(() =>
        {
            IntegrationTestsDatabaseConfiguration.CreateDatabase();
        });

    Target ApplyMigrationsForIntegrationTests => _ => _
        .OnlyWhenStatic(() => CheckForMigration())
        .DependsOn(CreateDatabaseForIntegrationTests)
        .Executes(() =>
        {
            EntityFrameworkDatabaseUpdate(_ => _
                .SetConnection(IntegrationTestsDatabaseConfiguration.ConnectionString)
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                .SetConfiguration(Configuration));
        });
}

