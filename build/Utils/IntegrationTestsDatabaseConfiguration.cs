using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Nuke.Common.NukeBuild;
using System;
using System.Data;
using System.IO;
using System.Threading;

namespace Utils
{
    public static class IntegrationTestsDatabaseConfiguration
    {
        static readonly string PATH_TO_FOLDER_WITH_APPSETTINGS = Path.Combine(RootDirectory, "Streetcode/Streetcode.WebApi");
        static readonly string CONNECTION_STRING;

        static IntegrationTestsDatabaseConfiguration()
        {
            CONNECTION_STRING = GetConnectionString();
        }
        public static void CreateDatabase()
        {
            string databaseName = GetDatabaseName();
            string query = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}];";
            SqlConnection sqlConnection = GetSqlConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            ExecuteCommand(command, sqlConnection);
        }

        public static string ConnectionString
        {
            get { return CONNECTION_STRING; }
        }

        private static void ExecuteCommand(SqlCommand command, SqlConnection sqlConnection)
        {
            DateTime startTime = DateTime.Now;
            do
            {
                try
                {
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            } while (TimeLimitNotExceeded(startTime));
        }

        private static bool TimeLimitNotExceeded(DateTime startDateTime)
        {
            return (DateTime.Now - startDateTime).TotalMinutes < 1;
        }

        private static SqlConnection GetSqlConnection()
        {
            var sqlConnectionBuilder = new SqlConnectionStringBuilder(CONNECTION_STRING);
            sqlConnectionBuilder.InitialCatalog = "master";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionBuilder.ConnectionString);
            return sqlConnection;
        }

        private static string GetDatabaseName()
        {
            var sqlConnectionBuilder = new SqlConnectionStringBuilder(CONNECTION_STRING);
            return sqlConnectionBuilder.InitialCatalog;
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
                .AddJsonFile(Path.Combine(PATH_TO_FOLDER_WITH_APPSETTINGS, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(PATH_TO_FOLDER_WITH_APPSETTINGS, $"appsettings.{environment}.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("STREETCODE_");

            return builder;
        }
    }
}
