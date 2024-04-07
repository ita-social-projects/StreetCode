using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Utils;

public static class DockerComposeTasks
{
    internal static string DockerPath => ToolPathResolver.GetPathExecutable("docker-compose");

    internal static void CustomLogger(OutputType type, string output)
    {
        switch (type)
        {
            case OutputType.Std:
                Serilog.Log.Debug(output);
                break;
            case OutputType.Err:
            {
                if (output.StartsWith("WARNING!"))
                    Serilog.Log.Warning(output);
                else
                    Serilog.Log.Debug(output);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public static IReadOnlyCollection<Output> DockerComposeUp(Configure<DockerComposeUpSettings> configure) =>
        DockerComposeUp(configure(new DockerComposeUpSettings()));

    public static IReadOnlyCollection<Output> DockerComposeUp(DockerComposeUpSettings settings = null) =>
        StartProcess(settings ?? new DockerComposeUpSettings());

    public static IReadOnlyCollection<Output> DockerComposeDown(Configure<DockerComposeDownSettings> configure) =>
        DockerComposeDown(configure(new DockerComposeDownSettings()));

    public static IReadOnlyCollection<Output> DockerComposeDown(DockerComposeDownSettings settings = null) =>
        StartProcess(settings ?? new DockerComposeDownSettings());
    
    public static IReadOnlyCollection<Output> DockerComposeBuild(Configure<DockerComposeBuildSettings> configure) =>
        DockerComposeBuild(configure(new DockerComposeBuildSettings()));

    public static IReadOnlyCollection<Output> DockerComposeBuild(DockerComposeBuildSettings settings = null) =>
        StartProcess(settings ?? new DockerComposeBuildSettings());

    public static IReadOnlyCollection<Output> DockerComposeLogs(Configure<DockerComposeLogsSettings> configure) =>
        DockerComposeLogs(configure(new DockerComposeLogsSettings()));
        
    public static IReadOnlyCollection<Output> DockerComposeLogs(DockerComposeLogsSettings settings = null) =>
        StartProcess(settings ?? new DockerComposeLogsSettings());

    private static IReadOnlyCollection<Output> StartProcess([NotNull] ToolSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        var process = ProcessTasks.StartProcess(settings);
        process.AssertWaitForExit();
        return process.Output;
    }
}