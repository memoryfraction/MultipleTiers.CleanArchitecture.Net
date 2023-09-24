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
                            path:$"{AppContext.BaseDirectory}Logs\\log.log", 
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit:365)
                        .CreateLogger();
            
            // Test whether Log works
            Serilog.Log.Information($"Current utc datetime: + {DateTime.UtcNow}");

            var services = new ServiceCollection();
            services.AddScoped<ITestService,TestService>();
            using (var sp = services.BuildServiceProvider())
            {
                var testService = sp.GetRequiredService<ITestService>();
                testService.PrintHelloWorld();
            }
        }
    }
}