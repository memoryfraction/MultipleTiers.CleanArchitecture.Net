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
        }


        [TestMethod]
        public void TestMethod1()
        {
            using (var sp = _services.BuildServiceProvider())
            {
                var testService = sp.GetRequiredService<ITestService>();
                testService.PrintHelloWorld();
            }
        }
    }
}