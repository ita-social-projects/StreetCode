using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static Utils.MigrationsService;
using static Nuke.Common.NukeBuild;

namespace Utils
{
    internal static class ScriptsService
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

        public static void DeleteMigrationScriptFile(string migrationFullName)
        {
            string scriptNamePattern = GetMigrationScriptNamePattern(migrationFullName);
            string scriptsMigrationFolderPath = GetScriptsMigrationFolderPath();

            DirectoryInfo scriptsDirectory = new DirectoryInfo(scriptsMigrationFolderPath);
            FileInfo[] allScriptFiles = scriptsDirectory.GetFiles("*.sql");
            FileInfo migrationScriptFile = allScriptFiles
                .FirstOrDefault(file => Regex.IsMatch(file.Name, scriptNamePattern));

            if (migrationScriptFile is not null)
            {
                File.Delete(migrationScriptFile.FullName);
            }
        }

        private static (string previousMigrationName, string actualMigrationName) GetMigrationInfoForScripts(string migrationName)
        {
            string migrNamePattern = $@"^\d*_{migrationName}$";

            IEnumerable<string> migrationNames = GetAllMigrationsNames();

            string previousMigrationName = migrationNames
                .TakeWhile(migrName => !Regex.IsMatch(migrName, migrNamePattern))
                .Last();

            string actualMigrationName = migrationNames
                .First(migrName => Regex.IsMatch(migrName, migrNamePattern));

            return (previousMigrationName, actualMigrationName);
        }

        private static string GetMigrationScriptNamePattern(string migrationFullName)
        {
            return @$"^Script_{migrationFullName}.sql$";
        }

        private static string GetScriptsMigrationFolderPath()
        {
            return RootDirectory / "Streetcode" / "Streetcode.DAL" / "Persistence" / "ScriptsMigration";
        }
    }
}
