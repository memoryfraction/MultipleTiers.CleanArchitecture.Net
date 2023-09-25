using Microsoft.Extensions.Configuration;

namespace MultipleTiersArchitectureTemplate.BLL
{
    public class TestService : ITestService
    {
        private readonly IConfiguration _configuration;
        public TestService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PrintConfigInfo()
        {
            var title = _configuration["Position:Title"];
            var name = _configuration["Position:Name"];

            Console.WriteLine($"title:{title}");
            Console.WriteLine($"name:{name}");
        }

        public void PrintHelloWorld()
        {
            Console.WriteLine($"Hello world!");
        }
    }
}