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
            string migrationNamePattern = @"^\d*_\w*$";
            string DALProject = Path.Combine(RootDirectory, "Streetcode", "Streetcode.DAL");
            string APIProject = Path.Combine(RootDirectory, "Streetcode", "Streetcode.WebApi");
            var migrationsNames = EntityFrameworkMigrationsList(_ => _
                .SetProject(DALProject)
                .SetStartupProject(APIProject))
                .Where(line => Regex.IsMatch(line.Text, migrationNamePattern))
                .Select(line => line.Text)
                .ToList();

            return migrationsNames;
        }

        public static string GetLastMigrationFullName()
        {
            string lastMigrationFullName = GetAllMigrationsNames()
                .Last();

            return lastMigrationFullName;
        }
    }
}
