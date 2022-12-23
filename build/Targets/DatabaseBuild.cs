using System;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using Nuke.Common.IO;

namespace Targets;

partial class Build
{
    [Parameter("Specifies migration name during its creation")]
    readonly string MigrName = "New Migration Added";

    public bool CheckForMigration(AbsolutePath dalPath) => 
        GitHasCleanWorkingCopy(dalPath / "Entities") 
        && GitHasCleanWorkingCopy(dalPath / "Enums") 
        && GitHasCleanWorkingCopy(dalPath / "Extensions") 
        && GitHasCleanWorkingCopy(dalPath / "Persistance" / "StreetcodeDbContext.cs");
     

    public bool CheckForMigration()
    {

    };
     
    Target AddMigration => _ => _
        .OnlyWhenStatic(()=> !CheckForMigration(DALDirectory))
        .Executes(() =>
        {
            EntityFrameworkMigrationsAdd(_ => _
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetName(MigrName)
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")  
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")            
                .SetConfiguration(Configuration)
                .SetOutputDirectory(@"Persistence\Migrations"));
        });

    [Parameter("Specifies migration name during db update")]
    [CanBeNull] 
    readonly string UpdMigrName = default;

    [Parameter("Specifies whether a migration rollback should be committed")]
    readonly bool RollbackMigration = false;

    Target UpdateDatabase => _ => _
        .OnlyWhenStatic(() => false)
        .DependsOn(DropDatabase)
        .Executes(() =>
        {
            EntityFrameworkDatabaseUpdate(_ => _
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetMigration(RollbackMigration ? "0" : (UpdMigrName ?? String.Empty))
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
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
                .SetStartupProject(@"Streetcode.WebApi\Streetcode.WebApi.csproj")
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                .SetConfiguration(Configuration)
            );
        });
}