
using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using RepositoryLayer;
using RepositoryLayer.Context;


var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Info("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    // NLog ko use karne ke liye configure 
  
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole(); //  console logging enabled

    builder.Services.AddControllers();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();  // ( var app = builder.Build(); ) Missing Registration Added
    builder.Services.AddScoped<IGreetingService, GreetingService>();

    // Database Connection


    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
    

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


   
     var app = builder.Build(); 
      

    //Swagger Enable

    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application failed to start!");
    throw;
}
finally
{
    LogManager.Shutdown(); 
}


