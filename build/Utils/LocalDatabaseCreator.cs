using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Nuke.Common.NukeBuild;
using System;
using System.Data;
using System.IO;

namespace Utils
{
    public static class LocalDatabaseCreator
    {
        static readonly string PATH_TO_APPSETTINGS = Path.Combine(RootDirectory, "Streetcode/Streetcode.WebApi");
        public static void CreateDatabase()
        {
            string connectionString = GetConnectionString();
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            sqlConnectionStringBuilder.InitialCatalog = string.Empty;

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            

            string Query = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Example') CREATE DATABASE [Example];";
            SqlCommand Command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private static string GetConnectionString()
        {
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "IntegrationTests";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .ConfigureCustom(environment);

            var configuration = configBuilder.Build();

            return configuration.GetConnectionString("DefaultConnection");
        }

        private static IConfigurationBuilder ConfigureCustom(this IConfigurationBuilder builder, string environment)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(PATH_TO_APPSETTINGS, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(PATH_TO_APPSETTINGS, $"appsettings.{environment}.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("STREETCODE_");

            return builder;
        }
    }
}
