using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

public class Program
{
    static int Main(string[] args)
    {
        string rootDirectory = GetRootFolderPath();
        string pathToSqlScripts = Path.Combine(
            rootDirectory,
            "ScriptsMigration");

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(rootDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("STREETCODE_")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

       
        try
        {
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            return -1;
        }
    }

    private static string GetRootFolderPath()
    {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        if (codeBase == null)
        {
            throw new NullReferenceException("Cannot find root folder");
        }

        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path) !;
    }
}
