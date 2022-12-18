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
        .DependsOn(Compile)
        .Executes(() =>
        {
            //ToDo Setup Front-end
            //ToDo Compile in diferent way than for back-end
        });

    Target SetLocalEnvironmentVariables => _ => _
        .Before(Compile)
        .Executes(() =>
        {
            //DoAsk what it means and how to use???
        });

}

