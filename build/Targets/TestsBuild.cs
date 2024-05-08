using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;
using System;
using Utils;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

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
        .DependsOn(SetupIntegrationTestsEnvironment)
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
        .DependsOn(SeedUsersToDatabase, UpdateDatabase, CreateDatabaseForIntegrationTests);

    Target SeedUsersToDatabase => _ => _
        .After(UpdateDatabase)
        .Executes(() =>
        {
            SeedUsersAndRoles.SeedDatabaseWithInitialUsers();
        });

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
}
