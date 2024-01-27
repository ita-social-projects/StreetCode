using System;
using System.IO;
using static Nuke.Common.Tools.PowerShell.PowerShellTasks;

namespace Utils
{
    internal static class DockerHelper
    {
        public static string DockerComposeFolderName { get; private set; }
        static DockerHelper()
        {
            DockerComposeFolderName = GetDockerComposeFolderName();
        }
        private static string GetDockerComposeFolderName()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment is null)
            {
                return "Docker";
            }

            return Path.Combine("Docker", environment);
        }
    }
}
