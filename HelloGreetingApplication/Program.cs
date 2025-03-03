
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;


var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Info("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    // NLog ko use karne ke liye configure 
  
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole(); //  console logging is enabled

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Swagger Enable
   
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
