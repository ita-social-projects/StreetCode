using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Streetcode.DAL.Persistence;
using Streetcode.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureClient(HttpClient client)
        {
            
            base.ConfigureClient(client);

        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            //builder.ConfigureServices(async services =>
            //{
            //    var dbContextDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbContextOptions<StreetcodeDbContext>));

            //    services.Remove(dbContextDescriptor);




            //    var dbConnectionDescriptor = services.SingleOrDefault(
            //        d => d.ServiceType ==
            //            typeof(DbConnection));

            //    services.Remove(dbConnectionDescriptor);
            //    File.WriteAllText("C:\\Users\\Надія\\Desktop\\connString2.txt", $"{DateTime.Now.Second}:{DateTime.Now.Millisecond}");
            //    services.AddSingleton<DbConnection>(container =>
            //    {
            //        var connection
            //        = new SqlConnection(

            //            // connectionString
            //             "Server=DESKTOP-I7Q35NQ\\SQLEXPRESS;Database=_TestStreetcodeDb;Trusted_Connection=True;TrustServerCertificate=True;User Id=sa;Password=Admin@1234;MultipleActiveResultSets=true;"
            //            );
            //        connection.Open();

            //        return connection;
            //    });

            //    services.AddDbContext<StreetcodeDbContext>((container, options) =>
            //    {

            //        var connection = container.GetRequiredService<DbConnection>();
            //        options.UseSqlServer(connection);

            //    });


            //});
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.IntegrationTests.json")
                    .Build();

                config.AddConfiguration(integrationConfig);
            });





        }
    }
}
