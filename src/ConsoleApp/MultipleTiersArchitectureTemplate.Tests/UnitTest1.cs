using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleTiersArchitectureTemplate.BLL;

namespace MultipleTiersArchitectureTemplate.Tests
{
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
}