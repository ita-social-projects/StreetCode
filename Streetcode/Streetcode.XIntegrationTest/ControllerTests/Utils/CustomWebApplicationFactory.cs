using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Streetcode.DAL.Persistence;
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
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                   .UseEnvironment("IntegrationTests");

            builder.ConfigureServices(async services =>
            {

                builder.ConfigureAppConfiguration(
                    async (builderContext, configBuilder) =>
                    {

                        var dbContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<StreetcodeDbContext>));

                        services.Remove(dbContextDescriptor);

                        var dbConnectionDescriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbConnection));

                        services.Remove(dbConnectionDescriptor);

                        services.AddSingleton<DbConnection>(container =>
                        {
                            var connection
                            = new SqlConnection(builderContext.Configuration.GetConnectionString("DefaultTestConnection"));
                            connection.Open();

                            return connection;
                        });

                        services.AddDbContext<StreetcodeDbContext>((container, options) =>
                        {

                            var connection = container.GetRequiredService<DbConnection>();
                            options.UseSqlServer(connection,opt=>opt.EnableRetryOnFailure());
                            
                        });

                        

                        //using (var scope = services.BuildServiceProvider().CreateScope())
                        //{
                        //    using (var appContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>())
                        //    {
                        //        try
                        //        {
                        //            Console.WriteLine("\n\n\n\nyyyyyyyyy\n");
                        //            appContext.Database.EnsureCreated();
                        //            var streetcodeContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
                        //            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

                        //            var scriptFiles = Directory.GetFiles($"../../../../Streetcode.DAL/Persistence/Scripts/");

                        //            await streetcodeContext.Database.EnsureDeletedAsync();
                        //            await streetcodeContext.Database.MigrateAsync();

                        //            //var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));

                        //            //foreach (var task in filesContexts)
                        //            //{
                        //            //    await streetcodeContext.Database.ExecuteSqlRawAsync(task);
                        //            //}
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine("\n\n\n\nereeerr\n");
                        //            throw;
                        //        }
                        //    }
                        //}
                    });





            });


        

        }
    }
}
