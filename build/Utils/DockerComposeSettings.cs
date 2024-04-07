using System;
using System.Collections.Generic;
using Nuke.Common.Tooling;

namespace Utils;

[Serializable]
public class DockerComposeSettings : ToolSettings
{
    public override string ProcessToolPath => DockerComposeTasks.DockerPath;
    public override Action<OutputType, string> ProcessCustomLogger => DockerComposeTasks.CustomLogger;

    internal List<string> FileInternal = default;

    public IReadOnlyCollection<string> File => FileInternal.AsReadOnly();
}