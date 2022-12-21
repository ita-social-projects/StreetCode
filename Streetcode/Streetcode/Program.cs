using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services.Interfaces;
using Services.Services;
using System;

// for Nuke
//environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//if (environment != "Local")
//{
//    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local", EnvironmentVariableTarget.Machine);
//}

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
    config.AddEnvironmentVariables("STREETCODE_");
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppServices();
builder.Services.AddDbContextPool<StreetcodeDBContext>(options => options.UseSqlServer("s"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<string>();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
