using System;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;

namespace Targets;

partial class Build
{
    [Parameter("Specifies migration name during its creation")]
    readonly string MigrName = "New Migration Added";
     
    Target AddMigration => _ => _
        //ToDo add condition to check if there is any changes in ef code first
        .OnlyWhenStatic(()=>false)
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
     
    Target SetupDatabase => _ => _
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
        //ToDo check if db exsists
        .OnlyWhenStatic(() => false)
        .Executes(() =>
        {
            EntityFrameworkDatabaseDrop(_ => _
                .SetProcessWorkingDirectory(SourceDirectory)
                .EnableForce()
                .SetProject(@"Streetcode.DAL\Streetcode.DAL.csproj")
                .SetStartupProject(@"Streetcode\Streetcode.WebApi.csproj")
                .SetContext("Streetcode.DAL.Persistence.StreetcodeDbContext")
                .SetConfiguration(Configuration)
            ); ;
        });
}