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
            //define Serilog Log
            Log.Logger = new LoggerConfiguration()
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


            // Define IOC
            var services = new ServiceCollection();
            services.AddScoped<ITestService, TestService>();     //Injection ITestService to the container
            services.AddSingleton<IConfiguration>(configuration);//Injection IConfiguration to the container

            // Make use of IOC
            using (var sp = services.BuildServiceProvider())
            {
                var testService = sp.GetRequiredService<ITestService>();
                testService.PrintHelloWorld();
                testService.PrintConfigInfo();
            }
        }
    }
}