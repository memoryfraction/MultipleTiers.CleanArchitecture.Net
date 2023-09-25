using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleTiersArchitectureTemplate.BLL;
using Serilog;


namespace MultipleTiersArchitectureTemplate.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()//define
                        .WriteTo.File(
                            path: $"{AppContext.BaseDirectory}Logs\\log.log",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 365)
                        .CreateLogger();

            // Test whether Log works
            Serilog.Log.Information($"Current utc datetime: + {DateTime.UtcNow}");


            // Build the configuration for config file, e.g. appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.stage.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddScoped<ITestService, TestService>();
            services.AddSingleton<IConfiguration>(configuration);
            using (var sp = services.BuildServiceProvider())
            {
                var testService = sp.GetRequiredService<ITestService>();
                testService.PrintHelloWorld();
            }
        }
    }
}