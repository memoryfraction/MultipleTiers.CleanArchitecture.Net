using Microsoft.AspNetCore.Mvc;
using MultipleTiersArchitectureTemplate.BLL; // Added for ITestService

namespace MultipleTiersArchitectureTemplate.Webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITestService _testService; // Added field for ITestService
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public WeatherForecastController(ITestService testService) // Constructor injection
        {
            _testService = testService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Example usage of ITestService
            _testService.Test(); // Call the Test method to demonstrate usage
            
            // test purpose
            // throw new Exception("This is a test exception to trigger logging.");

            // write a log here for test
            // Serilog.Log.Error("test");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
