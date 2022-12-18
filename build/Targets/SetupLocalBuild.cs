using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuke.Common;


namespace Targets;

partial class Build
{
    Target SetupBackEnd => _ => _
        .DependsOn(SetLocalEnvironmentVariables,Compile);

    Target SetupFrontEnd => _ => _
        .DependsOn(Compile);

    Target SetLocalEnvironmentVariables => _ => _
        .Before(Compile);

}

