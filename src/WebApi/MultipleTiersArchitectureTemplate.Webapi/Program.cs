
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

            // Register the TestService for Dependency Injection
            builder.Services.AddScoped<ITestService, TestService>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: $"{AppContext.BaseDirectory}Logs\\log.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 365)
                .CreateLogger();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}