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
    }
}