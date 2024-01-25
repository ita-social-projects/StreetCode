using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Utils;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using Nuke.Common.Tools.EntityFramework;

namespace Targets;

partial class Build
{
    [GitVersion] readonly GitVersion GitVersion;

    Target CleanTests => _ => _
    .Executes(() =>
    {
        UnitTestsDirectory
            .GlobDirectories("**/bin", "**/obj")
            .ForEach(DeleteDirectory);
        IntegrationTestsDirectory
            .GlobDirectories("**/bin", "**/obj")
            .ForEach(DeleteDirectory);
    });

    Target CleanNukeTemp => _ => _
    .Executes(() =>
    {
        NukeDirectory
            .GlobFiles("**/execution-plan.html")
            .ForEach(DeleteFile);
    });

    Target CleanSubmodule => _ => _
    .Executes(() =>
    {
        ClientDirectory
            .GlobDirectories("**/node-modules")
            .ForEach(DeleteDirectory);
    });

    Target Clean => _ => _
        .DependsOn(CleanTests, CleanNukeTemp, CleanSubmodule)
        .Executes(() =>
        {
            SourceDirectory
                .GlobDirectories("**/bin", "**/obj")
                .ForEach(DeleteDirectory);

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

    Target CompileAPI => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
            );
        });

    Target CompileCli => _ => _
        .OnlyWhenStatic(() => WithCli)
        .Executes(() =>
        {
            NpmRun(_ => _
                .SetProcessWorkingDirectory(ClientDirectory));
        });

    Target SetupIntegrationTestsEnvironment => _ => _
        .DependsOn(
            SetupIntegrationTestsEnvironmentVariables,
            CreateDatabaseForIntegrationTests,
            ApplyMigrationsForIntegrationTests);

    Target SetupIntegrationTestsEnvironmentVariables => _ => _
        .Executes(() =>
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests", EnvironmentVariableTarget.Process);
        });
    Target CreateDatabaseForIntegrationTests => _ => _
        .Executes(() =>
        {
            IntegrationTestsDatabaseConfiguration.CreateDatabase();
        });

    Target ApplyMigrationsForIntegrationTests => _ => _
        .OnlyWhenStatic(() => CheckForMigration())
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