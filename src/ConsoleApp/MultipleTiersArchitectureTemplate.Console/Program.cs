using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleTiersArchitectureTemplate.BLL;
using MultipleTiersArchitectureTemplate.BLL.Test.Services;
using Serilog;


namespace MultipleTiersArchitectureTemplate.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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


            #region Dependency Injection
            var serviceCollection = new ServiceCollection();

            #region Scoped
            serviceCollection.AddScoped<ITestService, TestService>();     //Injection ITestService to the container
            #endregion


            #region Singleton
            serviceCollection.AddSingleton<IConfiguration>(configuration);//Injection IConfiguration to the container

            // Register the Automapper to container
            serviceCollection.AddSingleton<IMapper>(sp =>
            {
                var autoMapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });
                return new Mapper(autoMapperConfiguration);
            });
            #endregion


            #endregion

            // Make use of IOC
            using (var sp = serviceCollection.BuildServiceProvider())
            {
                var testService = sp.GetRequiredService<ITestService>();
                testService.PrintHelloWorld();
                testService.PrintConfigInfo();
            }
        }


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                // Log the exception message and stack trace for detailed information
                Serilog.Log.Error(exception, $"A global unhandled exception occurred: {exception.Message}, StackTrace: {exception.StackTrace}");
                System.Console.WriteLine("A global unhandled exception occurred. Please check the log for details.");
            }
            else
            {
                Serilog.Log.Error("An unknown exception occurred.");
                System.Console.WriteLine("An unknown exception occurred. Please check the log for details.");
            }
        }
    }
}