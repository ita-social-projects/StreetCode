using System;
using Nuke.Common.Tooling;

namespace Utils;

[Serializable]
public class DockerComposeBuildSettings : DockerComposeSettings
{
    public bool NoCache { get; internal set; }

    public bool Quiet { get; internal set; }

    public DockerComposeBuildSettings SetNoCache(bool noCache)
    {
        NoCache = noCache;
        return this;
    }
    
    public DockerComposeBuildSettings SetQuiet(bool quiet)
    {
        Quiet = quiet;
        return this;
    }
    
    public DockerComposeBuildSettings EnableQuiet()
    {
        Quiet = true;
        return this;
    }
    
    public DockerComposeBuildSettings EnableNoCache()
    {
        NoCache = true;
        return this;
    }

    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments = base.ConfigureProcessArguments(arguments);
        arguments.Add("build")
            .Add("--no-cache", NoCache)
            .Add("--quiet", Quiet);
        return arguments;
    }

}