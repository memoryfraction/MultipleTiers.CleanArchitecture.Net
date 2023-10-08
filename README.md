# MultipleTiers.CleanArchitecture.Net
The clean Architecture template based on .Net for console app
To avoid the duplicate works

# Features
- Inverse Of Control 
- Dependency Injection
- Multiple environment,e.g. appsetting.json, appsetting.production.json etc
- Serilog
- MS UnitTest Project with Multiple environment, IOC + DI

# Tiers
- BusinessLogicLayer(BLL)
- DataAccessLayer(DAL)
- MODEL
- Console
- Tests

# Steps to use?
1. Download the repo from the github
2. Replace the as-is project name to your expectation name
3. Enjoy!



# Sample code
## Program.cs
```C#
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
```

## Test
```C#
[TestClass]
public class UnitTest1
{
    ServiceCollection _services;

    public UnitTest1()
    {
        _services = new ServiceCollection();
        _services.AddScoped<ITestService, TestService>();

        // Build the configuration for config file, e.g. appsettings.json
        IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.stage.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true)
            .Build();
        _services.AddSingleton<IConfiguration>(_configuration);
    }


    [TestMethod]
    public void TestMethod_Appsettings_Production()
    {
        using (var sp = _services.BuildServiceProvider())
        {
            var testService = sp.GetRequiredService<ITestService>();
            testService.PrintHelloWorld();

            var config = sp.GetRequiredService<IConfiguration>();
            var title = config["Position:Title"];
            Assert.AreEqual("Senior Software Engineer", title);
        }
    }
}
```
