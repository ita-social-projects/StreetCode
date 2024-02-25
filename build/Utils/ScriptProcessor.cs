using Microsoft.Extensions.Configuration;
using Nuke.Common.Tools.EntityFramework;
using System;
using System.IO;
using System.Linq;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using static Nuke.Common.NukeBuild;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Utils
{
    internal static class ScriptProcessor
    {
        public static bool IsScriptNameValid(string scriptName)
        {
            return !string.IsNullOrEmpty(scriptName.Trim());
        }

        public static void SetScriptParametersForSingleMigration(
            string migrationName,
            out string scriptName,
            out string fromMigration,
            out string toMigration)
        {
            (string previousMigrationFullName, string actualMigrationFullName) = GetMigrationInfoForScripts(migrationName);
            scriptName = $"Script_{actualMigrationFullName}";
            fromMigration = previousMigrationFullName;
            toMigration = actualMigrationFullName;
        }

        private static (string previousMigrationName, string actualMigrationName) GetMigrationInfoForScripts(string migrationName)
        {
            string migrNamePattern = $@"^\d*_{migrationName}$";
            string DALProject = Path.Combine(RootDirectory, "Streetcode", "Streetcode.DAL");
            string APIProject = Path.Combine(RootDirectory, "Streetcode", "Streetcode.WebApi");
            var commandOutputLines = EntityFrameworkMigrationsList(_ => _
                .SetProject(DALProject)
                .SetStartupProject(APIProject))
                .ToList();

            string previousMigrationName = commandOutputLines
                .TakeWhile(line => !Regex.IsMatch(line.Text, migrNamePattern))
                .Last().Text;

            string actualMigrationName = commandOutputLines
                .First(line => Regex.IsMatch(line.Text, migrNamePattern)).Text;

            return (previousMigrationName, actualMigrationName);
        }
    }
}
