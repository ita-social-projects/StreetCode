using System;
using Nuke.Common;
using Nuke.Common.Tooling;
using JetBrains.Annotations;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using System.IO;
using Utils;

namespace Targets;

partial class Build
{
    [Parameter("Specifies migration name during its creation", Name = "migrname")]
    readonly string MigrationName;

    [Parameter("Specifies script file name", Name = "name")]
    string ScriptName = string.Empty;

    [Parameter("Specifies FROM migration for script generationg", Name = "from")]
    string FromMigration = string.Empty;

    [Parameter("Specifies TO migration for script generationg", Name = "to")]
    string ToMigration = string.Empty;

    bool CheckForMigration() =>
        GitHasCleanCopy(DALDirectory / "Entities")
        && GitHasCleanCopy(DALDirectory / "Enums") 
        && GitHasCleanCopy(DALDirectory / "Extensions") 
        && GitHasCleanCopy(DALDirectory / "Persistence" / "StreetcodeDbContext.cs");

    Target AddMigration => _ => _
        .Before(GenerateSQLScripts, SetScriptParametersForMigrationAndScriptAutogeneration)
        .Requires(() => MigrationName)
        .Executes(() =>
        {
            EntityFrameworkMigrationsAdd(_ => _
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetName(MigrationName)
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                .SetConfiguration(Configuration)
                .SetOutputDirectory(@"Persistence\Migrations")
                .EnablePrefixOutput());
        });

    //[Parameter("Specifies migration name during db update")]
    //[CanBeNull] 
    //readonly string UpdMigrName = default;

    //[Parameter("Specifies whether a migration rollback should be committed")]
    //readonly bool RollbackMigration = false;

    Target UpdateDatabase => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
            .SetProjectFile(DbUpdateDirectory)
            .SetConfiguration(Configuration));
        });

    Target GenerateSQLScripts => _ => _
        .Requires(() => ScriptName)
        .Requires(() => FromMigration)
        .Requires(() => ToMigration)
        .Executes(() =>
        {
            var startupProjectPath = APIDirectory;
            var outputScriptPath = Path.Combine(DALDirectory, "Persistence", "ScriptsMigration");
            string scriptFullName = Path.Combine(outputScriptPath, $"{ScriptName}.sql");

            EntityFrameworkMigrationsScript(_ => _
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetOutput(scriptFullName)
                .SetFrom(FromMigration)
                .SetTo(ToMigration)
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                .SetConfiguration(Configuration)
                .SetIdempotent(true)
                .SetNoTransactions(true));
        });

    Target GenerateMigrationAndScript => _ => _
        .Requires(() => MigrationName)
        .DependsOn(
            AddMigration,
            SetScriptParametersForMigrationAndScriptAutogeneration,
            GenerateSQLScripts);

    Target SetScriptParametersForMigrationAndScriptAutogeneration => _ => _
        .Requires(() => MigrationName)
        .Before(GenerateSQLScripts)
        .Executes(() =>
        {
            ScriptProcessor.SetScriptParametersForLastMigration(
                MigrationName,
                out ScriptName,
                out FromMigration,
                out ToMigration);
        });

    Target DropDatabase => _ => _
       .Executes(() =>
       {
           EntityFrameworkDatabaseDrop(_ => _
               .SetProcessWorkingDirectory(SourceDirectory)
               .EnableForce()
               .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
               .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
               .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
               .SetConfiguration(Configuration)
           );
       });
}