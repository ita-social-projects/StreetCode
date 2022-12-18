using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuke.Common;


namespace Targets;

partial class Build
{
    Target SetLocalEnvironmentVariables => _ => _
        .DependsOn(Compile);

    Target SetupLocalDockerContainer => _ => _
        .DependsOn(StartDevelopmentContainers);

    Target CreateLocalDatabase => _ => _;

    Target UpdateLocalDatabase => _ => _
        .DependsOn(MigrateDatabase);

    Target SetupLocal => _ => _
        .DependsOn(SetLocalEnvironmentVariables, SetupLocalDockerContainer,
            CreateLocalDatabase, UpdateLocalDatabase);
}

