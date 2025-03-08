
using System.Text;
using BussinessLayer.Interface;
using BussinessLayer.Service;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
//using Microsoft.IdentityModel.Tokens;
using Middleware;
using NLog;
using RepositoryLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Helper;

//using RepositoryLayer.Helper;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using StackExchange.Redis;


var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Info("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    // JWT Authentication
    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //    .AddJwtBearer(options =>
    //    {
    //        options.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true,
    //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //            ValidAudience = builder.Configuration["Jwt:Audience"],
    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    //        };
    //    });

    // NLog ko use karne ke liye configure 

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole(); //  console logging enabled

    builder.Services.AddControllers();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();  // ( var app = builder.Build(); ) Missing Registration Added
    builder.Services.AddScoped<IGreetingService, GreetingService>();
    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    //builder.Services.AddScoped<JwtHelper>();
    builder.Services.AddScoped<PasswordHashing>();
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>(); // Register Global Exception Filter
    });

    builder.Services.AddScoped<GlobalExceptionFilter>(); // Ensure DI registration


    var redisConnectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value;
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
    builder.Services.AddSingleton<RedisCacheService>();



    // Database Connection

    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
    

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    

    var app = builder.Build();
    app.UseRouting();

    //Swagger Enable

    app.UseSwagger();
    app.UseSwaggerUI();

   // app.UseAuthentication();
    //app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers(); //  Controllers ko register kar raha hai(UserController)
    });

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


