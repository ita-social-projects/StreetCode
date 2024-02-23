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

    bool CheckForMigration() =>
        GitHasCleanCopy(DALDirectory / "Entities")
        && GitHasCleanCopy(DALDirectory / "Enums") 
        && GitHasCleanCopy(DALDirectory / "Extensions") 
        && GitHasCleanCopy(DALDirectory / "Persistence" / "StreetcodeDbContext.cs");

    Target AddMigration => _ => _
        .OnlyWhenStatic(() => CheckForMigration())
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
                .SetOutputDirectory(@"Persistence\Migrations"));
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
            .SetConfiguration(Configuration)
            );
        });

    Target CreateSQLScripts => _ => _
        .Executes(() =>
        {
            Console.WriteLine("Select name of SQL script:");
            string queryName = Console.ReadLine();

            var dbPath = Path.Combine(RootDirectory, "Streetcode/Streetcode.WebApi");
            var outputScriptPath = Path.Combine(RootDirectory, "Streetcode/Streetcode.DAL", "Persistence", "ScriptsMigration");

            PowerShell("if (-not (Get-Command dotnet-ef.exe -ErrorAction SilentlyContinue)) {dotnet tool install--global dotnet - ef}");
            PowerShell(@$"dotnet ef migrations script --idempotent --output {outputScriptPath}/{queryName}.sql  --project {dbPath}");
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