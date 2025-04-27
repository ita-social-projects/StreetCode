using DbUp;
using Microsoft.Extensions.Configuration;

public class Program
{
    static int Main(string[] args)
    {
        string rootDirectory = GetRootFolderPath();
        string pathToSqlScripts = Path.Combine(
            rootDirectory,
            "Streetcode",
            "Streetcode.DAL",
            "Persistence",
            "ScriptsMigration");

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(rootDirectory, "Streetcode", "Streetcode.WebApi"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("STREETCODE_")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsFromFileSystem(pathToSqlScripts)
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return 0;
    }

    private static string GetRootFolderPath()
    {
        // By root folder we mean folder, that contains .gitignore file.
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        var directory = new DirectoryInfo(currentDirectoryPath);
        while (directory is not null && !directory.GetFiles(".gitignore").Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new NullReferenceException("Cannot find root folder");
    }
}