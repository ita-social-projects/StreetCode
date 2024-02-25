using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using System.IO;
using static Utils.ScriptProcessor;

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
        .OnlyWhenDynamic(() => IsScriptNameValid(ScriptName))
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
            SetScriptParametersForSingleMigration(
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