using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleTiersArchitectureTemplate.BLL;
using MultipleTiersArchitectureTemplate.BLL.Test.Models;
using MultipleTiersArchitectureTemplate.BLL.Test.Services;
using MultipleTiersArchitectureTemplate.DAL.DataAccessModels;

namespace MultipleTiersArchitectureTemplate.Tests
{
    [TestClass]
    public class UnitTest1
    {
        ServiceProvider _serviceProvider;
        ServiceCollection _serviceCollection;

        public UnitTest1()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddScoped<ITestService, TestService>();

            // Build the configuration for config file, e.g. appsettings.json
            IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.stage.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true)
                .Build();
            _serviceCollection.AddSingleton<IConfiguration>(_configuration);

            // Register the Automapper to container
            _serviceCollection.AddSingleton<IMapper>(sp =>
            {
                var autoMapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });
                return new Mapper(autoMapperConfiguration);
            });

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }


        [TestMethod]
        public void TestMethod_Appsettings_Production()
        {
            var testService = _serviceProvider.GetRequiredService<ITestService>();
            testService.PrintHelloWorld();

            var config = _serviceProvider.GetRequiredService<IConfiguration>();
            var title = config["Position:Title"];
            Assert.AreEqual("Senior Software Engineer", title);
            
        }


        [TestMethod]
        public void ModelsMapping_Should_Work()
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            var personBLLModel = GeneratePersonalBLLModel();
            var personDALModel = mapper.Map<PersonDALModel>(personBLLModel);

            Assert.IsNotNull(personDALModel);
        }

        private PersonBLLModel GeneratePersonalBLLModel()
        {
            var model = new PersonBLLModel();

            model.Id = 1;
            model.Name = "John Smith";

            return model;
        }

    }
}