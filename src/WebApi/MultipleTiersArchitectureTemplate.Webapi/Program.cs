
using Serilog;
using MultipleTiersArchitectureTemplate.BLL; // Added for ITestService and TestService
using MultipleTiersArchitectureTemplate.Webapi.Middleware;

namespace MultipleTiersArchitectureTemplate.Webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Build the configuration for config file
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.stage.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true)
                .Build();

            // Register the TestService for Dependency Injection
            builder.Services.AddScoped<ITestService, TestService>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: $"{AppContext.BaseDirectory}Logs\\log.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 365)
                .CreateLogger();

            // Log the current environment
            var environment = builder.Environment;
            Log.Information("Current environment: {EnvironmentName}", environment.EnvironmentName);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline for all environment
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
