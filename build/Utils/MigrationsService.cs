using System.Collections.Generic;
using System.IO;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using static Nuke.Common.NukeBuild;
using Nuke.Common.Tools.EntityFramework;
using System.Linq;
using System.Text.RegularExpressions;
using Streetcode.DAL.Entities.Analytics;

namespace Utils
{
    public static class MigrationsService
    {
        public static IEnumerable<string> GetAllMigrationsNames()
        {
            string migrationNamePattern = GetMigrationNamePattern();
            DirectoryInfo migrationsDdirectory = new DirectoryInfo(GetMigrationFolderPath());
            IEnumerable<string> migrationNames = migrationsDdirectory
                .GetFiles()
                .Where(migrFile => Regex.IsMatch(migrFile.Name, migrationNamePattern))
                .Select(migrFile => migrFile.Name.Replace(".cs", string.Empty));

            return migrationNames;
        }

        public static string GetLastMigrationFullName()
        {
            string lastMigrationFullName = GetAllMigrationsNames()
                .Last();

            return lastMigrationFullName;
        }

        private static string GetMigrationFolderPath()
        {
            return RootDirectory / "Streetcode" / "Streetcode.DAL" / "Persistence" / "Migrations";
        }

        private static string GetMigrationNamePattern()
        {
            return @"^\d*_\w*.cs$";
        }
    }
}
